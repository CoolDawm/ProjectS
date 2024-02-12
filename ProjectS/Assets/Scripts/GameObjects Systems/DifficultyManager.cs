using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class DifficultyManager : MonoBehaviour
{
    public float modifier { get; private set; }
    private GameObject[] enList;
    public Action OnDifChange;
    private void Start()
    {
        modifier = 1;
    }

    public void SetModifier(float modifier)
    {
        this.modifier = modifier;
        OnDifChange?.Invoke();
    }

    
}
