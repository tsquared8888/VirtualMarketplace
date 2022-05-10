using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;

public class BuyItemsMP : MonoBehaviour
{
    public GameObject confirmPurchaseWindow;
    public GameObject itemBoughtWindow;
    public bool selecting = false;
    public bool confirmPurchase = false;

    public void displayPurchaseWindow()
    {
        confirmPurchaseWindow.SetActive(true);
        selecting = true;
    }

    public void undisplayPurchaseWindow()
    {
        confirmPurchaseWindow.SetActive(false);
        selecting = false;
    }

    public void Confirm()
    {
        confirmPurchase = true;
        undisplayPurchaseWindow();
        itemBoughtWindow.SetActive(true);
    }

    public void Cancel()
    {
        confirmPurchase = false;
        undisplayPurchaseWindow();
    }

    public void Okay()
    {
        itemBoughtWindow.SetActive(false);
    }
}
