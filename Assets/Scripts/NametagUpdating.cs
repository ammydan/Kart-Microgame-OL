// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// using Mirror;
// using TMPro;
// using UnityEngine.UI;
// using System.Runtime.InteropServices;

// public class NametagUpdateing : NetworkBehaviour
// {

// 	[SerializeField] private TMP_Text[] playerNameTexts = new TMP_Text[4];

// 	[SyncVar(hook = nameof(HandleDisplayNameChanged))]
//     public string DisplayName = "NameDefault";

//     private NetworkManagerLobby room;
//     public NetworkManagerLobby Room
//     {
//         get
//         {
//             if (room != null) { return room; }
//             room = NetworkManager.singleton as NetworkManagerLobby;
//             return room;
//         }
//     }

//     public override void OnStartAuthority()
//     {
//         CmdSetDisplayName(PlayerNameInput.DisplayName);

//         lobbyUI.SetActive(true);
//     }

//     public void HandleDisplayNameChanged(string oldValue, string newValue) => UpdateDisplay();

//     private void UpdateDisplay()
//     {
//         if (!hasAuthority)
//         {
//             foreach (var player in Room.RoomPlayers)
//             {
//                 if (player.hasAuthority)
//                 {
//                     player.UpdateDisplay();
//                     break;
//                 }
//             }
//             return;
//         }

//         for (int i = 0; i < playerNameTexts.Length; i++)
//         {
//             playerNameTexts[i].text = "NameDefault";
//         }
            
//         for (int i = 0; i < Room.RoomPlayers.Count; i++)
//         {
//             playerNameTexts[i].text = Room.RoomPlayers[i].DisplayName;
//         }
//     }

//     [Command]
//     private void CmdSetDisplayName(string displayName)
//     {
//         DisplayName = displayName;
//     }


// }
