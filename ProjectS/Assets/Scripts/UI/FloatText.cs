using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatText : MonoBehaviour
{
    private float randomX;
    private float randomZ;
    private Vector3 offset;
    // Start is called before the first frame update
    void Start()
    {
        randomX = Random.Range(0f, 3f); 
        randomZ = Random.Range(0f, 3f);
        gameObject.transform.rotation = Camera.main.gameObject.transform.rotation;
        Destroy(gameObject, 1f);
        offset = new Vector3(randomX, randomZ, 0);
        transform.localPosition += offset;
    }
}
