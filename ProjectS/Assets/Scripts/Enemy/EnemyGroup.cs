using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGroup : MonoBehaviour
{
    public List<GameObject> enemGroup = new List<GameObject>();
    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < enemGroup.Count; i++)
        {
            if (enemGroup[i] == null)
            {
                Debug.Log("Remove");
                enemGroup.Remove(enemGroup[i]);
            }
            else
            {
                bool _agro = enemGroup[i].GetComponent<EnemyBehaviour>().isAggro;
                if (_agro)
                {
                    for (int j = 0; j < enemGroup.Count; j++)
                    {
                        if (enemGroup[j] == null)
                        {
                            Debug.Log("Remove");
                            enemGroup.Remove(enemGroup[j]);
                        }
                        else
                        {
                            enemGroup[j].GetComponent<EnemyBehaviour>().isAggro=true;
                        }
                    }
                }
            }
        }
        /*foreach (GameObject enem in enemGroup)
        {
            if (enem == null)
            {
                Debug.Log("Remove");
                enemGroup.Remove(enem);
            }
            else
            {
                bool _agro = enem.GetComponent<EnemyBehaviour>().isAggro;
                if (_agro)
                {
                    foreach (GameObject en in enemGroup)
                    {
                        if (en == null)
                        {
                            Debug.Log("Remove");
                            enemGroup.Remove(enem);
                        }
                        else
                        {
                            en.GetComponent<EnemyBehaviour>().isAggro=true;
                        }
                    }
                }
            }
        }*/
    }
}
