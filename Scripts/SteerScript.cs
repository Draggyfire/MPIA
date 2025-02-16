using UnityEngine;

public class SteerScript : MonoBehaviour
{
    public Transform target; 
    public Transform fleeTarget;
    public Transform pursuitTarget;
    public Transform evadeTarget;
    public float maxSpeed = 5f; 
    public float maxSteeringForce = 0.5f;
    public float arrivalRadius = 2f; 
    public float fleeRange= 5f;
    public float evadeRange = 5f;

    private Vector2 velocity;
    private Vector3 initVelo = new Vector3(0,1,0);

    Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("❌ Rigidbody2D manquant sur la cible !");
        }
    }

    void Forward()
    {
        if (rb == null) return; 

        // Déplacement en utilisant la vélocité (au lieu de transformer directement la position)
        rb.velocity = transform.up * maxSpeed;
    }


    public void OnClick()
    {
        //Debug.Log(Mouse.current.position.value);
        var coord = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,Input.mousePosition.y,Camera.main.nearClipPlane));
        coord.z = 0;
        target.position = coord;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0)&&target!=null){
            OnClick();
        }
        if(pursuitTarget!=null)
            Pursuit();
        if(fleeTarget!=null)
            Flee();
        if(target!=null)
            Steer();
        if (evadeTarget != null)
            Evade();
        if(target==null&&pursuitTarget==null)
            Forward();
        
    }

    void Steer(){
        Vector2 currentPosition = transform.position;
        Vector2 targetPosition = target.position;
        Vector2 direction = targetPosition - currentPosition;
        float distance = direction.magnitude;

        float speed = maxSpeed;
        
        if (distance < arrivalRadius)
        {
            speed = Mathf.Lerp(0, maxSpeed, distance / arrivalRadius); 
        }
        

        Vector2 desiredVelocity = direction.normalized * speed;
        Vector2 steering = desiredVelocity - velocity;
        steering = Vector2.ClampMagnitude(steering, maxSteeringForce); // Limiter la force de steering

        velocity += steering * Time.deltaTime;
        velocity = Vector2.ClampMagnitude(velocity, maxSpeed); // Limiter la vitesse à la vitesse maximale
        transform.up += (Vector3)velocity;
        transform.position += transform.up * speed * Time.deltaTime;
    }

    void Flee(){
        Vector2 currentPosition = transform.position;
        Vector2 targetPosition = fleeTarget.position;
        Vector2 direction = targetPosition - currentPosition;
        float distance = direction.magnitude;
        if(distance > fleeRange)return;

        float speed = maxSpeed;
        if (distance < arrivalRadius)
        {
            speed = Mathf.Lerp(0, maxSpeed, distance / arrivalRadius); 
        }

        Vector2 desiredVelocity = direction.normalized * speed;

        Vector2 fleeing = -desiredVelocity - velocity;
        fleeing = Vector2.ClampMagnitude(fleeing, maxSteeringForce); // Limiter la force de steering

        velocity += fleeing * Time.deltaTime;
        velocity = Vector2.ClampMagnitude(velocity, maxSpeed); // Limiter la vitesse à la vitesse maximale

        transform.up += (Vector3)velocity;
        transform.position += transform.up * speed * Time.deltaTime;
    }
    void Pursuit()
    {
        Vector2 currentPosition = transform.position;
        Vector2 targetPosition = pursuitTarget.position;

        // Récupérer la vitesse et direction de la cible
        Rigidbody2D targetRigidbody = pursuitTarget.GetComponent<Rigidbody2D>();
        Vector2 targetVelocity = targetRigidbody != null ? targetRigidbody.velocity : Vector2.zero;
        float targetSpeed = targetVelocity.magnitude;

        float distance = Vector2.Distance(currentPosition, targetPosition);

        // plus la cible est rapide plus on prédit loin
        float basePredictionTime = distance / maxSpeed;
        float speedFactor = Mathf.Clamp(targetSpeed / maxSpeed, 0.5f, 3f);
        float predictionTime = basePredictionTime * speedFactor * 2.5f;
      

        predictionTime = Mathf.Clamp(predictionTime, 0.5f, 5f); 

        Vector2 predictedPosition = targetPosition + targetVelocity * predictionTime;

        Vector2 direction = (predictedPosition - currentPosition).normalized;
        Vector2 desiredVelocity = direction * maxSpeed;
        Vector2 steering = desiredVelocity - velocity;
        steering = Vector2.ClampMagnitude(steering, maxSteeringForce);
        Debug.DrawLine(transform.position, predictedPosition, Color.red);

        velocity += steering * Time.deltaTime;
        velocity = Vector2.ClampMagnitude(velocity, maxSpeed);

        if (velocity.magnitude > 0.1f)
        {
            transform.up = velocity.normalized;
        }
        transform.position += (Vector3)velocity * Time.deltaTime;
    }

    void Evade()
    {
        Vector2 targetPosition = evadeTarget.position;
        Rigidbody2D targetRigidbody = evadeTarget.GetComponent<Rigidbody2D>();
        Vector2 targetVelocity = targetRigidbody != null ? targetRigidbody.velocity : Vector2.zero;

        float distance = Vector2.Distance(transform.position, targetPosition);
        if(distance < evadeRange)
        {
            float predictionTime = Mathf.Clamp(distance / (maxSpeed + targetVelocity.magnitude), 0.5f, 5f);
            Vector2 predictedPosition = targetPosition + targetVelocity * predictionTime;

            Vector2 fleeDirection = (Vector2)transform.position - predictedPosition;
            fleeDirection.Normalize();
            Vector2 desiredVelocity = fleeDirection * maxSpeed;

            Vector2 steering = desiredVelocity - velocity;
            steering = Vector2.ClampMagnitude(steering, maxSteeringForce);

            velocity += steering * Time.deltaTime;
            velocity = Vector2.ClampMagnitude(velocity, maxSpeed);

            transform.up = velocity.normalized;
            transform.position += (Vector3)velocity * Time.deltaTime;
        }
        
    }


    void OnDrawGizmos()
    {
        if (pursuitTarget != null)
        {
         // Récupérer la position actuelle et la vitesse de la cible
            Vector2 targetPosition = pursuitTarget.position;
            Rigidbody2D targetRigidbody = pursuitTarget.GetComponent<Rigidbody2D>();
            Vector2 targetVelocity = targetRigidbody != null ? targetRigidbody.velocity : Vector2.zero;
            Debug.Log(targetVelocity);
            float targetSpeed = targetVelocity.magnitude;

            float distance = Vector2.Distance(transform.position, targetPosition);

            // Facteur de prédiction
            float basePredictionTime = distance / maxSpeed;
            float speedFactor = Mathf.Clamp(targetSpeed / maxSpeed, 0.5f, 3f);
            float predictionTime = basePredictionTime * speedFactor * 2.5f;

            if (Vector2.Dot(targetVelocity, (targetPosition - (Vector2)transform.position).normalized) < 0)
            {
                predictionTime *= 1.8f;
            }

            predictionTime = Mathf.Clamp(predictionTime, 0.5f, 5f);

            // Calcul de la position future prédite
            Vector2 predictedPosition = targetPosition + targetVelocity * predictionTime;

            // Dessiner une ligne entre l'ennemi et la position prédite
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, predictedPosition);

            // Dessiner un cercle pour marquer la position ciblée
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(predictedPosition, 0.3f);
        }else if (evadeTarget!=null)
        {
            // Récupérer la position et la vitesse de la cible
            Vector2 targetPosition = evadeTarget.position;
            Rigidbody2D targetRigidbody = evadeTarget.GetComponent<Rigidbody2D>();
            Vector2 targetVelocity = targetRigidbody != null ? targetRigidbody.velocity : Vector2.zero;

            // Calcul de la distance et du temps de prédiction
            float distance = Vector2.Distance(transform.position, targetPosition);
            float predictionTime = Mathf.Clamp(distance / (maxSpeed + targetVelocity.magnitude), 0.5f, 5f);
            Vector2 predictedPosition = targetPosition + targetVelocity * predictionTime;

            // Convertir transform.position en Vector2
            Vector2 fleeDirection = (Vector2)transform.position - predictedPosition;

            // Dessiner la position prédite de la cible
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(predictedPosition, 0.2f); // Point rouge représentant la position future

            // Dessiner la ligne de l'objet à la position prédite
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, predictedPosition); // Ligne verte de l'objet vers la position prédite

            // Dessiner la direction d'évasion (flèche)
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(predictedPosition, predictedPosition + fleeDirection.normalized * 2); // Flèche bleue dans la direction opposée
        }

       
    }
    





}