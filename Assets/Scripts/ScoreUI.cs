using UnityEngine;
using TMPro;
using Unity.Netcode;

public class ScoreUI : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;

    private PlayerScore localPlayerScore;

    private void Update()
    {
        if (localPlayerScore == null)
        {
            FindLocalPlayerScore();
            return;
        }

        scoreText.text = "Score: " + localPlayerScore.Score.Value;
    }

    private void FindLocalPlayerScore()
    {
        if (NetworkManager.Singleton == null)
            return;

        if (!NetworkManager.Singleton.IsClient)
            return;

        foreach (PlayerScore playerScore in
                 FindObjectsByType<PlayerScore>(FindObjectsSortMode.None))
        {
            if (playerScore.IsOwner)
            {
                localPlayerScore = playerScore;
                return;
            }
        }
    }
}