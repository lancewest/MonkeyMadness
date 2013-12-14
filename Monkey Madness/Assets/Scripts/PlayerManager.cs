using UnityEngine;
using System.Collections;

public class PlayerManager : MonoBehaviour 
{
	public Transform ControllerTransform;
	public PlayerController Controller;
	public MPPlayer MyPlayer;

	public Vector3 CurrentPosition;
	public float localScaleX;
	
	void Start () 
	{
		//ControllerTransform.gameObject.SetActive(false);
		//MyPlayer.playerManager = this;
	}

	void FixedUpdate() 
	{
		if(false)
		if(networkView.isMine)
		{
			CurrentPosition = ControllerTransform.position;
			localScaleX = ControllerTransform.localScale.x;
		}
		else
		{
			ControllerTransform.position = CurrentPosition;
			ControllerTransform.localScale.Set(localScaleX,1,1);
		}
	}

	void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info) 
	{
		if (stream.isWriting) 
		{
			stream.Serialize(ref CurrentPosition);
			stream.Serialize(ref CurrentPosition);
		} 
		else 
		{
			stream.Serialize(ref CurrentPosition);
			stream.Serialize(ref localScaleX);
		}	
	}




	void HandleBulletDamage(int damage, string weapon)
	{
		if(Network.isServer)
			Server_HandleBulletDamage(damage, weapon);
		else
			networkView.RPC("Server_HandleBulletDamage", RPCMode.Server, damage, weapon);
	}



	[RPC]
	public void Server_HandleBulletDamage(int damage, string weapon)
	{
		MyPlayer.health -= damage;
		if(MyPlayer.health < 1)
		{
			MyPlayer.isAlive = false;
			MyPlayer.health = 0;
			networkView.RPC("Client_Die", RPCMode.All);
		}
	}

	[RPC]
	public void Client_Die()
	{
		//OutsideView.SetActive(false);
	}

	[RPC]
	public void Client_ComeAlive()
	{
		//OutsideView.SetActive(true);
	}


	public void SetMyPlayer(MPPlayer myNewPlayer)
	{
		MyPlayer = myNewPlayer;
	}
}
