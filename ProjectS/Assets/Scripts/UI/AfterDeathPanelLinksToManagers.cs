using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfterDeathPanelLinksToManagers : MonoBehaviour
{
    private GameStateTransition _gameStateTransition;
    // Start is called before the first frame update
    void Start()
    {
        _gameStateTransition = GameObject.FindGameObjectWithTag("GameStateManager").GetComponent<GameStateTransition>();
    }

    public void ToCharacterSelector()
    {
        _gameStateTransition.ToCharacterSelector();
    }
    public void QuitTheGame()
    {
        _gameStateTransition.QuitTheGame();
    }
}
