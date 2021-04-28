using Mirror;
using System.Linq;
using UnityEngine;

    public class RoundSystem : NetworkBehaviour
    {
        [SerializeField] private Animator animator = null;

        private NetworkManagerLobby room;
        private NetworkManagerLobby Room
        {
            get
            {
                if (room != null) { return room; }
                return room = NetworkManager.singleton as NetworkManagerLobby;
            }
        }

        public void CountdownEnded()
        {
            animator.enabled = false;
        }

        #region Server

        public override void OnStartServer()
        {
            
            NetworkManagerLobby.OnServerStopped += CleanUpServer;
            NetworkManagerLobby.OnServerReadied += CheckToStartRound;
        }

        [ServerCallback]
        private void OnDestroy() => CleanUpServer();

        [Server]
        private void CleanUpServer()
        {
            NetworkManagerLobby.OnServerStopped -= CleanUpServer;
            NetworkManagerLobby.OnServerReadied -= CheckToStartRound;
        }

        [ServerCallback]
        public void StartRound()
        {
            RpcStartRound();
        }

        [Server]
        private void CheckToStartRound(NetworkConnection conn)
        {
            if (Room.GamePlayers.Count(x => x.connectionToClient.isReady) != Room.GamePlayers.Count) { 
                // Debug.Log("Some one is not ready!!!");
                return; 
            }
            // if(isServer)Debug.Log("checkToStartRound in the server!!!!");
            // Debug.Log("Server start to count down!");
            RpcStartCountdown();
            // Debug.Log("have start count down in the client rpc");
            animator.enabled = true;

            
        }

        #endregion

        #region Client
        public override void OnStartClient(){
            // Debug.Log("hello in the client RoundSystem!");
        }

        [ClientRpc]
        private void RpcStartCountdown()
        {
            // Debug.Log("Client start to count down 1!");
            animator.enabled = true;
        }

        [ClientRpc]
        private void RpcStartRound()
        {
            // Debug.Log("Client start round2!");
            InputManager.Remove(ActionMapNames.Player);
        }

        #endregion
    }
