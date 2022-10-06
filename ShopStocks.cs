using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShopStocks : MonoBehaviour
{
	public bool onStockCooldown = false;
	public string shelfType = "";
	public List<string> shelves = new List<string>();
	public float stockingSpeed = 0.5f;
	public PlayerInventory plyr;
	public int maxCapacity = 10;
	public TextMeshProUGUI shelfText;
	
    //Start game with a number of items on stock
    void Start()
    {
        for(int i = 0; i < 20; i++)
		{
			shelves.Add(shelfType);
		}
    }

    void Update()
    {
		//If player is in range, player collects items from stock
        if(!onStockCooldown && plyr != null)
		{
			if(!plyr.isFull() && shelves.Count>0)
			{
				plyr.AddToInventory(shelfType);
				shelves.Remove(shelves[0]);
			}
			onStockCooldown = true;
			Invoke("offStockCooldown",stockingSpeed);
		}
		
		//Shows a visual on what items and how many items are in stock
		string str = "";
		foreach(string s in shelves)
		{
			str+=s+"\n";
		}
		shelfText.SetText(str);
    }
	
	void offStockCooldown()
	{
		onStockCooldown = false;
	}
	
	//Adds player to collect from stock
	void OnTriggerEnter2D(Collider2D col)
	{
		if(col.tag=="Player")
		{
			plyr = col.gameObject.GetComponent<PlayerInventory>();
		}
	}
	
	//Removes player to collect from stock
	void OnTriggerExit2D(Collider2D col)
	{
		if(col.tag=="Player")
		{
			plyr = null;
		}
	}
}
