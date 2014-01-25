using UnityEngine;
using System.Collections;

public class PlayerManager : MonoBehaviour 
{
	public Transform ControllerTransform;
	public PlayerController Controller;
	public MPPlayer MyPlayer;
	public Transform weaponArm;

	public Vector3 CurrentPosition;
	public Quaternion CurrentAimAngle;
	public float localScaleX;
	public int state;
	public float armRotation = 0.0f;
	
	public float smoothTime = 0.1f;
	private float smoothYVelocity = 0.0f;
	private float smoothXVelocity = 0.0f;



	void Start () 
	{
		//ControllerTransform.gameObject.SetActive(false);
		MyPlayer.playerManager = this;

		if(!networkView.isMine)
			changePlayerRenderLayer(MyPlayer.playerNumber);

	}

	void FixedUpdate() 
	{
		if(Network.isServer || Network.isClient)
		{
			if(networkView.isMine)
			{
				CurrentPosition = ControllerTransform.position;
				localScaleX = ControllerTransform.localScale.x;
				state = Controller.state;
				CurrentAimAngle = weaponArm.rotation;
			}
			else
			{

				Controller.state = state;

				if(localScaleX != ControllerTransform.localScale.x)
					Controller.Flip();

				Vector3 newPosition = new Vector3( Mathf.SmoothDamp(ControllerTransform.position.x, CurrentPosition.x, ref smoothXVelocity, smoothTime),
				                                   Mathf.SmoothDamp(ControllerTransform.position.y, CurrentPosition.y, ref smoothYVelocity, smoothTime),
				                                   CurrentPosition.z
				                                  );
				ControllerTransform.position = newPosition;

				weaponArm.rotation = CurrentAimAngle;
			}
		}
	}

	void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info) 
	{
		if (stream.isWriting) 
		{
			stream.Serialize(ref CurrentPosition);
			stream.Serialize(ref CurrentAimAngle);
			stream.Serialize(ref localScaleX);
			stream.Serialize(ref state);
		} 
		else 
		{
			stream.Serialize(ref CurrentPosition);
			stream.Serialize(ref CurrentAimAngle);
			stream.Serialize(ref localScaleX);
			stream.Serialize(ref state);
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

	public void changePlayerRenderLayer(int newLayer)
	{
		foreach(SpriteRenderer renderer in gameObject.GetComponentsInChildren<SpriteRenderer>())
		{
			renderer.sortingLayerName = "Player" + newLayer;
		}
	}
}
