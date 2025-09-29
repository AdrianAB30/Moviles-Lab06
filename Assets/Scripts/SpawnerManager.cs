using UnityEngine;
using Unity.Netcode;
using System.Collections.Generic;

public class SpawnManager : NetworkBehaviour
{
    public static SpawnManager Instance;

    [Header("Spawn points")]
    public Transform[] spawnPoints;

    private Dictionary<ulong, int> assigned = new Dictionary<ulong, int>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    public Vector3 AssignAndGetSpawnPosition(ulong clientId)
    {
        if (assigned.ContainsKey(clientId))
            return spawnPoints[assigned[clientId]].position;

        int index = assigned.Count;
        if (index >= spawnPoints.Length) index = index % spawnPoints.Length;
        assigned[clientId] = index;
        return spawnPoints[index].position;
    }
    public void RemoveAssignment(ulong clientId)
    {
        if (assigned.ContainsKey(clientId))
            assigned.Remove(clientId);
    }
}
