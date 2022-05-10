using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCInteraction : MonoBehaviour
{
	private GameObject triggeringNpc;
	private bool triggering;

	public GameObject inventoryPanel;
	public GameObject npcText;
	public GameObject marketPanel;
	public Image npcTextBckground;
	
	void Start()
	{
		//all panels are disabled
		marketPanel.SetActive(false);
		inventoryPanel.SetActive(false);
		npcTextBckground.enabled = false;
	}
	
	void Update()
	{
		//if we are inside the npc zone...
		if(triggering)
		{
			//enable instructions at top of screen if marketplace isn't open.
			//inventory should be closed if marketplace isn't open
			if(!marketPanel.activeSelf)
			{
				npcText.SetActive(true);
				npcTextBckground.enabled = true;
				inventoryPanel.SetActive(false);
			}
			//marketplace should not open if the inventory is doesn't open
			if(!inventoryPanel.activeSelf){
				marketPanel.SetActive(false);
			}
			//right click to open the marketplace
			if(Input.GetMouseButtonDown(1) ){
				Debug.Log("Player right clicked on NPC");
				npcText.SetActive(false);
				npcTextBckground.enabled = false;
				marketPanel.SetActive(!marketPanel.activeSelf);
				inventoryPanel.SetActive(true);
			}
			//set everything to false if we aren't in range
		}else{
			inventoryPanel.SetActive(false);
			npcText.SetActive(false);
			npcTextBckground.enabled = false;
		}
	}
	
	//check if we are entering the marketplace
	void OnTriggerEnter(Collider other)
	{
		if(other.tag == "NPCMarket")
		{
			triggering = true;
			triggeringNpc = other.gameObject;
		}
	}
	
	//check if we exiting the marketplace
	void OnTriggerExit(Collider other)
	{
		if(other.tag == "NPCMarket")
		{
			triggering = false;
			marketPanel.SetActive(false);
			triggeringNpc = null;
		}
	}
}
