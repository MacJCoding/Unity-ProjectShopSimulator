using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class CustomerManager : MonoBehaviour
{
	ShopManager shopMan;
	public List<string> shoppingList = new List<string>();
	public List<string> inventory = new List<string>();
	bool isDoneShopping = false;
	public float speed = 10f;
	public GameObject emptyPrefab;
	GameObject startingPos;
	bool onItemCooldown = false;
	bool onBreak = false;
	
	bool onTimeCooldown = false;
	public int timeWaited = 0;
	
	public Transform target;
	public float nextWayPointDistance = 3f;
	
	Path path;
	int currentWaypoint = 0;
	bool reachedEndOfPath = false;
	
	Seeker seeker;
	Rigidbody2D rb;
	
    void Start()
    {
		seeker = GetComponent<Seeker>();
		rb = GetComponent<Rigidbody2D>();
		startingPos = Instantiate(emptyPrefab, transform.position, transform.rotation);
        shopMan = GameObject.Find("ShopManager").GetComponent<ShopManager>();
		RefreshNeeds();
		target = shopMan.GetShelfLocation(shoppingList[0]);
		InvokeRepeating("UpdatePath", 0f, 0.5f);
    }
	
	void UpdatePath()
	{
		if(seeker.IsDone())
		{
			seeker.StartPath(rb.position, target.position, OnPathComplete);
		}
	}
	
	void OnPathComplete(Path p)
	{
		if(!p.error)
		{
			path = p;
			currentWaypoint = 0;
		}
	}

    void Update()
    {
		//Change target if customers needs to get items or not 
        if(!isDoneShopping && shoppingList.Count > 0)
		{
			
			target = shopMan.GetShelfLocation(shoppingList[0]);
		} else {
			target = startingPos.transform;
		}
		
		//Wait for next shopping trip
		if(shoppingList.Count ==0 && Vector3.Distance(transform.position,startingPos.transform.position) < 1f && !onBreak)
		{
			float ran = Random.Range(0f, 5f);
			Invoke("offBreak", ran);
			onBreak = true;
			timeWaited = 0;
		}
		
		//Get item from shelves
		if(!onTimeCooldown && shoppingList.Count > 0 && Vector3.Distance(transform.position,shopMan.GetShelfLocation(shoppingList[0]).position) < 2f)
		{
			timeWaited++;
			onTimeCooldown = true;
			Invoke("offTimeCooldown", 1f);
			if(timeWaited > 10)
			{
				shoppingList.Remove(shoppingList[0]);
				timeWaited=0;
			}
		}
	}
	
	//This update method is for the Pathfinding and customer movement
	void FixedUpdate()
	{
		if(path==null)
			return;
		if(currentWaypoint >= path.vectorPath.Count)
		{
			reachedEndOfPath = true;
			return;
		} else {
			reachedEndOfPath = false;
		}
		
		Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
		Vector2 force = direction * speed * Time.deltaTime;
		
		rb.AddForce(force);
		
		float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);
		
		if(distance < nextWayPointDistance)
		{
			currentWaypoint++;
		}
    }
	
	//Give the customers what items they would want to get
	void RefreshNeeds()
	{
		while(inventory.Count>0)
		{
			inventory.Remove(inventory[0]);
		}
		int ran = Random.Range(1,shopMan.shelves.Count+1);
		for(int i = 0; i < ran; i++)
		{
			shoppingList.Add(shopMan.GetRandomInventory());
		}
	}
	
	//Custome takes item from the shelves
	public string TakeItem(string item)
	{
		if(item!=null && shoppingList.Count > 0 && item==shoppingList[0] && !onItemCooldown)
		{
			onItemCooldown = true;
			timeWaited = 0;
			Invoke("offItemCooldown",1f);
			shoppingList.Remove(item);
			inventory.Add(item);
			return item;
		}
		return null;
	}
	
	void offTimeCooldown()
	{
		onTimeCooldown = false;
	}
	
	void offItemCooldown()
	{
		onItemCooldown = false;
	}
	
	void offBreak()
	{
		RefreshNeeds();
		onBreak = false;
	}
}
