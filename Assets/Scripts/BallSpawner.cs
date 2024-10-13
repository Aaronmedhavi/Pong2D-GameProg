using UnityEngine;
using Unity.Netcode;

public class BallSpawner : NetworkBehaviour
{
    [SerializeField] private GameObject ballPrefab;
    [SerializeField] private float initialSpawnDelay = 2f;
    [SerializeField] private float respawnDelay = 3f;

    private NetworkObject spawnedBall;

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            NetworkManager.OnClientConnectedCallback += OnClientConnected;
            NetworkManager.OnClientDisconnectCallback += OnClientDisconnected;
        }
    }

    public override void OnNetworkDespawn()
    {
        if (IsServer)
        {
            NetworkManager.OnClientConnectedCallback -= OnClientConnected;
            NetworkManager.OnClientDisconnectCallback -= OnClientDisconnected;
        }
    }

    private void OnClientConnected(ulong clientId)
    {
        Debug.Log($"Client connected with ID: {clientId}");
        CheckAndStartGame();
    }

    private void OnClientDisconnected(ulong clientId)
    {
        Debug.Log($"Client disconnected with ID: {clientId}");
        if (NetworkManager.ConnectedClientsList.Count < 2)
        {
            ResetGame();
        }
    }

    private void CheckAndStartGame()
    {
        if (IsServer && NetworkManager.ConnectedClientsList.Count == 2)
        {
            StartCoroutine(SpawnBallWithDelay(initialSpawnDelay));
        }
    }

    private System.Collections.IEnumerator SpawnBallWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SpawnBall();
    }

    private void SpawnBall()
    {
        if (IsServer)
        {
            if (spawnedBall != null)
            {
                spawnedBall.Despawn();
            }

            Vector3 spawnPosition = Vector3.zero;
            GameObject ballInstance = Instantiate(ballPrefab, spawnPosition, Quaternion.identity);
            spawnedBall = ballInstance.GetComponent<NetworkObject>();

            if (spawnedBall != null)
            {
                spawnedBall.Spawn();
                Debug.Log("Ball spawned on the server.");
            }
            else
            {
                Debug.LogError("Ball prefab is missing NetworkObject component!");
            }
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void ResetBallServerRpc()
    {
        if (spawnedBall != null)
        {
            spawnedBall.Despawn();
        }
        StartCoroutine(SpawnBallWithDelay(respawnDelay));
    }

    private void ResetGame()
    {
        if (IsServer)
        {
            if (spawnedBall != null)
            {
                spawnedBall.Despawn();
                spawnedBall = null;
            }
        }
    }
}