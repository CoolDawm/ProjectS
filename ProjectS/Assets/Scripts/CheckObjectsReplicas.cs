using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckObjectsReplicas : MonoBehaviour
{
    void Start()
    {
        RemoveDuplicateObjects();
    }

    void RemoveDuplicateObjects()
    {
        RemoveDuplicatesByTag("Player");
        RemoveDuplicatesByTag("FreeLookCamera");
        RemoveDuplicatesByTag("PlayerHUD");
        RemoveDuplicatesByTag("CoroutineRunner");
        RemoveDuplicatesByTag("FloatingTextManager");
        RemoveDuplicatesByTag("DifficultyManager");
        RemoveDuplicatesByName("UIManager");
    }

    void RemoveDuplicatesByTag(string tag)
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag(tag);
        if (objects.Length > 1)
        {
            DeactivateAndDestroyDuplicates(objects);
        }
    }

    void RemoveDuplicatesByName(string name)
    {
        GameObject[] objects = GameObject.FindObjectsOfType<GameObject>();
        int foundCount = 0;
        foreach (GameObject obj in objects)
        {
            if (obj.name == name)
            {
                foundCount++;
                if (foundCount > 1)
                {
                    DeactivateAndDestroyObject(obj);
                }
            }
        }
    }

    void DeactivateAndDestroyDuplicates(GameObject[] objects)
    {
        foreach (GameObject obj in objects) {
            Debug.Log(obj.name);
            obj.SetActive(false); // Деактивируем объект
            obj.SetActive(true); // Деактивируем объект
        }
        for (int i = objects.Length-1; i >= 1; i--)
        {
            DeactivateAndDestroyObject(objects[i]);
        }
    }

    void DeactivateAndDestroyObject(GameObject obj)
    {
        obj.SetActive(false); // Деактивируем объект
        Transform rootTransform = obj.transform.root;
        if (rootTransform != null)
        {
            Destroy(rootTransform.gameObject);
        }
        else
        {
            Destroy(obj);
        }
    }
}
