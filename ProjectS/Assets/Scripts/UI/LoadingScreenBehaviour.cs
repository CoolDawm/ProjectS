using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScreenBehaviour : MonoBehaviour
{
    [SerializeField]
    private Image _loadingIndicator;
    [SerializeField]
    private float _loadingTime = 3f; 

    void Start()
    {
        StartCoroutine(LoadingCoroutine());
    }

    IEnumerator LoadingCoroutine()
    {
        float elapsedTime = 0f;
        while (elapsedTime < _loadingTime)
        {
            elapsedTime += Time.deltaTime; 
            _loadingIndicator.fillAmount = Mathf.Clamp01(elapsedTime / _loadingTime); 
            yield return null; 
        }

        
        OnLoadingComplete();
    }

    void OnLoadingComplete()
    {
        Debug.Log("Загрузка завершена!");
        gameObject.SetActive(false);
    }
}
