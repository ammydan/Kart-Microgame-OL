using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.InteropServices;

    public class NetworkRoomPlayerLobby : NetworkBehaviour
    {
        [Header("UI")]
        [SerializeField] private GameObject lobbyUI = null;
        [SerializeField] private TMP_Text[] playerNameTexts = new TMP_Text[4];
        [SerializeField] private TMP_Text[] playerReadyTexts = new TMP_Text[4];
        [SerializeField] private Button startGameButton = null;

        [SyncVar(hook = nameof(HandleDisplayNameChanged))]
        public string DisplayName = "Loading...";
        [SyncVar(hook = nameof(HandleReadyStatusChanged))]
        public bool IsReady = false;



        private bool isLeader;
        public bool IsLeader
        {
            get{
                return isLeader;
            }
            set
            {
                isLeader = value;
                startGameButton.gameObject.SetActive(value);
                
            }
            
        }

        private NetworkManagerLobby room;
        public NetworkManagerLobby Room
        {
            get
            {
                if (room != null) { return room; }
                room = NetworkManager.singleton as NetworkManagerLobby;
                // room.test("hello "+room.numPlayers);
                return room;
            }
        }

        public override void OnStartAuthority()
        {
            CmdSetDisplayName(PlayerNameInput.DisplayName);

            lobbyUI.SetActive(true);
        }

        public override void OnStartClient()
        {
            Room.RoomPlayers.Add(this);
            // CmdAddPlayerForRoom(this);
            GCHandle hander = GCHandle.Alloc(Room.RoomPlayers);
            var pin = GCHandle.ToIntPtr(hander);
            if(Room.RoomPlayers.Count==1){
                startGameButton.gameObject.SetActive(true);
                isLeader = true;
                // startGameButton.interactable = true;
            }
            // Debug.Log("start!!!Client, number: "+Room.RoomPlayers.Count+"   "+startGameButton.gameObject.activeSelf+" isLeader: "+isLeader+" add things:"+this);
            // room.test("add client");

            UpdateDisplay();
        }

        public override void OnStopClient()
        {
            // Room.RoomPlayers.Remove(this);

            UpdateDisplay();
        }

        public void HandleReadyStatusChanged(bool oldValue, bool newValue) => UpdateDisplay();
        public void HandleDisplayNameChanged(string oldValue, string newValue) => UpdateDisplay();

        private void UpdateDisplay()
        {
            if (!hasAuthority)
            {
                foreach (var player in Room.RoomPlayers)
                {
                    if (player.hasAuthority)
                    {
                        player.UpdateDisplay();
                        break;
                    }
                }
                return;
            }
            // if (!isLocalPlayer)
            // {
            //     foreach (var player in Room.RoomPlayers)
            //     {
            //         if (player.isLocalPlayer)
            //         {
            //             player.UpdateDisplay();
            //             break;
            //         }
            //     }
            //     return;
            
            // }
            for (int i = 0; i < playerNameTexts.Length; i++)
            {
                playerNameTexts[i].text = "Waiting For Player...";
                playerReadyTexts[i].text = string.Empty;
            }
            
            for (int i = 0; i < Room.RoomPlayers.Count; i++)
            {
                playerNameTexts[i].text = Room.RoomPlayers[i].DisplayName;
                playerReadyTexts[i].text = Room.RoomPlayers[i].IsReady ?
                    "<color=green>Ready</color>" :
                    "<color=red>Not Ready</color>";

            }
        }

        public void HandleReadyToStart(bool readyToStart)
        {
            if (!isLeader) { return; };
            startGameButton.interactable = readyToStart;
            Readyup(readyToStart);
            // startGameButton.interactable = true;
        }
        [ClientRpc]
        private void Readyup(bool readyToStart){
            if(!isLeader){return;}
            startGameButton.interactable = readyToStart;
        }

        [Command]
        private void CmdSetDisplayName(string displayName)
        {
            DisplayName = displayName;
        }

        [Command]
        public void CmdReadyUp()
        {
            // Debug.Log("update the  ready state!!!");
            IsReady = !IsReady;
            Room.NotifyPlayersOfReadyState();
        }

        [Command]
        public void CmdStartGame()
        {
            Debug.Log("numbers: "+Room.RoomPlayers.Count);
            if (Room.RoomPlayers[0].connectionToClient != connectionToClient) { return; }

            Room.StartGame();
        }
    }
