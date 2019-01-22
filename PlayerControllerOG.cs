using System;
using System.Collections;
using UnityEngine;

public class PlayerControllerOG{
	
	public float moveSpeed;
	public RigidBody theRB;
	public float jumpForce;
	
	void Start(){
		theRB = GetComponent<RigidBody>();
	}
	
	void Update(){
		theRB.velocity = new Vector3 (Input.GetAxis("Horizontal") * movespeed, theRB.velocity.y, Input.GetAxis("Vertical")* moveSpeed);
		
		if(Input.GetButtonDown("Jump"))
		{
			theRB.velocity = new Vector3(theRB.velocity.x, jumpForce, theRB.velocity.z)
		}
	}
	
}
