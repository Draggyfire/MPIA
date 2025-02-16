using Unity.VisualScripting;
using UnityEngine;

public class OneWayCircuit : MonoBehaviour
{
    public Transform[] waypoints;  // Liste des waypoints
    private int currentWaypointIndex = 0;  // Index du waypoint actuel
    public float maxSpeed = 5f;  // Vitesse maximale du mouvement
    public float maxSteeringForce = 10f;  // Force maximale pour le steering
    public float waypointRadius = 1f;  // Rayon de détection du waypoint
    public float stopRadius = 0.3f;  // Rayon pour stopper l'objet au centre du dernier waypoint
    private Vector2 velocity;  // Vitesse actuelle de l'objet
    private bool isStopped = false;  // Variable pour vérifier si l'objet est arrêté
    private float arrivalRadius = 1.5f;

    void Update()
    {
        // Si aucun waypoint n'est défini, ne rien faire
        if (waypoints.Length == 0) return;

        // Si l'objet est déjà arrêté, ne pas continuer à appliquer de mouvement
        if (isStopped)
        {
            return;
        }

        // Calculer la position actuelle du waypoint
        Transform targetWaypoint = waypoints[currentWaypointIndex];
        Vector2 targetPosition = targetWaypoint.position;
        Vector2 currentPosition = transform.position;
        Vector2 directionToTarget = (targetPosition - currentPosition).normalized;
        float distanceToTarget = Vector2.Distance(currentPosition, targetPosition);

        if (distanceToTarget < waypointRadius)
        {
            if (currentWaypointIndex == waypoints.Length - 1)
            {
                if (distanceToTarget <= stopRadius)
                {
                    velocity = Vector2.zero;
                    isStopped = true; 
                }
                else
                {
                    ArrivalBehavior(distanceToTarget, directionToTarget);
                }
            }
            else
            {
                currentWaypointIndex++; 
            }
        }
        else
        {
            // Déplacement normal pour les waypoints autres que le dernier
            Vector2 desiredVelocity = directionToTarget * maxSpeed;

            Vector2 steering = desiredVelocity - velocity;
            steering = Vector2.ClampMagnitude(steering, maxSteeringForce);

            velocity += steering * Time.deltaTime;
            velocity = Vector2.ClampMagnitude(velocity, maxSpeed);
        }

        transform.position += (Vector3)velocity * Time.deltaTime;

        if (!isStopped)
        {
            transform.up = velocity.normalized;
        }
    }

    // Fonction Arrival pour gérer l'arrivée douce au dernier waypoint
    void ArrivalBehavior(float distanceToTarget, Vector2 directionToTarget)
    {
        float speed = maxSpeed;
        if (distanceToTarget < arrivalRadius)
        {
            speed = Mathf.Lerp(0, maxSpeed, distanceToTarget / arrivalRadius);
        }else if (distanceToTarget < stopRadius)
        {
            // Calcule une vitesse réduite pour ralentir progressivement à l'arrivée
            velocity = Vector2.zero;  
            isStopped = true;
            return;
        }
      
        // Si on est loin du dernier waypoint, se déplacer normalement
        Vector2 desiredVelocity = directionToTarget * maxSpeed;

        // Calculer la force de steering
        Vector2 steering = desiredVelocity - velocity;
        steering = Vector2.ClampMagnitude(steering, maxSteeringForce);

        // Appliquer la force de steering à la vélocité
        velocity += steering * Time.deltaTime;
        velocity = Vector2.ClampMagnitude(velocity, speed);
       
    }
}
