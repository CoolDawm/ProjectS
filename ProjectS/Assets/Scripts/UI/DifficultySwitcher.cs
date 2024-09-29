using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DifficultySwitcher : MonoBehaviour//made for manequins
{
    public GameObject difficultyPanel;
    public List<TMP_InputField> attributeInputs; 

    private void OnTriggerEnter(Collider other)
    {
        ShowDifficultyPanel();
    }

    private void OnTriggerExit(Collider other)
    {
        CloseDifficultyPanel();
    }

    public void ShowDifficultyPanel()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        difficultyPanel.SetActive(true);
    }

    public void CloseDifficultyPanel()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        difficultyPanel.SetActive(false);
    }

    public void ApplyAttributes()
    {
        List<float> attributes = new List<float>();
        GameObject mannequin = GameObject.Find("Mannequin");
        foreach (var input in attributeInputs)
        {
            if (float.TryParse(input.text, out float value))
            {
                attributes.Add(value);
            }
            else
            {
                Debug.LogWarning("Invalid input for attribute: " + input.name);
                return;
            }
        }
        Characteristics characteristics = mannequin.GetComponent<Characteristics>();
        if (characteristics != null)
        {
            characteristics.AddAttributes(attributes);
        }
        difficultyPanel.SetActive(false);
    }
}
