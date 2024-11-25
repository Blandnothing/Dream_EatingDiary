using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class itemOnDrag : MonoBehaviour,IBeginDragHandler,IDragHandler,IEndDragHandler
{

	public Transform originalParent;

	public void OnBeginDrag(PointerEventData eventData)
	{
		originalParent = transform.parent;
		transform.SetParent(transform.parent.parent);
		
		GetComponent<CanvasGroup>().blocksRaycasts = false;
	}
	public void OnDrag(PointerEventData eventData)
	{
		Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		pos.z = 0;
		transform.position = pos;
	}
	public void OnEndDrag(PointerEventData eventData)
	{
		if (eventData.pointerCurrentRaycast.gameObject.name == "Item Image")
		{
			transform.SetParent(eventData.pointerCurrentRaycast.gameObject.transform.parent.parent);
			transform.position = eventData.pointerCurrentRaycast.gameObject.transform.parent.parent.position;
			eventData.pointerCurrentRaycast.gameObject.transform.parent.position = originalParent.position;
			eventData.pointerCurrentRaycast.gameObject.transform.parent.SetParent(originalParent);
			GetComponent<CanvasGroup>().blocksRaycasts = true;
			return;
		}
		transform.SetParent(eventData.pointerCurrentRaycast.gameObject.transform);
		transform.position = eventData.pointerCurrentRaycast.gameObject.transform.position;
		GetComponent<CanvasGroup>().blocksRaycasts = true;
	}
}
