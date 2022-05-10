using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class VendorSell : MonoBehaviour, IDropHandler
{
    public GameObject confirmWindow;
    public GameObject errorWindow;
    public InputField inputPrice;
    public GameObject npc;
    public GameObject inventory;
    private GameObject item;
    private int price;
    void Start()
    {
        confirmWindow.SetActive(false);
        errorWindow.SetActive(false);
    }

    //If an item is dropped in sell panel, the confirm window pops up
    public void OnDrop(PointerEventData eventData)
    {
        item = eventData.pointerDrag;
        Confirm();
        //confirmWindow.SetActive(true);
    }

    //Need this to be public so buttons can access them
    public void Confirm()
    {
        //this.transform.parent.parent.gameObject.SetActive(false);
        //confirmWindow.SetActive(false);
        //checks if input price is a positive integer
        //if (int.TryParse(inputPrice.text.ToString(), out price) && price >= 0)
        //{
            price = item.GetComponent<ItemData>().item.GetValue() * item.GetComponent<ItemData>().GetAmount();
            Debug.Log("The price of this item is: " + price);
        //add to marketplace database
        inventory.GetComponent<Inventory>().items[item.GetComponent<ItemData>().slot] = new Item();
            Destroy(item);

            //update the player's money
            //inventory.sell(price);
            inventory.GetComponent<Inventory>().VendorSell(price);
            //npc.GetComponent<NPC>().closeSell();
        //} else
        //{
         //   errorWindow.SetActive(true);
        //}
        
    }

    //Need this to be public so buttons can access them
    public void Cancel()
    {
        confirmWindow.SetActive(false);
        //this.transform.parent.parent.gameObject.SetActive(false);
    }

    public void Okay()
    {
        errorWindow.SetActive(false);
        //this.transform.parent.parent.gameObject.SetActive(true);
    }
}
