using UnityEngine;

public class TwoWayCircuit : MonoBehaviour
{
    public Transform[] waypoints;  // Liste des waypoints
    private int currentWaypointIndex = 0;  // Index du waypoint actuel
    public float maxSpeed = 5f;  // Vitesse maximale du mouvement
    public float maxSteeringForce = 10f;  // Force maximale pour le steering
    public float waypointRadius = 1f;  // Rayon de détection du waypoint
    public float stopRadius = 0.3f;  // Rayon pour stopper l'objet au centre du dernier waypoint
    private Vector2 velocity;  // Vitesse actuelle de l'objet
    private bool isStopped = false;  // Variable pour vérifier si l'objet est arrêté
    private bool goingForward = true;  // Direction actuelle (true = forward, false = backward)

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

        // Si on est proche du waypoint, appliquer Arrival (uniquement pour le dernier waypoint)
        if (distanceToTarget < waypointRadius)
        {
            UpdateWaypointIndex(); // Mettre à jour l'index du waypoint en fonction de la direction

            // Si on atteint un waypoint d'arrêt, on peut arrêter
            if (currentWaypointIndex == waypoints.Length - 1 || currentWaypointIndex == 0)
            {
                // Si on atteint le dernier waypoint ou le premier waypoint, on arrête
                if (distanceToTarget <= stopRadius)
                {
                    velocity = Vector2.zero;
                    isStopped = true;  // Marque que l'objet est arrêté
                }
            }
        }
        else
        {
            // Déplacement normal pour les waypoints
            Vector2 desiredVelocity = directionToTarget * maxSpeed;

            // Calculer la force de steering
            Vector2 steering = desiredVelocity - velocity;
            steering = Vector2.ClampMagnitude(steering, maxSteeringForce);

            // Appliquer la force de steering à la vélocité
            velocity += steering * Time.deltaTime;
            velocity = Vector2.ClampMagnitude(velocity, maxSpeed);
        }

        // Appliquer le déplacement
        transform.position += (Vector3)velocity * Time.deltaTime;

        // Si l'objet n'est pas arrêté, ajuster la rotation pour suivre la direction
        if (!isStopped)
        {
            transform.up = velocity.normalized;
        }
    }

    // Mettre à jour l'index du waypoint en fonction de la direction
    void UpdateWaypointIndex()
    {
        // Si on va dans le sens normal (avant)
        if (goingForward)
        {
            currentWaypointIndex++;
            // Si on atteint le dernier waypoint, on inverse la direction
            if (currentWaypointIndex >= waypoints.Length)
            {
                currentWaypointIndex = waypoints.Length - 2;  // Revenir au second dernier waypoint
                goingForward = false;  // Changer de direction
            }
        }
        // Si on va dans le sens inverse (retour)
        else
        {
            currentWaypointIndex--;
            // Si on atteint le premier waypoint, on inverse à nouveau la direction
            if (currentWaypointIndex < 0)
            {
                currentWaypointIndex = 1;  // Revenir au second waypoint
                goingForward = true;  // Revenir à la direction initiale
            }
        }
    }

    // Fonction Arrival pour gérer l'arrivée douce au dernier waypoint
    void ArrivalBehavior(float distanceToTarget, Vector2 directionToTarget)
    {
        if (distanceToTarget < stopRadius)
        {
            // Calcule une vitesse réduite pour ralentir progressivement à l'arrivée
            velocity = Vector2.zero;  // Arrêter complètement l'objet
            isStopped = true;  // Marque que l'objet est arrêté
        }
        else
        {
            // Si on est loin du dernier waypoint, se déplacer normalement
            Vector2 desiredVelocity = directionToTarget * maxSpeed;

            // Calculer la force de steering
            Vector2 steering = desiredVelocity - velocity;
            steering = Vector2.ClampMagnitude(steering, maxSteeringForce);

            // Appliquer la force de steering à la vélocité
            velocity += steering * Time.deltaTime;
            velocity = Vector2.ClampMagnitude(velocity, maxSpeed);
        }
    }
}
