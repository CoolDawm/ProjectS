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
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (_spellBook.activeSelf)
            {
                _spellBook.SetActive(false);
            }
            else
            {
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
                _character.SetActive(false);
            }
            else
            {
                _character.SetActive(true);
            }
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            if (_inventory.activeSelf)
            {
                _inventory.SetActive(false);
            }
            else
            {
                _inventory.SetActive(true);
            }
        }
    }
}
