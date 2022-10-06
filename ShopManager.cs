using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShopManager : MonoBehaviour
{
	public List<GameObject> shelves = new List<GameObject>();
	public float shopMoney = 0f;
	public float tax = 0.035f;
	public float trendCooldown = 10f;
	public string trend = "";
	public TextMeshProUGUI trendText;
	bool onTrendCooldown = false;

    void Update()
    {
        shopMoney = ((int)(shopMoney*100))/100f;
		trendText.SetText("Current Trend: "+trend);
		if(!onTrendCooldown)
		{
			trend = GetRandomInventory();
			onTrendCooldown = true;
			Invoke("OffTrendCooldown", trendCooldown);
		}
    }	
	
	//Returns a string of an item available in the shop
	public string GetRandomInventory()
	{
		int ran = Random.Range(0, shelves.Count);
		int rand = Random.Range(0,1);
		if(rand==0)
		return shelves[ran].GetComponent<ShopShelves>().shelfType;
		return trend;
	}
	
	//Used for getting the location an item's shelves
	public Transform GetShelfLocation(string str)
	{
		foreach(GameObject g in shelves)
		{
			if(str == g.GetComponent<ShopShelves>().shelfType)
				return g.transform;
		}
		print("null");
		return null;
	}
	
	//Gets price of a item
	float GetPrice(string str)
	{
		foreach(GameObject g in shelves)
		{
			if(g.GetComponent<ShopShelves>().shelfType == str)
			{
				return g.GetComponent<ShopShelves>().pricePerItem;
			}
		}
		return 0f;
	}
	
	void OffTrendCooldown()
	{
		onTrendCooldown = false;
	}	
	
	//Used to earn the money after the customer leaves with items
	void OnTriggerEnter2D(Collider2D col)
	{
		if(col.tag=="Customer" && col.gameObject.GetComponent<CustomerManager>().inventory.Count > 0)
		{
			float fl = 0f;
			foreach(string str in col.gameObject.GetComponent<CustomerManager>().inventory)
			{
				fl+=GetPrice(str);
			}
			fl = (fl+(fl*tax));
			shopMoney += fl;
		}
	}
}
