using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopInventory : MonoBehaviour
{

    public List<Item> items = new List<Item>();
    public List<GameObject> slots = new List<GameObject>();
    public List<GameObject> pslots = new List<GameObject>();
    public List<Item> pitems = new List<Item>();
    public List<GameObject> bslots = new List<GameObject>();
    public List<Item> bitems = new List<Item>();


    public GameObject inventorySlot = null;
    public GameObject shopSlot = null;
    public GameObject buySlot = null;
    public GameObject buttonSlot = null;
    public GameObject inventoryItem;
    public GameObject buybutton;
    
    public GameObject shopItem;
    public int numSlots;
    public bool isOutput;
    public GameObject slotPanel;
    public GameObject pricePanel;
    public GameObject buyPanel;
    public int[] usedID;
  
    public GameObject ParentContent = null;
    Inventory playerInventory;
    GameObject inventoryPanel;
    //GameObject slotPanel;
    ItemDB itemDB;
    ItemDB playerItems;

    private void Start()
    {
        itemDB = GameObject.FindGameObjectWithTag("ItemDB").GetComponent<ItemDB>();


        
        //Find the number of slots to create (1 per different item)
        List<Item> CountList = null;
        CountList = itemDB.GetItemList<Item>();
        numSlots = CountList.Count;

        
        playerInventory = GameObject.FindGameObjectWithTag("Inventory").GetComponent<Inventory>();
        if (ParentContent != null)
        {
            RectTransform rt = GameObject.FindGameObjectWithTag("ShopCanv").GetComponent<RectTransform>();
            rt.sizeDelta = new Vector2(rt.sizeDelta.x, numSlots * 40);
        }
        inventoryPanel = transform.GetChild(0).gameObject;


        //Instantiate the item images slots
        for (int i = 0; i < numSlots; i++)
        {
            items.Add(new Item());
            slots.Add(Instantiate(inventorySlot));
            slots[i].GetComponent<Slot>().slotNum = i;
            slots[i].tag = "ShopSlot";
            slots[i].transform.SetParent(slotPanel.transform, false);
        }
        //Instantiate the price slots

        if (pricePanel != null)
        {
            for (int i = 0; i < numSlots; i++)
            {
                pitems.Add(new Item());
                pslots.Add(Instantiate(shopSlot));

                pslots[i].GetComponent<PriceSlot>().slotNum = i;
                pslots[i].transform.SetParent(pricePanel.transform, false);
                
            }
        }
        //Instantiate the button slots

        if (buyPanel != null)
        {
            for (int i = 0; i < numSlots; i++)
            {
                bitems.Add(new Item());
                bslots.Add(Instantiate(buttonSlot));

                bslots[i].GetComponent<BuyButtonSlot>().slotNum = i;
                bslots[i].transform.SetParent(buyPanel.transform, false);


            }
        }
        

        playerItems = GetComponent<ItemDB>();
        //Add Items to buy
        if (playerItems.fileName != "")
        {
            usedID = new int[numSlots];
            List<Item> playerItemList = itemDB.GetItemList<Item>();
            int ind = 0;
            bool taken = false;

            for (int i = 0; i < playerItemList.Count; i++)
            {

                    TryAddItemByID(playerItemList[i].ID, 1, playerItems);
                    usedID[ind] = playerItemList[i].ID;
                    ind++;
                taken = false;
            }
        }



        //Add the price objects for each item
        int[] test = usedID;
        if (playerItems.fileName != "")
        {
            
            List<Item> playerItemList = itemDB.GetItemList<Item>();
            int ind = 0;
           for (int i = 0; i < playerItemList.Count; i++)
            {

                    TryAddPriceByID(playerItemList[i].ID, 1, playerItems);
                    usedID[ind] = playerItemList[i].ID;
                    ind++;
                }
            }
        


        //Add buy buttons
        if (playerItems.fileName != "")
        {

            List<Item> playerItemList = itemDB.GetItemList<Item>();
            int ind = 0;
            for (int i = 0; i < playerItemList.Count; i++)
            {

                    TryAddButtonByID(playerItemList[i].ID, 1, playerItems);
                    usedID[ind] = playerItemList[i].ID;
                    ind++;
                }
                //taken = false;
            //}
            usedID = test;
            for (int i = 0; i < bitems.Count; i++)
            {
                if (bitems[i].ID != -1)
                {
                    bslots[i].transform.GetChild(0).gameObject.GetComponent<BuyButtonS>().slotItem = bitems[i];
                    bslots[i].transform.GetChild(0).gameObject.GetComponent<BuyButtonS>().buttonID = i;
                }
            }
        }

    }

    public bool TryAddButtonByID(int ID, int amount = 1, ItemDB dB = null)
    {
        if (dB == null)
        {
            dB = itemDB;
        }
        Item itemToAdd = dB.GetItem<Item>(ID);
        int idx = GetItemIndexByID(ID);
        bool success = false;

        // Create and add a new Item object
        for (int i = 0; i < bitems.Count && amount > 0; i++)
        {
            if (bitems[i].ID <= -1)
            {
                success = true;
                bitems[i] = itemToAdd;

                GameObject itemObj = Instantiate(buybutton);
                BuyButtonS newData = itemObj.GetComponent<BuyButtonS>();

                newData.buttonID = i;
                newData.items = slots;
                itemObj.transform.SetParent(bslots[i].transform, false);
                itemObj.transform.position = itemObj.transform.parent.position; // Set object to slot position

                if (itemToAdd.Stackable)
                {
                    break;
                }
                amount--;

            }




        }
        return success;
    }

    public bool TryAddPriceByID(int ID, int amount = 1, ItemDB dB = null)
    {
        if (dB == null)
        {
            dB = itemDB;
        }
        Item itemToAdd = dB.GetItem<Item>(ID);
        int idx = GetItemIndexByID(ID);
        bool success = false;

            // Create and add a new Item object
            for (int i = 0; i < pitems.Count && amount > 0; i++)
            {
                if (pitems[i].ID <= -1)
                {
                    success = true;
                    pitems[i] = itemToAdd;
                    GameObject itemObj = Instantiate(shopItem);

                    itemObj.transform.SetParent(pslots[i].transform, false);
                    itemObj.transform.position = itemObj.transform.parent.position; // Set object to slot position
                    itemObj.GetComponent<Text>().text = pitems[i].GetValue().ToString();
                    if (itemToAdd.Stackable)
                    {
                        break;
                    }
                    amount--;

                }




            }
        return success;
    }

    // Add an item to the first available inventory UI slot, or increment the stack.
    public bool TryAddItemByID(int ID, int amount = 1, ItemDB dB = null)
    {
        if (dB == null)
        {
            dB = itemDB;
        }
        Item itemToAdd = dB.GetItem<Item>(ID);
        int idx = GetItemIndexByID(ID);
        bool success = false;

        if (itemToAdd.Stackable && idx >= 0)
        {
            ItemData existData = GetSlotByIndex(idx).GetItemData();
            existData.SetAmount(existData.amount + amount);
            success = true;
        }
        else
        {
            // Create and add a new Item object
            for (int i = 0; i < items.Count && amount > 0; i++)
            {
                if (items[i].ID <= -1)
                {
                    success = true;
                    items[i] = itemToAdd;
                    GameObject itemObj = Instantiate(inventoryItem);
                    PriceItemData newData = itemObj.GetComponent<PriceItemData>();
                    newData.item = itemToAdd;
                    newData.slot = i;
                    newData.inv = this;
                    newData.playerinv = playerInventory;
                    newData.SetAmount(1);
                    itemObj.transform.SetParent(slots[i].transform, false);
                    itemObj.transform.position = itemObj.transform.parent.position; // Set object to slot position
                    if (itemToAdd.GetSprite() == null)
                    {
                        itemObj.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Items/" + itemToAdd.ImgPath);
                    }
                    else
                    {
                        itemObj.GetComponent<Image>().sprite = itemToAdd.GetSprite();
                    }
                    if (itemToAdd.Stackable)
                    {
                        newData.SetAmount(amount);
                        break;
                    }
                    amount--;
                }
            }
        }
        return success;
    }

    public bool TryAddItemAtIndex(int ID, int index, int amount = 1, ItemDB dB = null)
    {
        if (dB == null)
        {
            dB = itemDB;
        }

        bool success = false;

        if (index >= 0 && index < slots.Count && amount > 0)
        {
            Item itemToAdd = dB.GetItem<Item>(ID);
            ItemData existData = GetSlotByIndex(index).GetItemData();
            if (existData)
            {
                if (itemToAdd.Stackable && itemToAdd.ID == existData.item.ID)
                {
                    existData.SetAmount(existData.amount + amount);
                    success = true;
                }
            }
            else
            {
                // Create and add a new Item object
                if (items[index].ID == -1)
                {
                    items[index] = itemToAdd;
                    GameObject itemObj = Instantiate(inventoryItem);
                    PriceItemData newData = itemObj.GetComponent<PriceItemData>();
                    newData.item = itemToAdd;
                    newData.slot = index;
                    newData.inv = this;
                    newData.SetAmount(1);
                    itemObj.transform.SetParent(slots[index].transform, false);
                    itemObj.transform.position = itemObj.transform.parent.position; // Set object to slot position
                    if (itemToAdd.GetSprite() == null)
                    {
                        itemObj.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Items/" + itemToAdd.ImgPath);
                    }
                    else
                    {
                        itemObj.GetComponent<Image>().sprite = itemToAdd.GetSprite();
                    }

                    if (amount == 1 || itemToAdd.Stackable)
                    {
                        newData.SetAmount(amount);
                        success = true;
                    }
                }
            }
        }
        return success;
    }

    public int RemoveItemAtIndex(int index, int amountToRemove = 1)
    {
        Slot slot = GetSlotByIndex(index);
        ItemData itemData = slot.GetItemData();
        int currentAmount = itemData.amount;
        if (amountToRemove >= 0 && amountToRemove < currentAmount)
        {
            itemData.SetAmount(currentAmount - amountToRemove);
            return amountToRemove;
        }
        else if (amountToRemove == currentAmount)
        {

            Destroy(slot.transform.GetChild(1).gameObject);
            slot.RemoveItem();
            return currentAmount;
        }
        else
        {
            return 0;
        }
    }

    public int RemoveItemByID(int ID, int amountToRemove = 1)
    {
        int currentAmount = CountItemByID(ID);
        if (amountToRemove >= 0 && amountToRemove <= currentAmount)
        {
            int index = GetItemIndexByID(ID);
            while (index >= 0 && amountToRemove > 0)
            {
                int amount = Mathf.Min(amountToRemove, GetSlotByIndex(index).GetItemData().GetAmount());
                RemoveItemAtIndex(index, amount);
                amountToRemove -= amount;
                index = GetItemIndexByID(ID);
            }
            return amountToRemove;
        }
        else
        {
            return 0;
        }

    }

    public int CountItemByID(int ID)
    {
        int total = 0;
        for (int i = 0; i < slots.Count; i++)
        {
            PriceItemData itemData = GetPriceSlotByIndex(i).GetItemData();
            if (itemData && itemData.item.ID == ID)
            {
                total += itemData.GetAmount();
            }
        }
        return total;
    }

    public Slot GetSlotByIndex(int index)
    {
        return slots[index].GetComponent<Slot>();

    }

    public PriceSlot GetPriceSlotByIndex(int index)
    {
        return pslots[index].GetComponent<PriceSlot>();

    }

    // Finds the first index of an Item and returns its index if found, or -1 on failure.

    public int GetItemIndexByID(int ID)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].ID == ID)
            {
                return i;
            }
        }
        return -1;
    }
}
