using UnityEngine;
using TMPro;

public class MatchUI : MonoBehaviour
{
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private TMP_Text countdownText;
    [SerializeField] private TMP_Text resultsText;
    [SerializeField] private TMP_Text winnerText;

    private GameManager gameManager;

    private void Update()
    {
        if (gameManager == null)
        {
            gameManager = FindFirstObjectByType<GameManager>();

            if (gameManager == null)
                return;
        }

        UpdateTimer();
        UpdateCountdown();
        UpdateResults();
        UpdateWinner();
    }

    private void UpdateTimer()
    {
        float timeRemaining = gameManager.MatchTimeRemaining.Value;

        int minutes = Mathf.FloorToInt(timeRemaining / 60f);
        int seconds = Mathf.FloorToInt(timeRemaining % 60f);

        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    private void UpdateCountdown()
    {
        if (gameManager.CurrentState.Value ==
            GameManager.MatchState.Countdown)
        {
            countdownText.gameObject.SetActive(true);
            countdownText.text = "GET READY!";
        }
        else
        {
            countdownText.gameObject.SetActive(false);
        }
    }

    private void UpdateResults()
    {
        if (gameManager.CurrentState.Value ==
            GameManager.MatchState.Results)
        {
            resultsText.gameObject.SetActive(true);
        }
        else
        {
            resultsText.gameObject.SetActive(false);
        }
    }

    private void UpdateWinner()
    {
        if (gameManager.CurrentState.Value != GameManager.MatchState.Results)
        {
            winnerText.gameObject.SetActive(false);
            return;
        }

        PlayerScore[] players =
            FindObjectsByType<PlayerScore>(FindObjectsSortMode.None);

        PlayerScore winner = null;

        foreach (PlayerScore player in players)
        {
            if (winner == null ||
                player.Score.Value > winner.Score.Value)
            {
                winner = player;
            }
        }

        if (winner != null)
        {
            winnerText.gameObject.SetActive(true);
            winnerText.text = "Winner: Player " + winner.OwnerClientId;
        }
    }
}