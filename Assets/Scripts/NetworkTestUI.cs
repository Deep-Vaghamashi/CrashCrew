using UnityEngine;
using Unity.Netcode;

public class NetworkTestUI : MonoBehaviour
{
    private void OnGUI()
    {
        NetworkManager networkManager = NetworkManager.Singleton;

        if (networkManager == null)
        {
            return;
        }

        if (!networkManager.IsClient && !networkManager.IsServer)
        {
            if (GUI.Button(new Rect(20, 20, 150, 50), "START HOST"))
            {
                networkManager.StartHost();
            }

            if (GUI.Button(new Rect(20, 80, 150, 50), "START CLIENT"))
            {
                networkManager.StartClient();
            }
        }
    }
}