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
    private bool inSpellBook = true;
    public void OnBeginDrag(PointerEventData eventData)
    {
        startPosition = transform.parent;
        parentAfterDrag = transform.parent;
        if (inSpellBook)
        {
            GameObject newObject = Instantiate(gameObject, parentAfterDrag);
            inSpellBook = !inSpellBook;
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
