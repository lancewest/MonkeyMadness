using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MultiplayerManager : MonoBehaviour 
{
	public static MultiplayerManager instance;

	public string PlayerName;
	public MPPlayer MyPlayer;

	public bool isPublicServer = true;
	
	public IList<MPPlayer> PlayerList = new List<MPPlayer>();
	public IList<MapSettings> MapList = new List<MapSettings>();
	public IList<GameObject> SpawnPoints = new List<GameObject>();

	public MapSettings CurrentMap = null;

	private int lastPrefix = 0;
	public bool matchLoaded;

	public Transform playerPrefab;

	void Start() 
	{
		instance = this;
		PlayerName = PlayerPrefs.GetString("PlayerName");
		DontDestroyOnLoad(gameObject);
	}

	void Update () 
	{
		instance = this;
	}

	public void StartServer(string ip, int port, bool isPublicS)
	{
		isPublicServer = isPublicS;

		bool useNat = !Network.HavePublicAddress();
		Network.InitializeServer(10,port,useNat);

		if(isPublicServer)
			MasterServer.RegisterHost("MonkeyMadness", "This tutorial is mega blurry", "Trlololol");
	}

	void OnServerInitialized()
	{
		Server_PlayerJoinRequest(PlayerName, Network.player);
	}

	void OnConnectedToServer()
	{
		networkView.RPC ("Server_PlayerJoinRequest", RPCMode.Server, PlayerName, Network.player);
	}

	void OnPlayerConnected(NetworkPlayer newPlayer)
	{
		foreach(MPPlayer existingPlayer in PlayerList)
		{
			networkView.RPC ("Client_AddPlayerToList", newPlayer, existingPlayer.PlayerName, existingPlayer.PlayerNetwork);
		}

		networkView.RPC ("Client_GetMultiplayerMatchSettings", newPlayer, CurrentMap.MapLoadName, "To Be Used Later");
	}

	void OnPlayerDisconnected(NetworkPlayer leavingPlayer)
	{
		string name = "";

		foreach (MPPlayer player in PlayerList) 
		{
			if(player.PlayerNetwork == leavingPlayer)
			{
				PlayerList.Remove(player);
				name = player.PlayerName;
				break;
			}
		}

		networkView.RPC("Client_RemovePlayer", RPCMode.Others, name, leavingPlayer);
	}

	void OnDisconnectedFromServer(NetworkDisconnection info)
	{
		PlayerList.Clear();
	}

	void OnLevelWasLoaded()
	{
		SpawnPoints = GameObject.FindGameObjectsWithTag("spawnpoint");
		matchLoaded = true;
	}




	[RPC]
	void Server_PlayerJoinRequest(string playerName, NetworkPlayer view)
	{
		networkView.RPC("Client_AddPlayerToList", RPCMode.All, playerName, view);
	}

	[RPC]
	void Client_RemovePlayer(string playerName, NetworkPlayer view)
	{
		foreach (MPPlayer player in PlayerList)
			if (player.PlayerName == playerName) 
			{
				PlayerList.Remove (player);
				break;
			}
	}

	[RPC]
	void Client_AddPlayerToList(string playerName, NetworkPlayer view)
	{
		MPPlayer newPlayer = new MPPlayer ();
		newPlayer.PlayerName = playerName;
		newPlayer.PlayerNetwork = view;

		PlayerList.Add(newPlayer);

		if (Network.player == view)
		{
			MyPlayer = newPlayer;
		}
	}

	[RPC]
	void Client_GetMultiplayerMatchSettings(string map, string mode)
	{
		CurrentMap = GetMap(map);
	}

	[RPC]
	void Client_LoadMultiplayerMap(string map, int prefix)
	{
		Network.SetSendingEnabled(0, false);
		Network.isMessageQueueRunning = false;

		Network.SetLevelPrefix(prefix);
		Application.LoadLevel(map);

		Network.isMessageQueueRunning = true;
		Network.SetSendingEnabled(0, true);
	}


	void SpawnPlayer(NetworkPlayer thisPlayer)
	{
		int spawnpointNumber = Random.Range(0, SpawnPoints.Count - 1);
		Network.Instantiate(playerPrefab, SpawnPoints[spawnpointNumber].transform.position, Quaternion.identity, 0);

		GameObject newPlayerPrefab = null;

		foreach( GameObject go in GameObject.FindGameObjectsWithTag("PlayerManager"))
		{
			if(go.GetComponent<PlayerManager>().networkView.isMine)
			{
				newPlayerPrefab = go;
			}
		}


		MyPlayer.playerManager = newPlayerPrefab.GetComponent<PlayerManager>();
		MyPlayer.health = 100;
		MyPlayer.isAlive = true;

		newPlayerPrefab.SendMessage("SetMyPlayer", MyPlayer);


		GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraFollow>().UpdateTarget(newPlayerPrefab.transform.FindChild("Player").transform);
	}




	void OnGUI()
	{
		if(!MyPlayer.isAlive && matchLoaded)
			showSpawnMenu();
	}

	public void showSpawnMenu()
	{
		if(GUI.Button( new Rect(Screen.width/2 - 150, Screen.height/2 - 25, 300, 50), "Respawn"))
		{
			//networkView.RPC("Server_SpawnPlayer", RPCMode.Server, Network.player);
			SpawnPlayer(Network.player);
		}
	}




	public MapSettings GetMap(string name)
	{
		//foreach(MapSettings st in MapList)

		return CurrentMap;
	}

	public NetworkPeerType getPeerTypePlease()
	{
		return Network.peerType;
	}

	public void startGame()
	{
		numberPlayers();
		networkView.RPC("Client_LoadMultiplayerMap", RPCMode.All, CurrentMap.MapLoadName, lastPrefix+1);
		lastPrefix++;
	}

	public void numberPlayers()
	{
		MyPlayer.playerNumber = 1;

		int counter = 2;
		foreach(MPPlayer player in PlayerList)
		{
			player.playerNumber = counter;
			counter++;
		}
	}

}

[System.Serializable]
public class MPPlayer
{
	public string PlayerName = "";
	public int playerNumber;
	public NetworkPlayer PlayerNetwork;
	public PlayerManager playerManager;
	public bool isAlive;
	public int health;
	public int kills;
	public int deaths;
}

[System.Serializable]
public class MapSettings
{
	public string MapName;
	public string MapLoadName;
	public Texture MapLoadTexture;
}