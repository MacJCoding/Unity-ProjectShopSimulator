using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShopShelves : MonoBehaviour
{
	public bool onStockCooldown = false;
	public string shelfType = "";
	public float pricePerItem = 0.55f;
	public List<string> shelves = new List<string>();
	public float stockingSpeed = 0.5f;
	public PlayerInventory plyr;
	public List<CustomerManager> customers = new List<CustomerManager>();
	public int maxCapacity = 10;
	public TextMeshProUGUI shelfText;
	
    void Update()
    {
		//If player is in range, player adds items to shelf
        if(!onStockCooldown && plyr != null && shelves.Count < maxCapacity)
		{
			string addition = plyr.RemoveFromInventory(shelfType);
			if(addition!="null")
			{
				shelves.Add(addition);
			}
			onStockCooldown = true;
			Invoke("offStockCooldown",stockingSpeed);
		}
		
		//Shows a visual on what items and how many items are in the shelves
		string str = "";
		foreach(string s in shelves)
		{
			str+=s+"\n";
		}
		shelfText.SetText(str);
		
		//If customer is in range, customer collects items from shelf
		if(customers.Count > 0)
		{
			string s = "";
			if(shelves.Count > 0){
				s= customers[0].TakeItem(shelfType);
			} else {
				s=customers[0].TakeItem(null);
			}
			if(s != null)
			{
				shelves.Remove(s);
			}
		}
    }
	
	void offStockCooldown()
	{
		onStockCooldown = false;
	}
	
	//Holds player or customers in the area of stocking/buying to a list
	void OnTriggerEnter2D(Collider2D col)
	{
		if(col.tag=="Player")
		{
			plyr = col.gameObject.GetComponent<PlayerInventory>();
		}
		if(col.tag=="Customer")
		{
			customers.Add(col.gameObject.GetComponent<CustomerManager>());
		}
	}
	
	//Removes player or customers in the area of stocking/buying from the list
	void OnTriggerExit2D(Collider2D col)
	{
		if(col.tag=="Player")
		{
			plyr = null;
		}
		if(col.tag=="Customer")
		{
			customers.Remove(col.gameObject.GetComponent<CustomerManager>());
		}
	}
}
