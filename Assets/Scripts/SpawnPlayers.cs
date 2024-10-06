using Photon.Pun;
using UnityEngine;

public class PlayerSpawner : MonoBehaviourPunCallbacks
{
    public GameObject playerPrefab1; // Assign the first player prefab
    public GameObject playerPrefab2; // Assign the second player prefab
    public Transform spawnPosition1; // Assign spawn position for Player 1
    public Transform spawnPosition2; // Assign spawn position for Player 2

    void Start()
    {
        SpawnPlayer();
    }

    void SpawnPlayer()
    {
        // If this is the master client, spawn as Player 1, otherwise Player 2
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Instantiate(playerPrefab1.name, spawnPosition1.position, Quaternion.identity);
        }
        else
        {
            PhotonNetwork.Instantiate(playerPrefab2.name, spawnPosition2.position, Quaternion.identity);
        }
    }
}
