using System;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class Player : NetworkBehaviour
{
    [Header("Categorías")]
    public GameObject[] bodyParts;       
    public GameObject[] eyes;            
    public GameObject[] gloves;          
    public GameObject[] headParts;       
    public GameObject[] mouthsAndNoses;

    private NetworkVariable<int> bodyIndex = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    private NetworkVariable<int> eyeIndex = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    private NetworkVariable<int> gloveIndex = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    private NetworkVariable<int> headIndex = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    private NetworkVariable<int> mouthIndex = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    public NetworkVariable<bool> isReady = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    public Transform partsParent;
    [SerializeField] private TMP_Text readyText;

    public override void OnNetworkSpawn()
    {

        bodyIndex.OnValueChanged += (previous, current) => ApplyAll();
        eyeIndex.OnValueChanged += (oldValue, newValue) => ApplyAll();
        gloveIndex.OnValueChanged += (before, after) => ApplyAll();
        headIndex.OnValueChanged += (last, now) => ApplyAll();
        mouthIndex.OnValueChanged += (prev, next) => ApplyAll();

        isReady.OnValueChanged += (oldValue, newValue) =>
        {
            UpdateReadyUI(newValue);
        };

        UpdateReadyUI(isReady.Value);

        if (IsServer)
        {
            Vector3 pos = SpawnManager.Instance.AssignAndGetSpawnPosition(OwnerClientId);
            transform.position = pos;
        }

        ApplyAll();
    }

    public override void OnNetworkDespawn()
    {
        if (IsServer)
            SpawnManager.Instance.RemoveAssignment(OwnerClientId);
    }

    private void ApplyAll()
    {
        ActivateOnly(bodyParts, bodyIndex.Value);
        ActivateOnly(eyes, eyeIndex.Value);
        ActivateOnly(gloves, gloveIndex.Value);
        ActivateOnly(headParts, headIndex.Value);
        ActivateOnly(mouthsAndNoses, mouthIndex.Value);
    }

    private void ActivateOnly(GameObject[] arr, int idx)
    {
        if (arr == null || arr.Length == 0) return;
        idx = ((idx % arr.Length) + arr.Length) % arr.Length;
        for (int i = 0; i < arr.Length; i++)
            if (arr[i] != null) arr[i].SetActive(i == idx);
    }

    [ServerRpc(RequireOwnership = true)]
    public void ChangeBodyServerRpc(int dir) => bodyIndex.Value = (bodyIndex.Value + dir + bodyParts.Length) % bodyParts.Length;

    [ServerRpc(RequireOwnership = true)]
    public void ChangeEyesServerRpc(int dir) => eyeIndex.Value = (eyeIndex.Value + dir + eyes.Length) % eyes.Length;

    [ServerRpc(RequireOwnership = true)]
    public void ChangeGlovesServerRpc(int dir) => gloveIndex.Value = (gloveIndex.Value + dir + gloves.Length) % gloves.Length;

    [ServerRpc(RequireOwnership = true)]
    public void ChangeHeadServerRpc(int dir) => headIndex.Value = (headIndex.Value + dir + headParts.Length) % headParts.Length;

    [ServerRpc(RequireOwnership = true)]
    public void ChangeMouthServerRpc(int dir) => mouthIndex.Value = (mouthIndex.Value + dir + mouthsAndNoses.Length) % mouthsAndNoses.Length;

    [ServerRpc(RequireOwnership = true)]
    public void ToggleReadyServerRpc()
    {
        isReady.Value = !isReady.Value;
    }
    private void UpdateReadyUI(bool ready)
    {
        if (readyText != null)
        {
            readyText.text = ready ? "READY" : "NOT READY";
            readyText.color = ready ? Color.green : Color.red;
        }
    }

    public new bool IsLocalPlayer() => OwnerClientId == NetworkManager.Singleton.LocalClientId;
}
