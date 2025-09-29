using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;

public class LobbyUI : MonoBehaviour
{
    [Header("UI References")]
    public Button readyButton;
    public Button startButton;

    private Player localPlayer;

    private void Start()
    {
        if (startButton != null)
        {
            startButton.gameObject.SetActive(true);
        }

        TryBindLocal();

        if (NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.OnClientConnectedCallback += (id) => TryBindLocal();
        }

        if (NetworkManager.Singleton != null && NetworkManager.Singleton.IsHost)
        {
            startButton.gameObject.SetActive(true);
        }
    }

    private void TryBindLocal()
    {
        if (localPlayer != null) return;

        localPlayer = FindLocalPlayer();
        if (localPlayer == null) return;

        readyButton?.onClick.AddListener(() => localPlayer.ToggleReadyServerRpc());
    }

    private Player FindLocalPlayer()
    {
        var players = Object.FindObjectsByType<Player>(FindObjectsSortMode.None);

        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].OwnerClientId == NetworkManager.Singleton.LocalClientId)
                return players[i];
        }

        return null;
    }

    public void OnStartGamePressed()
    {
        if (!NetworkManager.Singleton.IsHost) return;

        var players = Object.FindObjectsByType<Player>(FindObjectsSortMode.None);

        for (int i = 0; i < players.Length; i++)
        {
            if (!players[i].isReady.Value)
            {
                Debug.Log("No todos listos");
                return;
            }
        }
        NetworkManager.Singleton.SceneManager.LoadScene("Game",UnityEngine.SceneManagement.LoadSceneMode.Single);
    }
}
