using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ElementsStatusManager : MonoBehaviour
{
    
    [SerializeField]
    private List<Image> _statusIconsList;
    [SerializeField]
    private List<string> _statusNamesList;
    public bool isUnderEffect;
    private void Start()
    {
        Debug.Log(gameObject.name);
        if (_statusIconsList.Count == 0)
        {
            _statusIconsList = new List<Image>();
            Transform playerHud = GameObject.Find("PlayerHUD").transform;
            if (playerHud != null)
            {
                Transform panel = playerHud.Find("MainPanel").gameObject.transform.Find("HudShrotcutsPanel2");
                Debug.Log(panel.gameObject.name);
                GameObject statusPanel = panel.Find("StatusPanel").gameObject;
                int count = statusPanel.transform.childCount;
                for (int i = 0; i < count; i++)
                {
                    _statusIconsList.Add(statusPanel.transform.GetChild(i).GetComponent<Image>());
                }
                foreach (Image image in _statusIconsList)
                {
                    image.gameObject.SetActive(false);
                }
            }
            else
            {
                Debug.LogWarning("PlayerHud не найден!");
            }
        }
    }
    public void SetStatus(string status,Color color)
    {
        Debug.Log(status);

        int index = _statusNamesList.FindIndex(a => a == status);
        Debug.Log(index);
        _statusIconsList[index].gameObject.SetActive(true);
        isUnderEffect = true;
        //_statusText.text += status;
        //_statusText.color = color;
    }
    public void RemoveStatus(string status)
    {
        if (this == null) return;
        int index = _statusNamesList.FindIndex(a => a == status);
        _statusIconsList[index].gameObject.SetActive(false);
        //_statusText.text.Replace(status, string.Empty);
        isUnderEffect = false;

    }
}
