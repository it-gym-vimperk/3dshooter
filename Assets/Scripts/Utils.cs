using UnityEngine;
using UnityEngine.AI;

public static class Utils
{
    public static Vector3 GetRandomPointOnNavMesh(Vector3 center, float range)
    {
        // Generate a random point within the specified range
        Vector3 randomDirection = Random.insideUnitSphere * range;
        randomDirection += center;

        // Try to find the nearest point on the NavMesh to the random point
        if (NavMesh.SamplePosition(randomDirection, out var hit, range, NavMesh.AllAreas))
        {
            return hit.position; // Return the point on the NavMesh
        }

        Debug.Assert(false);
        return Vector3.zero; // Return zero if no valid point found
    }
}
