using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TeleportOnScene : MonoBehaviour
{
    [SerializeField]
    private Vector3 teleportPoint;
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            TransitionToLevel(other.gameObject);
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            //_difMenu.GetComponent<HUDManager>().ShowOrCloseWindow();
        }
    }

    private void TransitionToLevel(GameObject teleportObject)
    {
        Debug.Log("Teleport to training" + teleportObject);

        Debug.Log(teleportPoint);
        teleportObject.transform.position = teleportPoint;
    }
}
