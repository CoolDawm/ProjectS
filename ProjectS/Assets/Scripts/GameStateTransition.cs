using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateTransition : MonoBehaviour
{
   private GameObject[] destroyList;
   private GameObject object1;
   private GameObject object2;

   private void Start()
   {
      destroyList = GameObject.FindGameObjectsWithTag("DontDestroy");
      object1 = GameObject.FindGameObjectWithTag("FloatingTextManager");
      object2 = GameObject.FindGameObjectWithTag("AbilitiesManager");
   }
   //Need to rework that by doing adding gameobjects to this list through player save script
   public void ToCharacterSelector()
   {
      foreach (GameObject obj in destroyList)
      {
         Destroy(obj);
      }
      Destroy(object1);
      Destroy(object2);
      SceneManager.LoadScene("CharacterSelection");
   }
   public void QuitTheGame()
   {
      Application.Quit();
   }
}
