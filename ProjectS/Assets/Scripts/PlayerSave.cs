using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerSave : MonoBehaviour
{
    private GameStateTransition _gameStateTransition;
    void Awake()
    {
        _gameStateTransition = GameObject.FindGameObjectWithTag("GameStateManager").GetComponent<GameStateTransition>();
        _gameStateTransition.destroyList.Add(this.gameObject);
        DontDestroyOnLoad(this.gameObject);
    }
}
