using UnityEngine;

public class Circuit : MonoBehaviour
{
    public Transform[] waypoints;  // Liste des waypoints
    private int currentWaypointIndex = 0;  // Index du waypoint actuel
    public float maxSpeed = 5f;  // Vitesse maximale du mouvement
    public float maxSteeringForce = 10f;  // Force maximale pour le steering
    public float waypointRadius = 1f;  // Rayon de détection du waypoint
    private Vector2 velocity;  // Vitesse actuelle de l'objet

    void Update()
    {
        // Si aucun waypoint n'est défini
        if (waypoints.Length == 0) return;

        // Calculer la position actuelle du waypoint
        Transform targetWaypoint = waypoints[currentWaypointIndex];

        // Calculer la direction et la distance
        Vector2 targetPosition = targetWaypoint.position;
        Vector2 currentPosition = transform.position;
        Vector2 directionToTarget = (targetPosition - currentPosition).normalized;
        float distanceToTarget = Vector2.Distance(currentPosition, targetPosition);

        // Si on est proche du waypoint, passer au suivant
        if (distanceToTarget < waypointRadius)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length; // Passer au prochain waypoint
        }

        // Calculer la vélocité désirée
        Vector2 desiredVelocity = directionToTarget * maxSpeed;

        // Calculer la force de steering
        Vector2 steering = desiredVelocity - velocity;
        steering = Vector2.ClampMagnitude(steering, maxSteeringForce);

        // Appliquer la force de steering à la vélocité
        velocity += steering * Time.deltaTime;
        velocity = Vector2.ClampMagnitude(velocity, maxSpeed);

        // Appliquer le déplacement
        transform.position += (Vector3)velocity * Time.deltaTime;

        // Ajuster la rotation pour suivre la direction
        transform.up = velocity.normalized;
    }
}
