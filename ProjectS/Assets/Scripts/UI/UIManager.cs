using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    private bool cursorOn=false;
    void Start()
    {
        if (SceneManager.GetActiveScene().name != "CharacterSelection")
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
       
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (cursorOn)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            cursorOn = !cursorOn;
        }
    }
}
