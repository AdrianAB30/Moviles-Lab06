using Unity.Netcode;
using UnityEngine;

public class UIManager : NetworkBehaviour
{
    public GameObject lobbyPanel;
    public GameObject customizationPanel;

    private void Start()
    {
        ShowLobby();
    }

    public void ShowLobby()
    {
        if (lobbyPanel) lobbyPanel.SetActive(true);
        if (customizationPanel) customizationPanel.SetActive(false);
    }

    public void ShowCustomization()
    {
        if (lobbyPanel) lobbyPanel.SetActive(false);
        if (customizationPanel) customizationPanel.SetActive(true);
    }
    public void JoinPlayer()
    {
        NetworkManager.Singleton.StartClient();
    }
}
