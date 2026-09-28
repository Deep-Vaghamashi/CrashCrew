using UnityEngine;
using Unity.Netcode;

public class PlayerScore : NetworkBehaviour
{
    public NetworkVariable<int> Score =
        new NetworkVariable<int>(
            0,
            NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Server
        );

    public void AddScore(int amount)
    {
        if (!IsServer)
            return;

        Score.Value += amount;
    }

    public void ResetScore()
    {
        if (!IsServer)
            return;

        Score.Value = 0;
    }
}