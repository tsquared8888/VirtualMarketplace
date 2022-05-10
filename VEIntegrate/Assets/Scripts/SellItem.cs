using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SellItem : MonoBehaviour, IDropHandler
{
    public GameObject confirmWindow;
    public GameObject errorWindow;
    public InputField inputPrice;
	
	private MarketDB marketDB;
	private PlayerDB playerDB;
	private Inventory marketInventory;
    private GameObject item;
    private int price;
    void Start()
    {
        confirmWindow.SetActive(false);
        errorWindow.SetActive(false);
		marketDB = GetComponent<MarketDB>();
		playerDB = GetComponent<PlayerDB>();
		marketInventory = GameObject.Find("Market Inventory").GetComponent<Inventory>();
    }

    //If an item is dropped in sell panel, the confirm window pops up
    public void OnDrop(PointerEventData eventData)
    {
        item = eventData.pointerDrag;
        confirmWindow.SetActive(true);
    }

    //Need this to be public so buttons can access them
    public void Confirm()
    {
        confirmWindow.SetActive(false);
        //checks if input price is a positive integer
        if (int.TryParse(inputPrice.text.ToString(), out price) && price >= 0)
        {
			GameObject gameItem = Instantiate(item);
            ItemData itemData = gameItem.GetComponent<ItemData>();
			int itemid = itemData.item.ID;
			marketDB.addListing(itemid, price, itemData.amount, 1, "Item");  
			marketInventory.addToMarketPlace(itemid, itemData.amount, null, price);
            Destroy(item);
        } else
        {
            errorWindow.SetActive(true);
        }
        
    }

    //Need this to be public so buttons can access them
    public void Cancel()
    {
        confirmWindow.SetActive(false);
    }

    public void Okay()
    {
        errorWindow.SetActive(false);
    }
}
