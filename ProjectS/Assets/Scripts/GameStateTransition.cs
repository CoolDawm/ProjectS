using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateTransition : MonoBehaviour
{
   public List<GameObject> destroyList = new List<GameObject>();
   public void ToCharacterSelector()
   {
      foreach (GameObject obj in destroyList)
      {
         Debug.Log(obj.name);
         Destroy(obj);
      }
      SceneManager.LoadScene("CharacterSelection");
   }
   public void QuitTheGame()
   {
      Application.Quit();
   }
}
