using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class TraceMovement : MonoBehaviour
{
    public Transform target; // Cible que le transform doit suivre
    public float rotationSpeed = 5f; // Vitesse de rotation
    public float moveSpeed = 3f; // Vitesse de déplacement

    private LineRenderer lineRenderer;
    private List<Vector3> points = new List<Vector3>();

    void Start()
    {
        // Récupérer le LineRenderer attaché
        lineRenderer = GetComponent<LineRenderer>();

        // Ajouter la position de départ
        points.Add(transform.position);
        UpdateLineRenderer();
    }

    void Update()
    {
        if (target == null) return;

        // Déplacer le transform comme avant
        Vector3 directionToTarget = (target.position - transform.position).normalized;
        Vector3 newUp = Vector3.Lerp(transform.up, directionToTarget, Time.deltaTime * rotationSpeed);
        transform.up = newUp;
        transform.position += transform.up * moveSpeed * Time.deltaTime;

        // Ajouter la nouvelle position si elle a changé
        if (points.Count == 0 || Vector3.Distance(points[points.Count - 1], transform.position) > 0.1f)
        {
            points.Add(transform.position);
            UpdateLineRenderer();
        }
    }

    void UpdateLineRenderer()
    {
        lineRenderer.positionCount = points.Count;
        lineRenderer.SetPositions(points.ToArray());
    }
}
