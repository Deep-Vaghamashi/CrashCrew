using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;

public class NetworkBootstrap : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        NetworkManager.Singleton.OnServerStarted += OnServerStarted;
    }

    private void OnServerStarted()
    {
        if (!NetworkManager.Singleton.IsServer)
            return;

        NetworkManager.Singleton.SceneManager.LoadScene(
            "Arena",
            LoadSceneMode.Single
        );
    }
}