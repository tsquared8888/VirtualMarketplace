
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemData : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler {

    public Item item;       // The Item object from the database
    public int amount;      // The amount of the item in the stack
    public int slot;        // The slot index within the inventory that
    public Inventory inv;
    public Tooltip tooltip;
    public int price;

    private GameObject residual = null;
    private ItemData residualData = null;
    private bool splitStack = false;
    private CanvasGroup canvasGroup;
    private bool marketPurchase = false;

    void Start()
    {
        // Inventory inv is set when item is added to an inventory or swapped.
        tooltip = GameObject.FindGameObjectWithTag("TooltipController").transform.GetComponent<Tooltip>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    /*void Update()
    {
        if (transform.parent.CompareTag("MarketSlot"))
        {

        }
    }*/

    public void OnBeginDrag(PointerEventData eventData)
    {
        marketPurchase = false;
        if (item != null)
        {
            if(transform.parent.tag == "MarketplaceSlot")
            {
                marketPurchase = true;
            }
            canvasGroup.blocksRaycasts = false;
            if (Input.GetMouseButton(0))
            {
                transform.position = eventData.position;
                transform.SetParent(transform.root); // Ensures object is rendered on top of other panels
            }
            else if (Input.GetMouseButton(1))
            {
                if (item.Stackable && amount > 1)
                {
                    residual = Instantiate(gameObject, transform.position, transform.rotation, transform.parent);
                    residualData = residual.GetComponent<ItemData>();

                    int residualAmount = Mathf.RoundToInt(Mathf.Floor(amount / 2));
                    residualData.amount = residualAmount;
                    amount -= residualAmount;

                    residualData.UpdateAmountDisplay();
                    UpdateAmountDisplay();

                    splitStack = true;
                }
                transform.position = eventData.position;
                transform.SetParent(transform.root); // Ensures object is rendered on top of other panels
            }
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (item != null)
        {
            transform.position = eventData.position; // - offset;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (CheckSplitStack())
        { // Replace the item stack.
            ItemData residualData = GetResidualData();
            residualData.amount += amount;
            residualData.UpdateAmountDisplay();
            Destroy(gameObject);
        }

        transform.SetParent(inv.slots[slot].transform);
        transform.position = inv.slots[slot].transform.position;
        canvasGroup.blocksRaycasts = true;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        tooltip.Activate(item);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tooltip.Deactivate();
    }

    public bool GetMarketPurchase()
    {
        return marketPurchase;
    }

    public int GetAmount()
    {
        return amount;
    }
    public void SetAmount(int newAmount)
    {
        amount = newAmount;
        UpdateAmountDisplay();
    }

    public void UpdateAmountDisplay()
    {
        string display;
        if (amount == 1)
        {
            display = "";
        }
        else
        {
            display = amount.ToString();
        }
        transform.GetChild(0).GetComponent<Text>().text = display;
    }

    public GameObject GetResidual()
    {
        return residual;
    }

    public ItemData GetResidualData()
    {
        return residualData;
    }

    public bool CheckSplitStack()
    {
        if (splitStack)
        {
            splitStack = false;
            return true;
        }
        return false;
    }
}
