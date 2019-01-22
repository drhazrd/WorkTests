using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	public int level = 3;
	public int health = 86;
	
	#region UI Methods
	
	public void ChangeLevel (int amount)
	{
		level += amount;
	}
	
	public void ChangeHealth (int amount)
	{
		health += amount;
	}
	#endregion 
	
	//Can be used in a gamemanager in an empty
	public void SavePlayer(){
		SaveSystem.SavePlayer(this);
	}
	public void LoadPlayer(){
		PlayerData data = SaveSystem.LoadPlayer();
		level = data.level;
		health = dat.health;
		
		Vector3 position;
		position.x = data.position[0];
		position.y = data.position[1];
		position.z = data.position[2];
		transform.position = position;
		
		
		
		
	}

}
