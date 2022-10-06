using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerInventory : MonoBehaviour
{
	public List<string> inventory = new List<string>();
	public int maxCapacity = 10;
	public TextMeshProUGUI inventoryText;
	
    void Update()
    {
        string str = "";
		foreach(string s in inventory)
		{
			str+=s+"\n";
		}
		inventoryText.SetText(str);
    }
	
	//Add items from stock to inventory
	public void AddToInventory(string str)
	{
		inventory.Add(str);
	}
	
	//If player reached max capacity
	public bool isFull()
	{
		if(inventory.Count>=maxCapacity)
			return true;
		return false;
	}
	
	//Removes item from inventory used to give the illusion of transfering item to the shelves
	public string RemoveFromInventory(string str)
	{
		for(int i = 0; i < inventory.Count; i++)
		{
			if(inventory[i]==str)
			{
				inventory.Remove(inventory[i]);
				return str;
			}
		}
		return "null";
	}
}
