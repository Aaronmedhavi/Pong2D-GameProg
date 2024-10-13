using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;

public class NetworkUI : NetworkBehaviour
{
    [SerializeField] private Button hostButton;
    [SerializeField] private Button clientButton;
    [SerializeField] private GameObject lobbyPanel;
    [SerializeField] private GameObject gameUIPanel;

    void Start()
    {
        hostButton.onClick.AddListener(OnHostButtonPressed);
        clientButton.onClick.AddListener(OnClientButtonPressed);
        lobbyPanel.SetActive(true);
        gameUIPanel.SetActive(false);
        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
    }

    private void OnHostButtonPressed()
    {
        NetworkManager.Singleton.StartHost();
        Debug.Log("Host started");
    }

    private void OnClientButtonPressed()
    {
        NetworkManager.Singleton.StartClient();
        Debug.Log("Client started");
    }

    private void OnClientConnected(ulong clientId)
    {
        if (clientId == NetworkManager.Singleton.LocalClientId)
        {
            Debug.Log("Client connected successfully");
            SwitchToGameUI();
        }
    }

    private void SwitchToGameUI()
    {
        lobbyPanel.SetActive(false);
        gameUIPanel.SetActive(true);
    }
}
