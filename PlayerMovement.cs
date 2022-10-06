using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Pathfinding;

public class PlayerMovement : MonoBehaviour
{
	public float speed = 1f;
	public float maxSpeed = 1f;
	Rigidbody2D rb;

    void Start()
    {
        InvokeRepeating("UpdateScan", 0f, 0.1f);
    }
	
	void UpdateScan()
	{
		AstarPath.active.Scan();
		rb = GetComponent<Rigidbody2D>();
	}

	//Basic player movement using the rigidbody and forces
    void Update()
    {
		if(Input.GetKey(KeyCode.R) || Input.GetKey(KeyCode.Return))
		{
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		}
        if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow) && rb.velocity.x > -maxSpeed)
		{
			rb.AddForce(-transform.right * speed, ForceMode2D.Impulse);
		}
		if(Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow) && rb.velocity.x < maxSpeed)
		{
			rb.AddForce(transform.right * speed, ForceMode2D.Impulse);
		}
		if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow) && rb.velocity.y < maxSpeed)
		{
			rb.AddForce(transform.up * speed, ForceMode2D.Impulse);
		}
		if(Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow) && rb.velocity.y > -maxSpeed)
		{
			rb.AddForce(-transform.up * speed, ForceMode2D.Impulse);
		}
    }
}
