using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RoomObstacle : MonoBehaviour
{
    private Collider _obstacle;

    //private NavMeshObstacle _navObstacle;
    void Start()
    {
        _obstacle = GetComponent<Collider>();
        OpenObstacle();
      //  _navObstacle = GetComponent<NavMeshObstacle>();
    }

    public void CloseObstacle()
    {
        _obstacle.enabled = true;
    }
    public void OpenObstacle()
    {
        _obstacle.enabled = false;
    }
    
}
