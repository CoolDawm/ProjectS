using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLevelSystem : MonoBehaviour
{
    private float _neededExpForNextLevel;
    private float _currentExp=0;
    private int _currentLevel=1;
    private Characteristics _characteristics;
    public Action<float, float> onExpChange;
    // Start is called before the first frame update
    void Start()
    {
        //_neededExp should be pulled from save file
        _characteristics = GetComponent<Characteristics>();
        _neededExpForNextLevel = _currentLevel * 50;

        onExpChange?.Invoke(_neededExpForNextLevel, _currentExp);

    }

    public void AddExp(float amount)
    {
        Debug.Log($"+{amount} XP") ;
        _currentExp += amount;
        CheckForLevelUp();
    }
    private void CheckForLevelUp()
    {
        if (_currentExp == _neededExpForNextLevel)
        {
            SetNewLevel();
            _currentExp = 0;
        }else if (_currentExp > _neededExpForNextLevel)
        {
            SetNewLevel();
            _currentExp-= _neededExpForNextLevel;
        }
        onExpChange?.Invoke(_neededExpForNextLevel,_currentExp);
    }
    private void SetNewLevel()
    {
        Debug.Log("New Level");
        _currentLevel++;
        _neededExpForNextLevel = _currentLevel * 50;
        _characteristics.AddFreePoints(_currentLevel);
    }
}
