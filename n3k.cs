#region 
using System;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class Timer: MonoBehaviour
{
	public Text timerText;
	private float startTime;
	
	void Start(){
		startTime = Time.time;
	}
	void Update(){
		float t = Time.time - startTime;
		string mins = ((int) t/60).ToString();
		string secs = (t%60).ToString("f2");
		timerText.text = mins +":"+sec;
	}
	public void EndTimer()
	{
		timerText.color = color.Yellow
	}
}
#endRegion
#region 

using System;
using System.Collections;
using UnityEngine;

public class GreatExpectations : MonoBehaviour
{

	private Vector3 startPos;
	private Transform thisTransform;
	private MeshRenderer mr;
	private void Start() 
	{
		thisTransform = transform;
		startpos = thisTransform.position; 
		mr = thisTransform.GetComponent<MeshRenderer>();
	}		
	private void Update() {
		if(isButton) 
		{
			mr.enabled = Input.GetComponent(buttonName); 
		}
		if(leftJoyStick){
			Vector3 inputDirection = Vector3.zero; 
			inputDirection.x = Input.GetAxis("LeftJoystickHorizontal"); 
			inputDirection.z = Input.GetAxis("LeftJoystickVertical"); 
			thisTransform.position = startpos + inputDirection;
		}
		else
		{
			Vector3 inputDirection = Vector3.zero; 
			inputDirection.x = Input.GetAxis("RightJoystickHorizontal"); 
			thisTransform.position = startpos + inputDirection;
		}	
	}
}
#endRegion
#region 
using System;
using System.Collections;
using UnityEngine.SceneManager;
using UnityEngine;

public class CharacterSelection: MonoBehaviour
{
	private int selection = 0;
	public List<GameObject> models = new List<GameObject>();
	void Start(){
		foreach(GameObject go in models)
		{
			go.SetActive(false);
		}
	models[selection].SetActive(true);
	}
	void Update(){
		if (Input.GetKeyDown(KeyCode.W))
		{
			models[selection].SetActive(false);

			selection++;
			
			if (selection >= models.Count)
				selection = 0;
			models[selection].SetActive(true);
		}
		
		if (Input.GetKeyDown(KeyCode.S))
		{
			models[selection].SetActive(false);
			selection++;
			if (selection < 0)
				selection = models.Count-1;
			models[selection].SetActive(true);
		}
		//model summon on PlayerPrefs
		PlayerPrefs.SetInt("Perferred Model", selection);
		SceneManager.LoadScene("Game");
	}
}
#endRegion
#region 
using System;
using System.Collections;
using UnityEngine;

public class DayNight : MonoBehaviour
{
	private Light sunLight;
	private float transition = 0.0f;
	public float transitionSpeed = 0.0f;
	private bool isSunset = false;
	private bool isSunrise = true;
	
	private void Start()
	{
		sunLight = GetComponent<Light>();
	}
	private void Update()
	{
		transition += (isSunrise)?transitionSpeed * Time.deltaTime : - transitionSpeed * Time.deltaTime;
		if (transition < 0.0f || transition > 1.0f)
		{
			isSunrise=!isSunrise;
		}
		sunLight.intensity = transition;
		sunLight.color = Color.Lerp(Color.blue,Color.white,transition);
	}
}
#endRegion
#region N3K Platformer
#region 
using System;
using System.Collections;
using UnityEngine;

public class PlayerScript: MonoBehaviour
{
	private CharacterController controller;
	private float verticalVelocity;
	private float speed = 5f;
	private float gravity = 1.0f;
	private Vector3 moveVector;
	private float inputDirection;
	private bool secondJumpAvail = false;
	void Start(){
		controller = GetComponent<CharacterController>();
	}
	
	void Update(){
		inputDirection = Input.GetAxis("HorizontalMove") * speed;
		
		
		if(controller.isGrounded)
		{
			verticalVelocity = 0;
			if(Input.GetButtonDown("Jump")){
				verticalVelocity = 10;
				secondJumpAvail = true;
			}
		} 
		else 
		{
			if(Input.GetButtonDown("Jump")){
				if(Input.GetButtonDown("Jump")){
					verticalVelocity = 10;
					secondJumpAvail = false;
					}
			}
			verticalVelocity -= gravity;
		}
		moveVector = new Vector3 (inputDirection, verticalVelocity, 0);
		controller.Move(moveVector * Time.deltaTime);
	}
	private void OnControllerColliderHit(ControllerColliderHit hit)
	{
		if (controller.collisionFlags == CollisionFlag.Sides)
		{
			if(Input.GetButtonDown("Jump"))
			{
				Debug.DrawRay(hit.point, hit.normal, Color.red, 2.0f);
				moveVector = hit.normal * speed;
			}
		}
		//Switch case based on tag
		switch (hit.gameObject.tag)
		{
			case "Coin":
				Destroy(hit.gameObject);
				break;
			case "JumpPad":
				verticalVelocity = JumpForce * 2;
				break;
			case "Teleporter":
				transform.position = hit.transform.GetChild(0).position;
				break;
			default:
				break
		}
	}
}
#endRegion
#region 
using System;
using System.Collections;
using UnityEngine;

public class ...: MonoBehaviour
{
	
}
#endRegion#region 
using System;
using System.Collections;
using UnityEngine;

public class ...: MonoBehaviour
{
	
}
#endRegion
#region 
using System;
using System.Collections;
using UnityEngine;

public class ...: MonoBehaviour
{
	
}
#endRegion#region 
using System;
using System.Collections;
using UnityEngine;

public class ...: MonoBehaviour
{
	
}
#endRegion
#region 
using System;
using System.Collections;
using UnityEngine;

public class ...: MonoBehaviour
{
	
}
#endRegion#region 
using System;
using System.Collections;
using UnityEngine;

public class ...: MonoBehaviour
{
	
}
#endRegion
#endRegion
#region 
using System;
using System.Collections;
using UnityEngine;

public class ...: MonoBehaviour
{
	
}
#endRegion
