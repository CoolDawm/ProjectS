using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterPanel : MonoBehaviour
{
    [SerializeField] private GameObject _charPanel;
    [SerializeField] private GameObject _talentsPanel;
    [SerializeField] private GameObject _charIncreasePanel;
    public void ToCharacteristics()
    {
        if (!_charPanel.activeSelf)
        {
            _charPanel.SetActive(true);
            _talentsPanel.SetActive(false);
            _charIncreasePanel.SetActive(false);
        }
    }
    public void ToTalents()
    {
        if (!_talentsPanel.activeSelf)
        {
            _charPanel.SetActive(false);
            _talentsPanel.SetActive(true);
            _charIncreasePanel.SetActive(false);
        }
    }
    public void ToCharIncreasePanel()
    {
        if (!_charIncreasePanel.activeSelf)
        {
            _charPanel.SetActive(false);
            _talentsPanel.SetActive(false);
            _charIncreasePanel.SetActive(true);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
