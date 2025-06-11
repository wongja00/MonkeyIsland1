using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AYellowpaper.SerializedCollections;

public class LookToBuilding : MonoBehaviour
{
    [SerializeField]
    private Transform target;
    
    [SerializeField]
    private Camera mainCamera;
    
    public void SetTarget()
    {
        mainCamera.transform.position = new Vector3(target.position.x + 2, mainCamera.transform.position.y, mainCamera.transform.position.z);
    }
}
