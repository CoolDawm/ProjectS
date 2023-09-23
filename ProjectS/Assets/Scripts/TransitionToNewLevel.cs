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
    }
    public void OnTriggerEnter(Collider other)
    {
        Debug.Log("TransiTry");
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("Complete");
            SceneManager.LoadScene(_sceneName);
        }
    }
}
