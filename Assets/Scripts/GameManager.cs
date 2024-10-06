using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun; // Import Photon PUN
using Photon.Realtime;

public class GameManager : MonoBehaviourPunCallbacks
{
    public SceneManagement SceneManagement;
    public GameObject Panelwin;
    public string textPlayerL, textPlayerR;
    public TMP_Text UIPlayerWin;
    public GameObject explosionPrefab;

    public int PlayerScoreL = 0;
    public int PlayerScoreR = 0;

    public TMP_Text txtPlayerScoreL;
    public TMP_Text txtPlayerScoreR;

    public static GameManager instance;

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        txtPlayerScoreL.text = PlayerScoreL.ToString();
        txtPlayerScoreR.text = PlayerScoreR.ToString();
    }

    // This method is called when a score occurs, but now it's updated across the network
    public void Score(string wallID)
    {
        if (PhotonNetwork.IsMasterClient) // Ensure only the Master Client handles the score logic
        {
            if (wallID == "BorderLeft")
            {
                // Call RPC to update Player R's score on all clients
                photonView.RPC("UpdateScoreR", RpcTarget.AllBuffered);
                TriggerExplosion("PlayerL");
            }
            else
            {
                // Call RPC to update Player L's score on all clients
                photonView.RPC("UpdateScoreL", RpcTarget.AllBuffered);
                TriggerExplosion("PlayerR");
            }

            // Check if anyone has won
            photonView.RPC("ScoreCheck", RpcTarget.AllBuffered);
        }
    }

    // RPC method to update Player L's score on all clients
    [PunRPC]
    public void UpdateScoreL()
    {
        PlayerScoreL += 10;
        txtPlayerScoreL.text = PlayerScoreL.ToString();
    }

    // RPC method to update Player R's score on all clients
    [PunRPC]
    public void UpdateScoreR()
    {
        PlayerScoreR += 10;
        txtPlayerScoreR.text = PlayerScoreR.ToString();
    }

    // RPC method to check the score and declare a winner if the condition is met
    [PunRPC]
    public void ScoreCheck()
    {
        if (PlayerScoreL >= 50)
        {
            Debug.Log("PlayerL Win");
            UIPlayerWin.text = textPlayerL;
            Panelwin.SetActive(true);
            ChangeSceneWin();
        }
        else if (PlayerScoreR >= 50)
        {
            Debug.Log("PlayerR Win");
            UIPlayerWin.text = textPlayerR;
            Panelwin.SetActive(true);
            ChangeSceneWin();
        }
    }

    private void ChangeSceneWin()
    {
        Invoke("ChangeSceneToMenu", 2f);
    }

    private void ChangeSceneToMenu()
    {
        SceneManagement.ChangeScene("MainMenu");
    }

    private void TriggerExplosion(string playerTag)
    {
        GameObject player = GameObject.FindGameObjectWithTag(playerTag);
        if (player != null && explosionPrefab != null)
        {
            // Use PhotonNetwork.Instantiate to spawn the explosion across the network
            GameObject explosion = PhotonNetwork.Instantiate(explosionPrefab.name, player.transform.position, Quaternion.identity);

            // Play the explosion sound (this is synchronized since it's part of the prefab)
            AudioSource explosionAudio = explosion.GetComponent<AudioSource>();
            if (explosionAudio != null)
            {
                explosionAudio.Play();
            }

            // Start a coroutine to destroy the explosion after a delay
            StartCoroutine(DestroyExplosionAfterDelay(explosion, 1f));
        }
    }

    // Coroutine to destroy the explosion after a delay
    private IEnumerator DestroyExplosionAfterDelay(GameObject explosion, float delay)
    {
        yield return new WaitForSeconds(delay);

        // Use PhotonNetwork.Destroy to destroy the object across the network
        PhotonNetwork.Destroy(explosion);
    }
}
