using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class UIController : MonoBehaviour
{
    public GameObject shopUI = null;
    public GameObject marketPanel = null;
    public GameObject confirmPurchaseWindow;
    public GameObject confirmSellWindow;
    public GameObject inputErrorWindow;
    public GameObject itemBoughtWindow;
    public GameObject reticle;
    public GameObject craftingUI;
    public GameObject lootUI;
    public GameObject inventory;
    public GameObject tooltip;
    public GameObject FPSController;
    public Transform MainCamera;
    public GameObject SellTab = null;

    Transform CameraParent;

    private void Start()
    {
        lootUI.SetActive(false);
        craftingUI.SetActive(false);
        inventory.SetActive(false);
        if (shopUI != null)
        {
            shopUI.SetActive(false);
        }
		if(marketPanel != null){
			itemBoughtWindow.SetActive(false);
	        inputErrorWindow.SetActive(false);
	        confirmPurchaseWindow.SetActive(false);
	        confirmSellWindow.SetActive(false);
	        marketPanel.SetActive(false);
			Debug.Log("confirm purchase windows " + confirmPurchaseWindow.activeSelf + " item bought " + itemBoughtWindow.activeSelf);
		}
        
        

        tooltip = GameObject.FindGameObjectWithTag("TooltipController").transform.GetChild(0).gameObject;
        MainCamera = GameObject.FindGameObjectWithTag("MainCamera").transform;
        CameraParent = MainCamera.parent;
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (shopUI.activeInHierarchy)
            {
                shopUI.SetActive(!shopUI.activeInHierarchy);
                SellTab.SetActive(!SellTab.activeInHierarchy);
                if (inventory.activeInHierarchy)
                {
                    inventory.SetActive(false);

                }
            }
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            craftingUI.SetActive(!craftingUI.activeInHierarchy);
            inventory.SetActive(craftingUI.activeInHierarchy);
        }
        else if (Input.GetKeyDown(KeyCode.I))
        {
            if (shopUI != null)
            {
                if (shopUI.activeInHierarchy)
                {
                    shopUI.SetActive(!shopUI.activeInHierarchy);

                    inventory.SetActive(!inventory.activeInHierarchy);
                }
            }
            if (!craftingUI.activeInHierarchy)
            {
                inventory.SetActive(!inventory.activeInHierarchy);
            }
        }
        else if (Input.GetKeyDown(KeyCode.P))
        {
            if (shopUI != null)
            {
                shopUI.SetActive(!shopUI.activeInHierarchy);
                inventory.SetActive(shopUI.activeInHierarchy);
            }
        }

        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            inventory.SetActive(false);
            craftingUI.SetActive(false);
            //shopUI.SetActive(false);
            if (lootUI.activeInHierarchy)
            {
                var lootScript = lootUI.GetComponent<LootInventory>();
                foreach (Transform child in lootScript.getLootSlotPanel().transform)
                {
                    foreach (Transform slot in child.transform)
                    {
                        if (slot.tag == "item")
                        {
                            GameObject.Destroy(slot.gameObject);
                        }
                    }
                }
                for (int i = 0; i < lootScript.numSlots; i++)
                {
                    lootScript.items[i] = new Item();
                    lootScript.slots[i].GetComponent<Slot>().slotNum = i;
                }
                lootUI.SetActive(false);
            }
        }

        if (lootUI.activeInHierarchy)
        {
            inventory.SetActive(true);
        }

        Cursor.visible = inventory.activeInHierarchy;

        if (Cursor.visible)
        {
            Cursor.lockState = CursorLockMode.None;
            if (FPSController)
            {
                MainCamera.SetParent(CameraParent.parent);
                FPSController.SetActive(false);
            }
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            tooltip.SetActive(false);
            if (FPSController)
            {
                FPSController.SetActive(true);
                MainCamera.SetParent(CameraParent);
            }
        }
    }
}
