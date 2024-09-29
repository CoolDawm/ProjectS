using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionToNewLevel : MonoBehaviour
{
    private string _sceneName;
    private GameObject _difMenu;

    void Start()
    {
        _sceneName = SceneManager.GetActiveScene().name;
        InitializeSceneName();

        _difMenu = GameObject.FindGameObjectWithTag("PlayerHUD");
        GameObject.FindGameObjectWithTag("DifficultyManager").GetComponent<DifficultyManager>().OnDifChange += TransToLevel;
    }

    private void InitializeSceneName()
    {
        // Логика для инициализации названия следующего уровня
        double number = char.GetNumericValue(_sceneName[5]);
        double number2 = char.GetNumericValue(_sceneName[6]);
        number2++;
        if (number2 == 10)
        {
            number++;
            number2 = 0;
        }
        _sceneName = "Level" + number.ToString() + number2.ToString();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _difMenu.GetComponent<HUDManager>().ShowOrCloseWindow();
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _difMenu.GetComponent<HUDManager>().ShowOrCloseWindow();
        }
    }

    private void TransToLevel()
    {
        _difMenu.GetComponent<HUDManager>().ShowOrCloseWindow();
        StartCoroutine(LoadSceneAsync(_sceneName));
    }

    IEnumerator LoadSceneAsync(string levelName)
    {
        AsyncOperation loadSceneOperation = SceneManager.LoadSceneAsync(levelName);
        yield return null;
    }
}
