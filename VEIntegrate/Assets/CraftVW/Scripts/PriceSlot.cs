using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PriceSlot : MonoBehaviour
{
    public int slotNum;
    public int itemCost = 10;
    public ShopInventory inv;
    private PriceItemData itemData;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public PriceItemData GetItemData()
    {
        if (transform.childCount > 1)
        {
            Transform existingItemObject = transform.GetChild(1);
            PriceItemData existingData = existingItemObject.GetComponent<PriceItemData>();
            return existingData;
        }
        return null;
    }

    public void RemoveItem()
    {
        inv.items[slotNum] = new Item();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
