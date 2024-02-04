using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManagement : MonoBehaviour
{
    [SerializeField] private RoomObstacle[] rooms;
    private List<GameObject> enemies=new List<GameObject>();
    public float enemyCount = 0;
    private bool playerHere=false;
    
    void Update()
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            if (enemies[i] == null)
            {
                enemies.Remove(enemies[i]);
                enemyCount--;
            }
        }
        if (enemyCount > 0&& playerHere)
        {
            foreach (RoomObstacle ro in rooms)
            {
                ro.CloseObstacle();
            }
        }
        else
        {
            foreach (RoomObstacle ro in rooms)
            {
                ro.OpenObstacle();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") && other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            enemies.Add(other.gameObject);
            enemyCount++;
        }else if (other.CompareTag("Player"))
        {
            playerHere = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        
        if (other.CompareTag("Enemy") && other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            enemyCount--;
        }
    }
}
