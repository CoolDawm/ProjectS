using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.AssemblyQualifiedNameParser;
using UnityEngine;
using UnityEngine.SceneManagement;
public class TransitionToNewLevel : MonoBehaviour
{
    private string _sceneName;
    private string _a;
    private string _b;
    private GameObject _difMenu;
    void Start()
    {
        _sceneName=SceneManager.GetActiveScene().name;
        Debug.Log(_sceneName);
        double number = char.GetNumericValue(_sceneName[5]);
        double number2 = char.GetNumericValue(_sceneName[6]);
        number2++;
        if (number2 == 10)
        {
            number++;
            number2 = 0;
        }
        string _a = number.ToString();
        string _b = number2.ToString();
        _sceneName = "Level" + _a + _b;
       
        _difMenu=GameObject.FindGameObjectWithTag("PlayerHUD");
        GameObject.FindGameObjectWithTag("DifficultyManager").GetComponent<DifficultyManager>().OnDifChange+=TransToLEvel;
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            _difMenu.GetComponent<HUDManager>().ShowOrCloseWindow();
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            _difMenu.GetComponent<HUDManager>().ShowOrCloseWindow();
        }
    }

    private void TransToLEvel()
    {
        _difMenu.GetComponent<HUDManager>().ShowOrCloseWindow();
        SceneManager.LoadScene(_sceneName);
    }
}
