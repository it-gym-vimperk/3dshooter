using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class TrajectoryVisualizer : MonoBehaviour
{
    public Transform targetVisual;
    public Transform shootingPoint; // The point where the projectile will be shot from
    public float launchForce = 20f; // Force applied to the projectile
    public float drag = 0.5f;
    public int resolution = 100; // Number of trajectory points
    public float timeStep = 0.1f; // Time between trajectory points

    private LineRenderer lineRenderer;

    private List<Vector3> points;
    void Start()
    {
        points = new List<Vector3>();
        lineRenderer = GetComponent<LineRenderer>();
    }

    public void UpdateLaunchForce(float force)
    {
        launchForce = force;
    }
    
    private void Update()
    {
        Vector3 initialVelocity = shootingPoint.forward * launchForce;
        RenderTrajectory(shootingPoint.position, initialVelocity);
    }

    private void RenderTrajectory(Vector3 startPoint, Vector3 initialVelocity)
    {
        Vector3[] trajectoryPoints = CalculateTrajectoryPoints(startPoint, initialVelocity);
        lineRenderer.positionCount = trajectoryPoints.Length;
        lineRenderer.SetPositions(trajectoryPoints);
    }

    private void OnDisable()
    {
        points.Clear();
        lineRenderer.positionCount = 0;
    }

    private Vector3[] CalculateTrajectoryPoints(Vector3 startPoint, Vector3 initialVelocity)
    {
        points.Clear();
        Vector3 currentPosition = startPoint;
        Vector3 velocity = initialVelocity;
        targetVisual.gameObject.SetActive(false);
        
        for (int i = 0; i < resolution; i++)
        {
            if (i > 1)
            {
                if (Physics.Raycast(
                        points[^2], points[^1] - points[^2], 
                        out var info, (points[^1] - points[^2]).magnitude))
                {
                    if (!info.collider.CompareTag("Bullet"))
                    {
                        targetVisual.position = info.point + new Vector3(0, 0.01f, 0);
                        targetVisual.gameObject.SetActive(true);
                        points[^1] = targetVisual.position;
                        break;
                    }
                }
            }
            points.Add(currentPosition);

            // Apply drag (if applicable) and gravity
            velocity += Physics.gravity * timeStep; // Add gravity
            velocity *= 1f - drag* timeStep; // Apply drag
            currentPosition += velocity * timeStep;
        }
        return points.ToArray();
    }
}