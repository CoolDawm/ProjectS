using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionToNamedScene : MonoBehaviour
{
    [SerializeField]
    private string _sceneName;
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            TransitionToLevel();
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            //_difMenu.GetComponent<HUDManager>().ShowOrCloseWindow();
        }
    }

    private void TransitionToLevel()
    {
        SceneManager.LoadScene(_sceneName);
    }
}
