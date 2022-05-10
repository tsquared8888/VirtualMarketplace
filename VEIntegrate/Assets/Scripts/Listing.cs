using System.Collections.Generic;

[System.Serializable]
public class Listing {
    public int listingID;
    public int itemID;
	public string category;

    public Listing(int listingID, int itemID, string category)
    {
        this.listingID = listingID;
        this.itemID = itemID;
        this.category = category;
		
    }

}
