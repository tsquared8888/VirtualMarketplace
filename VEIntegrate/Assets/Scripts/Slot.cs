using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class Slot : MonoBehaviour, IDropHandler
{
    public int slotNum;
    public Inventory inv;
    private ItemData itemData;
    public GameObject Item;
    public Text displayPrice;
    //private PriceItemData pitemData;
    //public Button button = null;

    private bool selecting;
    private BuyItemsMP purchaseWindow;
    private BuyItemsMP buyItems;
    private bool marketPurchase = false;
    private bool itemBought = false;
	private MarketDB marketDB;
	private PlayerDB playerDB;
    static bool purchaseComplete = false;
    static Transform droppedItem;
    static ItemData droppedData;
    static int savedSlot;
    static Inventory savedInv;
    static Item savedItem;
    

    // Use this for initialization
    void Start()
    {
        purchaseWindow = GameObject.Find("Market Inventory").GetComponent<BuyItemsMP>();
        inv = transform.parent.parent.parent.GetComponent<Inventory>();
        if (inv == null)
        {
            transform.parent.parent.parent.GetComponent<LootInventory>();
        }

        // Button btn = this.GetComponent<Button>();
        if (this.tag.Equals("ShopSlot"))
        {
            //pitemData = this.transform.GetChild(1).GetComponent<PriceItemData>();
            
        }
        purchaseWindow.undisplayPurchaseWindow();
		marketDB = GetComponent<MarketDB>();
		playerDB = GetComponent<PlayerDB>();
    }

    void Update()
    {
        if (gameObject.CompareTag("MarketplaceSlot"))
        {
            //Debug.Log(droppedData == null);
            if(droppedData != null)
            {
                //Debug.Log(droppedData.price);
                
            }
            if (purchaseComplete)
            {
                purchaseComplete = false;
                //if the marketslot is missing an item, we want to destory that slot (unfinished)
                for (int i = 0; i < transform.childCount; i++)
                {
                    if (transform.GetChild(i).CompareTag("item"))
                    {
                        break;
                    }
                    else if (i == transform.childCount - 1)
                    {
                        //Destroy(gameObject);
                    }
                }
            } else
            {
                for (int i = 0; i < transform.childCount; i++)
                {
                    if (transform.GetChild(i).CompareTag("item"))
                    {
                        //displays price of item
                        displayPrice.text = transform.GetChild(i).GetComponent<ItemData>().price.ToString();
                        break;
                    }
                }
            }
        }
    }

    /*void BuyItem()
    {
        //ShopInventory sinv = GameObject.Find("ShopInventory").GetComponent<ShopInventory>();
        Inventory pinv = GameObject.Find("Inventory").GetComponent<Inventory>();
        int empty = -1 ;
        if (pinv.Money >= this.transform.GetChild(1).gameObject.GetComponent<PriceItemData>().item.Value)
        {
            for (int i = 0; i < pinv.numSlots; i++)
            {
                if (pinv.items[i].ID == -1 && empty == -1)
                {
                    empty = i;
                    pinv.items[i].ID = i;
                }

            }
            if (empty != -1)
            {
                GameObject money = GameObject.Find("MoneyAmount").gameObject;
                //money.GetComponent<Text>().text = (pinv.Money - this.transform.GetChild(1).gameObject.GetComponent<PriceItemData>().item.Value).ToString();
                pinv.Money = pinv.Money - this.transform.GetChild(1).gameObject.GetComponent<PriceItemData>().item.Value;
                //pinv.Money = pinv.Money - 10;
                Item itemToAdd = this.transform.GetChild(1).gameObject.GetComponent<PriceItemData>().item;
                GameObject itemObj = Instantiate(Item);
                ItemData newData = itemObj.GetComponent<ItemData>();
                newData.item = itemToAdd;
                newData.slot = empty;
                newData.inv = pinv;
                newData.SetAmount(1);
                itemObj.transform.SetParent(pinv.slots[empty].transform, false);
                itemObj.transform.position = itemObj.transform.parent.position; // Set object to slot position

                //GameObject itemObj = Instantiate(Item);
                //itemObj.transform.SetParent(inv.slots[empty].transform, false);
                //itemObj.transform.position = itemObj.transform.parent.position;
                this.transform.GetChild(1).gameObject.GetComponent<PriceItemData>().tooltip.Deactivate();
                itemObj.GetComponent<Image>().sprite = this.transform.GetChild(1).gameObject.GetComponent<Image>().sprite;

                Destroy(this.transform.GetChild(1).gameObject);

            }
        }
    }*/

    public bool OnPointerEnter()
    {
        return true;
    }

    public ItemData GetItemData()
    {
        if (transform.childCount > 1)
        {
            Transform existingItemObject = transform.GetChild(1);
            ItemData existingData = existingItemObject.GetComponent<ItemData>();
            return existingData;
        }
        return null;
    }

    public void SetItemData(ItemData itemData)
    {
        droppedData = itemData;
    }

    IEnumerator Selecting(Transform droppedItem, ItemData droppedData)
    {
        //Waits for user to click a button before popping up proper windows
        
		int price = droppedData.price;
		int remainingMoney = inv.Money - price;
		if(remainingMoney >= 0){
			selecting = true;
	        if (gameObject){
				purchaseWindow.displayPurchaseWindow();	
			}
	        while (purchaseWindow.selecting)
	        {
	            yield return null;
	        }
	        itemDropController(droppedItem, droppedData);
		}else{
			yield return null;
		}
    }

    public void OnDrop(PointerEventData eventData)
    {
        droppedItem = eventData.pointerDrag.transform;
        droppedData = droppedItem.GetComponent<ItemData>();
        savedSlot = droppedData.slot;
        savedItem = inv.items[slotNum];
        savedInv = droppedData.inv;
        purchaseComplete = false;

        //checks if item is coming from marketplace
        if (droppedData.GetMarketPurchase())
        {
            if (itemBought)
            {
                itemDropController(droppedItem, droppedData);
                itemBought = false;
                purchaseComplete = true;
            }
            else if (!itemBought)
            {
				int price = droppedData.price;
				int remainingMoney = inv.Money - price;
				if(remainingMoney >= 0){
	                itemBought = true;
	                marketPurchase = true;
	                StartCoroutine(Selecting(droppedItem, droppedData));
				}
            }
            
        }
        else
        {
            itemDropController(droppedItem, droppedData);
        }

    }

    //Determines what to do when given an item
    private void itemDropController(Transform droppedItem, ItemData droppedData)
    {
        //Prevents users from dragging items into marketplace slots
        if (!gameObject.CompareTag("MarketplaceSlot"))
        {
            if (!inv.isOutput && droppedData != null)
            {
                if (inv.items[slotNum].ID == -1) // The slot is empty, move the dragged item.
                {
                    if (!droppedData.CheckSplitStack())
                    {
                        droppedData.inv.items[droppedData.slot] = new Item();
                    }
                    inv.items[slotNum] = droppedData.item;
                    droppedData.slot = slotNum;
                    droppedData.inv = inv;
					//Remove money from player's inventory
					int price = droppedData.price;
					inv.Money = inv.Money - price;
					inv.UpdateAmountDisplay();
					droppedData.price = -1;
					//TODO: need to add money to seller's inventory
                }
                else if (droppedData.slot != slotNum || droppedData.inv != inv)
                {   // The slot is occupied, decide what to do with the existing item.
                    Transform existingItem = transform.GetChild(1);
                    itemData = existingItem.GetComponent<ItemData>();
                    bool split = droppedData.CheckSplitStack();
                    if (droppedData.item.Stackable && (droppedData.item.ID == itemData.item.ID || split))
                    {
                        if (droppedData.item.ID == itemData.item.ID)
                        {   // Combine the item stacks.
                            if (!split)
                            {
                                droppedData.inv.items[droppedData.slot] = new Item();
                            }
                            itemData.amount += droppedData.amount;
                            itemData.UpdateAmountDisplay();
                            Destroy(droppedItem.gameObject);
                        }
                        else if (split)
                        {   // Replace the item stack.
                            ItemData residualData = droppedData.GetResidualData();
                            residualData.amount += droppedData.amount;
                            residualData.UpdateAmountDisplay();
                            Destroy(droppedItem.gameObject);
                        }
                    }
                    else if (!droppedData.inv.isOutput)
                    { // Items need to be swapped.
                      // Change swapped item's slot, inv, and position references to the dropped item's, and add it to the dropped item's original inventory.
                        itemData.slot = droppedData.slot;
                        itemData.inv = droppedData.inv;
                        itemData.inv.items[droppedData.slot] = itemData.item;
                        existingItem.transform.SetParent(itemData.inv.slots[droppedData.slot].transform);
                        existingItem.transform.position = itemData.inv.slots[droppedData.slot].transform.position;

                        // Change dropped item's slot, inv, and position references, and add it to this slot's inventory.
                        droppedData.slot = slotNum;
                        droppedData.inv = inv;
                        droppedData.inv.items[droppedData.slot] = droppedData.item;
                        droppedData.transform.SetParent(transform);
                        droppedData.transform.position = transform.position;

                        itemData = droppedData;
                    }

                }
            }
        }
    }
    public void RemoveItem()
    {
        inv.items[slotNum] = new Item();
    }
}
