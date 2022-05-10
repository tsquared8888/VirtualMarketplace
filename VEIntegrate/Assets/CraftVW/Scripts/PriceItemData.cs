
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PriceItemData : MonoBehaviour, /*IBeginDragHandler, IDragHandler, IEndDragHandler,*/ IPointerEnterHandler, IPointerExitHandler {

    public Item item;       // The Item object from the database
    public int amount;      // The amount of the item in the stack
    public int slot;        // The slot index within the inventory that
    public ShopInventory inv;
    public Inventory playerinv;
    public Tooltip tooltip;

    private GameObject residual = null;
    private PriceItemData residualData = null;
    private bool splitStack = false;

    void Start()
    {
        // Inventory inv is set when item is added to an inventory or swapped.
        tooltip = GameObject.FindGameObjectWithTag("TooltipController").transform.GetComponent<Tooltip>();
       
    }

   

    //public void OnBeginDrag(PointerEventData eventData)
    //{
    //    if (item != null)
    //    {
    //        if (Input.GetMouseButton(0))
    //        {
    //            transform.position = eventData.position;
    //            transform.SetParent(transform.root); // Ensures object is rendered on top of other panels
    //            GetComponent<CanvasGroup>().blocksRaycasts = false;
    //        }
    //        else if (Input.GetMouseButton(1))
    //        {
    //            if (item.Stackable && amount > 1)
    //            {
    //                residual = Instantiate(gameObject, transform.position, transform.rotation, transform.parent);
    //                residualData = residual.GetComponent<PriceItemData>();

    //                int residualAmount = Mathf.RoundToInt(Mathf.Floor(amount / 2));
    //                residualData.amount = residualAmount;
    //                amount -= residualAmount;

    //                residualData.UpdateAmountDisplay();
    //                UpdateAmountDisplay();

    //                splitStack = true;
    //            }
    //            transform.position = eventData.position;
    //            transform.SetParent(transform.root); // Ensures object is rendered on top of other panels
    //            GetComponent<CanvasGroup>().blocksRaycasts = false;
    //        }
    //    }
    //}

    //public void OnDrag(PointerEventData eventData)
    //{
    //    if (item != null)
    //    {
    //        transform.position = eventData.position; // - offset;
    //    }
    //}

    //public void OnEndDrag(PointerEventData eventData)
    //{
    //    if (CheckSplitStack())
    //    { // Replace the item stack.
    //        PriceItemData residualData = GetResidualData();
    //        residualData.amount += amount;
    //        residualData.UpdateAmountDisplay();
    //        Destroy(gameObject);
    //    }

    //    transform.SetParent(inv.slots[slot].transform);
    //    transform.position = inv.slots[slot].transform.position;
    //    GetComponent<CanvasGroup>().blocksRaycasts = true;
    //}

    public void OnPointerEnter(PointerEventData eventData)
    {
        tooltip.Activate(item);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tooltip.Deactivate();
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

    public PriceItemData GetResidualData()
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
