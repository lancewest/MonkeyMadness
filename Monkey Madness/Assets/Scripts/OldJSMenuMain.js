#pragma strict
var ip:String = "127.0.0.1";
var port:int = 25000;
var useNot:boolean;
var isServer:boolean = false;
var act:boolean = false;
var isSearching:boolean = false;
var chats = new Array();
var text : String = "";
var hostData :HostData[];
var hosting : boolean = false;
var loggedIn : boolean = false;
var playerName : String = "";

function Start () 
{

}

@RPC
function AddChat(chat : String)
{
	if(chats.length >= 5)
		chats.RemoveAt(0);
	chats.Add(chat);
}

function Update () 
{

}

function OnGUI()
{
	if(Network.peerType == NetworkPeerType.Disconnected)
	{
		if(!act && !isServer && !isSearching)
		{
			if(GUILayout.Button("Start Server"))
			{
				isServer = true;
			}
			if(GUILayout.Button("Direct Join Server"))
			{
				act = true;
			}
			if(GUILayout.Button("Search Online For Servers"))
			{
				isSearching = true;
			}
		}
		else if(!act && !isSearching)
		{
			GUILayout.Label("Port: " + port);
			port = GUILayout.HorizontalSlider(port, 1000, 50000);
			if(GUILayout.Button("-100"))
			{
				if(port >= 1100) 
					port = port - 100;
			}
			if(GUILayout.Button("+100"))
			{
				if(port <= 49900)
					port = port + 100;
			}
			if(GUILayout.Button("Start Server"))
			{
				act = false;
				isServer = false;
				useNot = !Network.HavePublicAddress();
				Network.InitializeServer(10,port,useNot);
			}
		}
		else if(!isSearching)
		{
			GUILayout.Label("Ip:");
			ip = GUILayout.TextField(ip);
			GUILayout.Label("Port: " + port);
			port = GUILayout.HorizontalSlider(port, 1000, 50000);
			if(GUILayout.Button("-100"))
			{
				if(port >= 1100) 
					port = port - 100;
			}
			if(GUILayout.Button("+100"))
			{
				if(port <= 49900)
					port = port + 100;
			}
			
			if(GUILayout.Button("Start Client"))
			{
			act = false;
			isServer = false;
			Network.Connect(ip, port);
			}
			
		}
		else
		{
			if(GUILayout.Button("Refresh"))
			{
				MasterServer.ClearHostList();
				MasterServer.RequestHostList("MultiplayerTutorial");
			}
				
			hostData = MasterServer.PollHostList();
			
			if(hostData.Length >= 1)
			{
				for(var t : int = 0; t < hostData.Length ; t++)
				{
					GUILayout.BeginHorizontal();
						if(GUILayout.Button("Join"))
						{
							Network.Connect(hostData[t]);
							isSearching = false;
						}
					GUILayout.Label("Players: " + hostData[t].connectedPlayers + "/" + hostData[t].playerLimit + "   Password: "+hostData[t].passwordProtected);
					GUILayout.EndHorizontal();
				}
			}
		}
		
		if(isSearching || act || isServer)
		{
			if(GUILayout.Button("Back"))
			{
				isServer = false;
				act = false;
				isSearching = false;
			}
		}
	}
	else if(Network.peerType == NetworkPeerType.Server)
	{
		if(loggedIn)
			if(!hosting)
			{
				if(GUILayout.Button("Host"))
				{
					MasterServer.RegisterHost("MonkeyMadness", "This tutorial is mega blurry", "Trlololol");
					hosting = true;
				}
			}
		if(GUILayout.Button("Disconnect"))
		{
			Network.Disconnect(500);
			if(hosting)
			{
				MasterServer.UnregisterHost();
				hosting = false;
				chats = new Array();
				loggedIn = false;
			}
			
		}
	}
	else if(Network.peerType == NetworkPeerType.Client)
	{
		if(GUILayout.Button("Disconnect"))
		{
			Network.Disconnect(500);
			chats = new Array();
			loggedIn = false;
		}
	}
	else
	{
		Debug.Log("Connection!!!");
	}
	
	if(Network.peerType == NetworkPeerType.Client || Network.peerType == NetworkPeerType.Server)
	{
		if(loggedIn)
		{
			for(var a : int = 0 ; a < chats.length ; a++)
			{
				GUILayout.BeginHorizontal();
				GUILayout.Label(chats[a].ToString());
				GUILayout.EndHorizontal();
			}
			
			text = GUILayout.TextField(text);
			if(text != "")
			{
				if(GUILayout.Button("Send"))
				{
					networkView.RPC("AddChat", RPCMode.All, playerName + ": " + text);
					text = "";
				}
			}
		}
		else
		{
			GUILayout.BeginHorizontal();
			GUILayout.Label("Enter Player Name:");
			playerName = GUILayout.TextField(playerName);
			GUILayout.EndHorizontal();
			
			if(GUILayout.Button("Log In"))
			{
				networkView.RPC("AddChat", RPCMode.All, playerName + " has joined the game!");
				loggedIn = true;
			}
		}
		
	}
	
	if(Network.peerType == NetworkPeerType.Disconnected && !isSearching && !act && !isServer && !Application.isEditor)
	{
		if(GUILayout.Button("Quit"))
		{
			Application.Quit();
		}
	}
}