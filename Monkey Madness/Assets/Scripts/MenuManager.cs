using UnityEngine;
using System.Collections;

public class MenuManager : MonoBehaviour 
{
	string ip = "127.0.0.1";
	int port = 25000;
	bool isPublic = true;
	string[] chats = new string[5];
	string text = "";
	HostData[] hostData;
	string playerName = "";
	string CurrentMenu;

	public Transform test;

	public MenuManager instance;
	
	void Start () 
	{
		instance = this;
		CurrentMenu = "LogIn";

		playerName = PlayerPrefs.GetString("PlayerName");

		if (playerName == "" || playerName == null)
			playerName = "Bob";
	}


	void OnGUI()
	{
		if (CurrentMenu == "Lobby") 
		{
			showLobby();
		}
		if (CurrentMenu == "LogIn")
			showLogIn();
		if (CurrentMenu == "Main")
			showMain();
		if (CurrentMenu == "ServerBrowser")
			showServerBrowser();
		if (CurrentMenu == "Host")
			showHost();
		if (CurrentMenu == "DirectJoin")
			showDirectJoinn();
	}

	public void showLobby()
	{

		GUILayout.Label("Lobby");

		if(GUILayout.Button("Disconnect"))
		{
			Network.Disconnect(500);

			if(Network.isServer)
				MasterServer.UnregisterHost();

			CurrentMenu = "Main";
		}

		if(Network.isServer)
			if (GUILayout.Button("Start Game")) 
			{
				MultiplayerManager.instance.startGame();
			}

		GUILayout.BeginArea (new Rect (Screen.width - 200, 0, 200, Screen.height));

		GUILayout.Label("Players:");

		if(MultiplayerManager.instance.PlayerList != null && MultiplayerManager.instance.PlayerList.Count > 0)
			foreach (MPPlayer player in MultiplayerManager.instance.PlayerList) 
			{
				GUILayout.Label(player.PlayerName);
			}

		GUILayout.EndArea();
	}
	
	public void showLogIn()
	{
		GUILayout.BeginHorizontal();
		GUILayout.Label("Enter Player Name:");
		playerName = GUILayout.TextField(playerName);
		GUILayout.EndHorizontal();

		if (GUILayout.Button ("Log In")) 
		{
			if(playerName != "")
			{
				MultiplayerManager.instance.PlayerName = playerName;
				PlayerPrefs.SetString("PlayerName", playerName);
				CurrentMenu = "Main";
			}
		}

		if(!Application.isEditor)
			if(GUILayout.Button("Quit"))
				Application.Quit();
	}

	public void showMain()
	{
		if(GUILayout.Button("Host Server"))
			CurrentMenu = "Host";

		if(GUILayout.Button("Browse Servers"))
			CurrentMenu = "ServerBrowser";

		if(GUILayout.Button("Direct Join to Server"))
			CurrentMenu = "DirectJoin";

		if(!Application.isEditor)
			if(GUILayout.Button("Quit"))
				Application.Quit();
	}

	public void showServerBrowser()
	{
		GUILayout.BeginHorizontal();
		if(GUILayout.Button("Refresh"))
		{
			MasterServer.ClearHostList();
			MasterServer.RequestHostList("MonkeyMadness");
		}

		if(GUILayout.Button("Back"))
			CurrentMenu = "Main";

		GUILayout.EndHorizontal();


		hostData = MasterServer.PollHostList();
		
		if(hostData.Length >= 1)
		{
			for(int t = 0; t < hostData.Length ; t++)
			{
				GUILayout.BeginHorizontal();
				if(GUILayout.Button("Join"))
				{
					Network.Connect(hostData[t]);
					CurrentMenu = "Lobby";
				}
				GUILayout.Label("Players: " + hostData[t].connectedPlayers + "/" + hostData[t].playerLimit + "   Password: "+hostData[t].passwordProtected);
				GUILayout.EndHorizontal();
			}
		}
	}

	public void showHost()
	{
		GUILayout.Label("Port: " + port);
		port = (int)(GUILayout.HorizontalSlider(port, 1000, 50000));

		isPublic = GUILayout.Toggle(isPublic, "Public");


		GUILayout.BeginHorizontal();

		if(GUILayout.Button("Host!"))
		{
			MultiplayerManager.instance.StartServer(ip, port, isPublic);
			CurrentMenu = "Lobby";
		}
		
		if(GUILayout.Button("Back"))
			CurrentMenu = "Main";

		GUILayout.EndHorizontal();
	}

	public void showDirectJoinn()
	{
		GUILayout.BeginHorizontal();
		GUILayout.Label("Ip:");
		ip = GUILayout.TextField(ip);
		GUILayout.EndHorizontal();
		
		GUILayout.Label("Port: " + port);
		port = (int)(GUILayout.HorizontalSlider(port, 1000, 50000));
		
		
		GUILayout.BeginHorizontal();
		
		if(GUILayout.Button("Join!"))
		{
			Network.Connect(ip, port);
			CurrentMenu = "Lobby";
		}
		
		if(GUILayout.Button("Back"))
			CurrentMenu = "Main";
		
		GUILayout.EndHorizontal();
	}

	void OnDisconnectedFromServer(NetworkDisconnection info)
	{
		CurrentMenu = "Main";
	}
	
}
