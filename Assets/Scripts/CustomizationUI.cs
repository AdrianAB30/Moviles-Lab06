using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;

public class CustomizationUI : MonoBehaviour
{
    [Header("Botones Body")]
    public Button nextBody;
    public Button prevBody;
    [Header("Botones Eyes")]
    public Button nextEyes;
    public Button prevEyes;
    [Header("Botones Gloves")]
    public Button nextGloves;
    public Button prevGloves;
    [Header("Botones Head")]
    public Button nextHead;
    public Button prevHead;
    [Header("Botones Mouth")]
    public Button nextMouth;
    public Button prevMouth;

    private Player localPlayer;

    private void Start()
    {
        TryBindLocal();

        if (NetworkManager.Singleton != null)
            NetworkManager.Singleton.OnClientConnectedCallback += (id) => TryBindLocal();
    }

    private void TryBindLocal()
    {
        if (localPlayer != null) return;
        localPlayer = FindLocalPlayer();
        if (localPlayer == null) return;

        nextBody?.onClick.AddListener(() => localPlayer.ChangeBodyServerRpc(1));
        prevBody?.onClick.AddListener(() => localPlayer.ChangeBodyServerRpc(-1));

        nextEyes?.onClick.AddListener(() => localPlayer.ChangeEyesServerRpc(1));
        prevEyes?.onClick.AddListener(() => localPlayer.ChangeEyesServerRpc(-1));

        nextGloves?.onClick.AddListener(() => localPlayer.ChangeGlovesServerRpc(1));
        prevGloves?.onClick.AddListener(() => localPlayer.ChangeGlovesServerRpc(-1));

        nextHead?.onClick.AddListener(() => localPlayer.ChangeHeadServerRpc(1));
        prevHead?.onClick.AddListener(() => localPlayer.ChangeHeadServerRpc(-1));

        nextMouth?.onClick.AddListener(() => localPlayer.ChangeMouthServerRpc(1));
        prevMouth?.onClick.AddListener(() => localPlayer.ChangeMouthServerRpc(-1));
    }

    private Player FindLocalPlayer()
    {
        var players = Object.FindObjectsByType<Player>(FindObjectsSortMode.None);
        foreach (var p in players)
        {
            if (p.OwnerClientId == NetworkManager.Singleton.LocalClientId)
                return p;
        }
        return null;
    }
}
