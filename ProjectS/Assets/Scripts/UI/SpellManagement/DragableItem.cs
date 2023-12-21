using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragableItem : MonoBehaviour,IBeginDragHandler,IDragHandler,IEndDragHandler
{
    [HideInInspector]public Transform parentAfterDrag;
    public Ability ability;
    public Transform startPosition;
    public Image image;
    public bool inSpellBook = true;
    public void OnBeginDrag(PointerEventData eventData)
    {
        startPosition = transform.parent;
        parentAfterDrag = transform.parent;
        if (gameObject.transform.parent.CompareTag("BarSlot"))
        {
            inSpellBook = false;
            Debug.Log("+++++++++++++");
        }
        else if (gameObject.transform.parent.CompareTag("SpellSlot"))
        {
            GameObject newObject = Instantiate(gameObject, startPosition);
            Debug.Log("-------------------------");
        }
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
        Debug.Log("Begin Drag");
        image.raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (parentAfterDrag == startPosition)
        {
            Destroy(gameObject);
            return;
        }
        transform.SetParent(parentAfterDrag);
        Debug.Log("End Drag");
        image.raycastTarget = true;
    }
}
