using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class Organ : MonoBehaviour
{

	
	public abstract void OrganEnter();
	public abstract void OrganStay();
	public abstract void OrganExit();

	//触发之后
	public abstract void AfterTrigger();
	
	
	
	public void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			OrganEnter();
		}
	}
	public void OnTriggerStay2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			OrganStay();
		}
	}
	public void OnTriggerExit2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			OrganExit();
			AfterTrigger();
		}
	}

	// Update is called once per frame
    void Update()
    {
        
    }
}
