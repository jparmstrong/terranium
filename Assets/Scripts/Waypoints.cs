using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoints : MonoBehaviour
{

    public float waypointSize = 0.5f;
    public Color waypointColor = Color.red;
    private bool atFirstWaypoint = false;
    // from tutorial https://www.youtube.com/watch?v=EwHiMQ3jdHw
    private void OnDrawGizmos()
    {

        foreach (Transform t in transform)
        {
            Gizmos.color = waypointColor;
            Gizmos.DrawWireSphere(t.position, waypointSize);
        }

        for (int i = 0; i < transform.childCount - 1; i++)
        {
            Gizmos.DrawLine(transform.GetChild(i).position, transform.GetChild(i + 1).position);
        }
        Gizmos.DrawLine(transform.GetChild(transform.childCount - 1).position, transform.GetChild(0).position);
    }

    public Transform GetNextWaypoint(Transform currentWaypoint)
    {
        if (currentWaypoint == null)
        {
            atFirstWaypoint = true;
            return transform.GetChild(0);
        }

        if (currentWaypoint.GetSiblingIndex() < transform.childCount - 1)
        {
            atFirstWaypoint = false;
            return transform.GetChild(currentWaypoint.GetSiblingIndex() + 1);
        }

        atFirstWaypoint = true;
        return transform.GetChild(0);
    }

    public bool AtFirstWaypoint()
    {
        return atFirstWaypoint;
    }

    public Transform GetRandomWaypoint(Transform currentWaypoint)
    {
        Transform waypoint = null;
        do
        {
            waypoint = transform.GetChild(Random.Range(0, transform.childCount - 1));
        } while (waypoint != currentWaypoint || transform.childCount < 2);

        return waypoint;
    }

}
