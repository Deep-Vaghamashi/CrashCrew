using UnityEngine;
using Unity.Netcode;

public class PlayerHealth : NetworkBehaviour
{
    [SerializeField] private float maxHealth = 100f;

    public NetworkVariable<float> CurrentHealth =
        new NetworkVariable<float>(
            100f,
            NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Server
        );

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            CurrentHealth.Value = maxHealth;
        }
    }

    public void TakeDamage(float damage)
    {
        if (!IsServer)
            return;

        if (CurrentHealth.Value <= 0f)
            return;

        CurrentHealth.Value -= damage;

        if (CurrentHealth.Value <= 0f)
        {
            CurrentHealth.Value = 0f;
            DestroyPlayer();
        }
    }

    private void DestroyPlayer()
    {
        NetworkObject networkObject = GetComponent<NetworkObject>();

        if (networkObject == null)
            return;

        ulong clientId = OwnerClientId;

        PlayerRespawnManager respawnManager =
            FindFirstObjectByType<PlayerRespawnManager>();

        networkObject.Despawn();

        if (respawnManager != null)
        {
            respawnManager.RequestRespawn(clientId);
        }
    }

    public void ResetHealth()
    {
        if (!IsServer)
            return;

        CurrentHealth.Value = maxHealth;
    }

    private void OnGUI()
    {
        if (!IsOwner) return;

        GUI.Label(
            new Rect(20, 140, 300, 30),
            "Health: " + Mathf.CeilToInt(CurrentHealth.Value)
            );
    }
}