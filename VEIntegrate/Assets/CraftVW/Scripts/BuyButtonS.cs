using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class BuyButtonS : MonoBehaviour
{
    public int buttonID;
    public Button button;
    public Item slotItem;
    public GameObject PFItem;
    public List<GameObject> items = new List<GameObject>();
    ItemDB playerItems;

    // Start is called before the first frame update
    void Start()
    {
        //Click listener for the buy button
        button.onClick.AddListener(BuyItem);
        playerItems = GetComponent<ItemDB>();
    }

    // Update is called once per frame
    void Update()
    {

        


        
    }

    public void BuyItem()
    {
        Debug.Log("Test");
        Inventory pinv = GameObject.Find("Inventory").GetComponent<Inventory>();
        ItemDB shopDB = GameObject.Find("Shop").GetComponent<ItemDB>();
        int empty = -1;
        bool added = false;

        //Make sure player has enough money
        if (pinv.Money >= slotItem.Value)
        {
            for (int i = 0; i < pinv.numSlots; i++)
            {
                //If the item can be stacked
                if( slotItem.Stackable == true)
                {
                    if (pinv.items[i].ID == slotItem.ID && added == false)
                    {
                        //Add item to players inventory
                        pinv.TryAddItemByID(slotItem.ID, 1, shopDB);

                        empty = -2;
                        //Update money
                        pinv.Money = pinv.Money - slotItem.Value;
                        pinv.UpdateAmountDisplay();
                        added = true;
                    }
                    //Find next empty slot
                    if(pinv.items[i].ID == -1 && empty == -1)
                    {
                        empty = i;

                    }
                }
                //Finds next empty slot if not stackable
                else if (pinv.items[i].ID == -1 && empty == -1)
                {
                    empty = i;
                }

            }
            //If the item cant be stacked or doesnt exist
            if (empty > -1 && added == false)
            {

                pinv.TryAddItemByID(slotItem.ID, 1, shopDB);
                GameObject money = GameObject.Find("MoneyAmount").gameObject;
                pinv.Money = pinv.Money - slotItem.Value;
                pinv.UpdateAmountDisplay();

            }
        }
    }
}

