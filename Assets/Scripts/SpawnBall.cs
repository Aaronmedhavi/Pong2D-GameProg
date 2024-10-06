using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class SpawnBall : MonoBehaviourPunCallbacks
{
    public GameObject ballPrefab;    // Prefab for the ball
    public Transform ballSpawnPosition; // Ball spawn position

    private GameObject ballInstance;

    void Start()
    {
        TrySpawnBall();
    }

    void TrySpawnBall()
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount == 2 && ballInstance == null && PhotonNetwork.IsMasterClient)
        {
            ballInstance = PhotonNetwork.Instantiate(ballPrefab.name, ballSpawnPosition.position, Quaternion.identity);
        }
    }

    // Called when a player joins the room
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        TrySpawnBall();
    }

    // Called when a player leaves the room
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        // Destroy the ball if there's less than 2 players
        if (PhotonNetwork.CurrentRoom.PlayerCount < 2 && ballInstance != null)
        {
            PhotonNetwork.Destroy(ballInstance);
            ballInstance = null;
        }
    }
}
