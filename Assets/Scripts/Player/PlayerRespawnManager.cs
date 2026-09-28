using System.Collections;
using UnityEngine;
using Unity.Netcode;

public class PlayerRespawnManager : MonoBehaviour
{
    [SerializeField] private NetworkObject playerPrefab;
    [SerializeField] private float respawnDelay = 3f;

    public Transform[] spawnPoints;

    public void RequestRespawn(ulong clientId)
    {
        if (NetworkManager.Singleton == null)
            return;

        if (!NetworkManager.Singleton.IsServer)
            return;

        StartCoroutine(RespawnAfterDelay(clientId));
    }

    private IEnumerator RespawnAfterDelay(ulong clientId)
    {
        yield return new WaitForSeconds(respawnDelay);

        if (!NetworkManager.Singleton.ConnectedClients.ContainsKey(clientId))
            yield break;

        int spawnIndex = FindAvailableSpawnPoint();

        Transform spawnPoint = spawnPoints[spawnIndex];

        NetworkObject newPlayer =
            Instantiate(
                playerPrefab,
                spawnPoint.position,
                spawnPoint.rotation
            );

        newPlayer.SpawnAsPlayerObject(clientId, true);

        PlayerHealth health =
            newPlayer.GetComponent<PlayerHealth>();

        if (health != null)
        {
            health.ResetHealth();
        }
    }

    private int FindAvailableSpawnPoint()
    {
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            Collider[] nearbyColliders = Physics.OverlapSphere(
                spawnPoints[i].position,
                2.5f
            );

            bool occupied = false;

            foreach (Collider collider in nearbyColliders)
            {
                if (collider.GetComponent<PlayerCarController>() != null)
                {
                    occupied = true;
                    break;
                }
            }

            if (!occupied)
            {
                return i;
            }
        }

        // If every spawn point is occupied, use the first one.
        return 0;
    }
}