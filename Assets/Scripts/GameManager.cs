using UnityEngine;
using Unity.Netcode;

public class GameManager : NetworkBehaviour
{
    public enum MatchState
    {
        Lobby,
        Countdown,
        Playing,
        Results
    }

    public NetworkVariable<MatchState> CurrentState =
        new NetworkVariable<MatchState>(
            MatchState.Lobby,
            NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Server
        );

    public NetworkVariable<float> MatchTimeRemaining =
        new NetworkVariable<float>(
            300f,
            NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Server
        );

    [SerializeField] private float countdownDuration = 10f;

    private float countdownTimer;

    public override void OnNetworkSpawn()
    {
        if (!IsServer)
            return;

        StartMatch();
    }

    private void Update()
    {
        if (!IsServer)
            return;

        if (CurrentState.Value == MatchState.Countdown)
        {
            UpdateCountdown();
        }
        else if (CurrentState.Value == MatchState.Playing)
        {
            UpdateMatchTimer();
        }
    }

    private void StartMatch()
    {
        CurrentState.Value = MatchState.Countdown;
        countdownTimer = countdownDuration;
        MatchTimeRemaining.Value = 30f;
    }

    private void UpdateCountdown()
    {
        countdownTimer -= Time.deltaTime;

        if (countdownTimer <= 0f)
        {
            CurrentState.Value = MatchState.Playing;
        }
    }

    private void UpdateMatchTimer()
    {
        MatchTimeRemaining.Value -= Time.deltaTime;

        if (MatchTimeRemaining.Value <= 0f)
        {
            MatchTimeRemaining.Value = 0f;
            CurrentState.Value = MatchState.Results;
        }
    }

    private void ResetPlayerScores()
    {
        PlayerScore[] players =
            FindObjectsByType<PlayerScore>(FindObjectsSortMode.None);

        foreach (PlayerScore player in players)
        {
            player.ResetScore();
        }
    }
}