using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class NPC : MonoBehaviour
{

    public int money = 20;
    public GameObject player;
    bool shopOpen = false;
    bool sellOpen = false;
    public GameObject shopScreen;

    public GameObject sellScreen;
    public GameObject Inventory;
    public Text nearby;

    //Sell Script Vars
    private GameObject item;
    private int price;
    

    // Start is called before the first frame update
    void Start()
    {
    
    }

    // Update is called once per frame
    void Update()
    {

        //If player is close enough and hits e, opens NPC shop
        if (Vector3.Distance(this.transform.position, player.transform.position) < 3)
        {
            nearby.enabled = true;
                //StartCoroutine(ShowMessage("Hit 'E' to Shop", 3));
            
            if (Input.GetKeyDown(KeyCode.E))
            {

                if (!sellOpen)
                {
                    openShop();
                    openSell();
                }
                else if (sellOpen == true)
                {
                    closeSell();
                    closeShop();
                }
                
            } 
            
                
            
        }
        else
        {
            nearby.enabled = false;

        }

    }
    IEnumerator ShowMessage(string message, float delay)
    {
        nearby.enabled = true;
        yield return new WaitForSeconds(delay);
        nearby.enabled = false;
    }

    public void openShop()
    {
        print("Test");
        shopScreen.SetActive(true);
        Inventory.SetActive(true);

        shopOpen = true;
    }

    public void closeShop()
    {
        print("Test");
        shopScreen.SetActive(false);
        Inventory.SetActive(false);
        shopOpen = false;
    }

    public void openSell()
    {
        sellScreen.SetActive(true);
        Inventory.SetActive(true);
        sellOpen = true;
    }

    public void closeSell()
    {
        sellScreen.SetActive(false);
        Inventory.SetActive(false);
        sellOpen = false;
    }
}
