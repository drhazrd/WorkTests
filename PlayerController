using System;
using System.Collections;
using UnityEngine;

public class PlayerController{
	
	public float moveSpeed;
	public float jumpForce;
	private Vector3 moveDirection;
	public CharacterController controller;
	public float gravityScale;
	
	void Start(){
		controller = GetComponent<CharacterController>();
	}
	
	void Update(){
		moveDirection = new Vector3 (Input.GetAxis("Horizontal") * movespeed, 0f, Input.GetAxis("Vertical")* moveSpeed);
		
		if(Input.GetButtonDown("Jump"))
		{
			moveDirection.y = jumpForce;
		}
		moveDirection = moveDirection + (Physics.gravity.y * Time.deltaTime * gravityScale);
		// Can remove Time.deltaTime, due to it being an extra delay
		controller.Move(moveDirection * Time.deltaTime);
	}
	
}
