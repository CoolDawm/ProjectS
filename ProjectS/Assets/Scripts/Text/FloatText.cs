using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatText : MonoBehaviour
{
    public Vector3 offset = new Vector3(0, 0.5f, 0);
    // Start is called before the first frame update
    void Start()
    {
        gameObject.transform.rotation = Camera.main.gameObject.transform.rotation;
        Destroy(gameObject, 1f);
        transform.localPosition += offset;
    }
}
