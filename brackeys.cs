#region PowerUp Tutorial
#region PlayerStats
using System;
using System.Collections;
using UnityEngine;

public class PlayerStats: MonoBehaviour
{
	public int health = 50;
	public int gold = 654;
	public int exp = 3632;
	
	public void GoBattle (){
		health -= 1;
		exp += 2;
		gold+=5
	}
}
#endRegion
#region PowerUp
using System;
using System.Collections;
using UnityEngine;

public class PowerUp: MonoBehaviour
{
	public GameObject pickupEffect;
	public float multiplier;
	public float duration;
	
	public void OnTriggerEnter(Collider other){
		if(other.CompareTag("Player"))
		{
			StartCouroutine(PickUp(other, duration));
		}
	}
	public IEnumerator PickUp(Collider player, float powerTimer){
		
		Instantiate(pickupEffect, transform.position, transform.rotation);
		
		//player.transform.localScale *= multiplier;
		PlayerStats stats = player.GetComponent<PlayerStats>();
		stats.health *= multiplier;
		GetComponent<MeshRenderer>().enabled = false;
		GetComponent<Collider>().enabled = false;
		yield return new WaitForSeconds(powerTimer);
		stats.health /= multiplier
		Destroy(gameObject);
	}
}
#endRegion
#endRegion
#region Loading Bar
#region 
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
#region 

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
#endRegion
#region 
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
#endRegion
#region 
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
#endRegion
#region 
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
#endRegion
#region 
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
#endRegion
#region 
using System;
using System.Collections;
using UnityEngine;

public class ...: MonoBehaviour
{
	
}
#endRegion
