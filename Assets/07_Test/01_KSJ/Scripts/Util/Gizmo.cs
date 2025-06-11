using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gizmo : MonoBehaviour
{
    
    
    void OnDrawGizmos()
    {
        Gizmos.DrawIcon(transform.position, "WaypointGizmo.png", true);
    }
}
