using UnityEngine;
using Unity.Netcode;

public class PlayerSpawnManager : MonoBehaviour
{
    public Transform[] spawnPoints;

    private void Start()
    {
        if (!NetworkManager.Singleton.IsServer)
            return;

        // Spawn the host player at the first spawn point.
        MovePlayerToSpawn(NetworkManager.Singleton.LocalClientId, 0);

        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
    }

    private void OnClientConnected(ulong clientId)
    {
        // Host is already handled above.
        if (clientId == NetworkManager.Singleton.LocalClientId)
            return;

        int spawnIndex = (int)(clientId % (ulong)spawnPoints.Length);
        MovePlayerToSpawn(clientId, spawnIndex);
    }

    private void MovePlayerToSpawn(ulong clientId, int spawnIndex)
    {
        if (!NetworkManager.Singleton.ConnectedClients.TryGetValue(
                clientId, out NetworkClient client))
            return;

        NetworkObject playerObject = client.PlayerObject;

        if (playerObject == null)
            return;

        playerObject.transform.SetPositionAndRotation(
            spawnPoints[spawnIndex].position,
            spawnPoints[spawnIndex].rotation
        );
    }

    private void OnDestroy()
    {
        if (NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnected;
        }
    }
}