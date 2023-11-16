using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


    public class PlayerSelect : MonoBehaviour
    {
        private GameObject[] _characters;
        private int _index;

        private void OnEnable()
        {
            _index = PlayerPrefs.GetInt("CharacterSelected");
            _characters = new GameObject[transform.childCount];
            for (int i = 0; i < transform.childCount; i++)
            {
                _characters[i] = transform.GetChild(i).gameObject;
            }
            foreach (GameObject go in _characters)
            {
                go.SetActive(false);
            }

            if (_index < 0 || _index >= _characters.Length) 
            {
                _index = 0;
            }
            _characters[_index].SetActive(true);
        }

        public void SelectLeft()
        {
            _characters[_index].SetActive(false);
            _index--;
            if (_index < 0)
            {
                _index = _characters.Length - 1;
            }
            _characters[_index].SetActive(true);
        }

        public void SelectRight()
        {
            _characters[_index].SetActive(false);
            _index++;
            if (_index >= _characters.Length)
            {
                _index = 0;
            }
            _characters[_index].SetActive(true);
        }

        public void StartScene()
        {
            PlayerPrefs.SetInt("CharacterSelected", _index);
            SceneManager.LoadScene("Level01");
        }
    }

