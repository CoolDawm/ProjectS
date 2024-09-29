using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacteristicsIncreaseManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _freePointsMenu;
    [SerializeField]
    private List<Button> _addButtonsList;
    [SerializeField]
    private List<Button> _removeButtonsList;
    [SerializeField]
    private List<string> _charNamesList;
    private Characteristics _characteristics;

    void Start()
    {
        _characteristics = GameObject.FindGameObjectWithTag("Player").GetComponent<Characteristics>();
        for (int i = 0; i < _addButtonsList.Count; i++)
        {
            int index = i;
            _addButtonsList[i].onClick.AddListener(() => TryAddPoint(_charNamesList[index]));
        }
        for (int i = 0; i < _removeButtonsList.Count; i++)
        {
            int index = i;
            _removeButtonsList[i].onClick.AddListener(() => TryRemovePoint(_charNamesList[index]));
        }
    }
    private void OnEnable()
    {
        if (_characteristics == null)
        {
            _characteristics = GameObject.FindGameObjectWithTag("Player").GetComponent<Characteristics>();

        }
        UpdateFreePoints(_characteristics.GetFreePointsAmount());
    }
    public void UpdateFreePoints(int amount)
    {
        _freePointsMenu.text = $"Free Points: {amount}";
    }
    private void TryAddPoint(string charName)
    {
        if (_characteristics.GetFreePointsAmount()>0)
        {
            _characteristics.AddPointToCharacteristic(charName);
            Debug.Log($"Added point to {charName}");
            UpdateFreePoints(_characteristics.GetFreePointsAmount());
        }
        else
        {
            Debug.Log("Insufficient amount of points");
        }
    }
    private void TryRemovePoint(string charName)
    {
        if (_characteristics.charDic[charName] > 1)
        {
            _characteristics.RemovePointFromCharacteristic(charName);
            Debug.Log($"Point removed from {charName}");
            UpdateFreePoints(_characteristics.GetFreePointsAmount());
        }
        else
        {
            Debug.Log("Cannot remove point");
        }
    }
}
