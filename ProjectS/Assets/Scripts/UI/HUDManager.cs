using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDManager : MonoBehaviour
{
    private GameObject _playerHUD;
    private GameObject _spellBook;
    private GameObject _map;
    private GameObject _character;
    private GameObject _inventory;
    void Start()
    {
        _playerHUD = GameObject.FindGameObjectWithTag("PlayerHUD");
        _spellBook = transform.Find("SpellBook").gameObject;
        _map = transform.Find("Map").gameObject;
        _character = transform.Find("Character").gameObject;
        _inventory = transform.Find("Inventory").gameObject;
        gameObject.SetActive(GameObject.FindWithTag("Player"));
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (_spellBook.activeSelf)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                _spellBook.SetActive(false);
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                _spellBook.SetActive(true);
            }
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            if (_map.activeSelf)
            { 
                _map.SetActive(false);
            }
            else
            {
                _map.SetActive(true);
            }
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (_character.activeSelf)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                _character.SetActive(false);
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                _character.SetActive(true);
            }
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            if (_inventory.activeSelf)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                _inventory.SetActive(false);
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                _inventory.SetActive(true);
            }
        }
    }
}
