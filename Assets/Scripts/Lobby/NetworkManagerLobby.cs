using Mirror;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Runtime.InteropServices;

public class NetworkManagerLobby : NetworkManager
{
    [SerializeField] private int minPlayers = 1;

    //mirror your scene
    [Scene] [SerializeField] private string menuScene = string.Empty;

    [Header("Room")]
    [SerializeField] private NetworkRoomPlayerLobby roomPlayerPrefab = null;

    [Header("Game")]
    //game 
    [SerializeField] private NetworkGamePlayerLobby gamePlayerPrefab = null;
    [SerializeField] private GameObject playerSpawnSystem = null;
    [SerializeField] private GameObject roundSystem = null;

    // public NetworkManagerLobby()
    // {
    //     this.menuScene = string.Empty;
    // }

    //listen to the menu UI, add client func.
    public static event Action OnClientConnected;
    public static event Action OnClientDisconnected;
    public static event Action<NetworkConnection> OnServerReadied;
    public static event Action OnServerStopped;
    public List<NetworkRoomPlayerLobby> RoomPlayers { get; } = new List<NetworkRoomPlayerLobby>();
    public List<NetworkGamePlayerLobby> GamePlayers { get; } = new List<NetworkGamePlayerLobby>();
    // public List<NetworkConnection> PlayerConnections{get; } = new List<NetworkConnection>();

    //load all prefabs    
    public override void OnStartServer() => spawnPrefabs = Resources.LoadAll<GameObject>("SpawnablePrefabs").ToList();

    public override void OnStartClient()
    {
        var spawnablePrefabs = Resources.LoadAll<GameObject>("SpawnablePrefabs");

        foreach (var prefab in spawnablePrefabs)
        {
            ClientScene.RegisterPrefab(prefab);
        }
        
        ClientScene.RegisterPrefab(playerSpawnSystem);
        ClientScene.RegisterPrefab(roundSystem);

        // Debug.Log("register spwan and round!!!");
    }

    public void test(string line){
        Debug.Log("the same lobbymanager"+line);
        Debug.Log("count: "+ RoomPlayers.Count);
    }

    public override void OnClientConnect(NetworkConnection conn)
    {
        base.OnClientConnect(conn);

        OnClientConnected?.Invoke();
    }

    public override void OnClientDisconnect(NetworkConnection conn)
    {
        base.OnClientDisconnect(conn);

        OnClientDisconnected?.Invoke();
    }

    public override void OnServerConnect(NetworkConnection conn)
    {
        // Debug.Log("one client connected to the Server! "+numPlayers);
        if (numPlayers >= maxConnections)
        {
            conn.Disconnect();
            return;
        }

        if (SceneManager.GetActiveScene().path != menuScene)
        {
            conn.Disconnect();
            return;
        }
        // PlayerConnections.Add(conn);
    }

    // called when the client connect.
    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        if (SceneManager.GetActiveScene().path == menuScene)
        {
            bool isLeader = RoomPlayers.Count == 0;

            NetworkRoomPlayerLobby roomPlayerInstance = Instantiate(roomPlayerPrefab);

            roomPlayerInstance.IsLeader = isLeader;
            GCHandle hander = GCHandle.Alloc(RoomPlayers);
            var pin = GCHandle.ToIntPtr(hander);
            //dedicated server mode
            // RoomPlayers.Add(roomPlayerInstance);

            // Debug.Log(conn+" add player "+roomPlayerInstance.IsLeader+" "+RoomPlayers.Count+" "+"0x" + pin+" room.count:"+roomPlayerInstance.Room.RoomPlayers.Count);

            //tie the connection and gameObject
            NetworkServer.AddPlayerForConnection(conn, roomPlayerInstance.gameObject);
        }
    }

    public override void OnServerDisconnect(NetworkConnection conn)
    {
        
        if (conn.identity != null)
        {
            var player = conn.identity.GetComponent<NetworkRoomPlayerLobby>();
            // Debug.Log("remove room player in managerlobby");
            RoomPlayers.Remove(player);
            // for (int i = PlayerConnections.Count-1; i>=0; i--)
            // {
            //     if (PlayerConnections[i].identity == conn.identity){
            //         PlayerConnections.Remove(PlayerConnections[i]);
            //         break;
            //     }
                    
            // }

            NotifyPlayersOfReadyState();
        }

        base.OnServerDisconnect(conn);
    }

    public override void OnStopServer()
    {
        OnServerStopped?.Invoke();

        RoomPlayers.Clear();
        GamePlayers.Clear();
    }

    public void NotifyPlayersOfReadyState()
    {
        foreach (var player in RoomPlayers)
        {
                player.HandleReadyToStart(IsReadyToStart());
        }
    }

    private bool IsReadyToStart()
    {
        if (numPlayers < minPlayers) { return false; }

        foreach (var player in RoomPlayers)
        {
            if (!player.IsReady) { return false; }
        }

        return true;
    }

    public void StartGame()
    {
        if (SceneManager.GetActiveScene().path == menuScene)
        {
            if (!IsReadyToStart()) { return; }

            //mapHandler = new MapHandler(mapSet, numberOfRounds);

            //ServerChangeScene(mapHandler.NextMap);
            ServerChangeScene("RaceScene");
        }
    }

    public override void ServerChangeScene(string newSceneName)
    {
            // From menu to game
            if (SceneManager.GetActiveScene().path == menuScene && newSceneName.StartsWith("RaceScene"))
            {
                for (int i = RoomPlayers.Count - 1; i >= 0; i--)
                {
                    var conn = RoomPlayers[i].connectionToClient;
                    var gameplayerInstance = Instantiate(gamePlayerPrefab);
                    gameplayerInstance.SetDisplayName(RoomPlayers[i].DisplayName);
                    NetworkServer.Destroy(conn.identity.gameObject);

                    NetworkServer.ReplacePlayerForConnection(conn, gameplayerInstance.gameObject, true);
                }
            }

            base.ServerChangeScene(newSceneName);
    }

    public override void OnServerSceneChanged(string sceneName)
    {
        if (sceneName.StartsWith("RaceScene"))
        {
            
            GameObject playerSpawnSystemInstance = Instantiate(playerSpawnSystem);
            NetworkServer.Spawn(playerSpawnSystemInstance);

            GameObject roundSystemInstance = Instantiate(roundSystem);
            NetworkServer.Spawn(roundSystemInstance);
            // foreach(NetworkConnection conn2 in PlayerConnections){
            //     // NetworkServer.ReplacePlayerForConnection(conn2, playerSpawnSystemInstance.gameObject, true);
            //     // NetworkServer.ReplacePlayerForConnection(conn2, roundSystemInstance.gameObject, true);
            //     GameObject playerSpawnSystemInstance2 = Instantiate(playerSpawnSystem);
            //     NetworkServer.Spawn(playerSpawnSystemInstance2,conn2);

            //     GameObject roundSystemInstance2 = Instantiate(roundSystem);
            //     NetworkServer.Spawn(roundSystemInstance2,conn2);
            // }
            for (int i = RoomPlayers.Count - 1; i >= 0; i--)
            {
                var conn = RoomPlayers[i].connectionToClient;
                NetworkServer.AddPlayerForConnection(conn, playerSpawnSystemInstance);
                NetworkServer.AddPlayerForConnection(conn, roundSystemInstance);
            }
            // Debug.Log("spawn in the race scene. Add connection!");
        }
        
    }

    public override void OnServerReady(NetworkConnection conn)
    {
        base.OnServerReady(conn);

        OnServerReadied?.Invoke(conn);
    }
    
}
