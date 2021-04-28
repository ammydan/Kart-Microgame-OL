using UnityEngine;


    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private NetworkManagerLobby networkManager = null;

        [Header("UI")]
        [SerializeField] private GameObject landingPagePanel = null;

        public void HostLobby()
        {
            networkManager.StartHost();

            //next player do not need to turn on the landingPagePanel
            landingPagePanel.SetActive(false);
        }
    }
