using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
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

    public void Score(string wallID)
    {
        if (wallID == "BorderLeft")
        {
            PlayerScoreR = PlayerScoreR + 10;
            txtPlayerScoreR.text = PlayerScoreR.ToString();
            TriggerExplosion("PlayerL");
            ScoreCheck();
        }
        else
        {
            PlayerScoreL = PlayerScoreL + 10;
            txtPlayerScoreL.text = PlayerScoreL.ToString();
            TriggerExplosion("PlayerR");
            ScoreCheck();
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

    public void ScoreCheck()
    {
        if (PlayerScoreL == 50)
        {
            Debug.Log("PlayerL Win");
            UIPlayerWin.text = textPlayerL;
            Panelwin.SetActive(true);
            ChangeSceneWin();
        }
        else if (PlayerScoreR == 50)
        {
            Debug.Log("PlayerR Win");
            UIPlayerWin.text = textPlayerR;
            Panelwin.SetActive(true);
            ChangeSceneWin();
        }
    }
    private void TriggerExplosion(string playerTag)
    {
        GameObject player = GameObject.FindGameObjectWithTag(playerTag);
        if (player != null && explosionPrefab != null)
        {
            GameObject explosion = Instantiate(explosionPrefab, player.transform.position, Quaternion.identity);
            AudioSource explosionAudio = explosion.GetComponent<AudioSource>();
            if (explosionAudio != null)
            {
                explosionAudio.Play();
            }
            Destroy(explosion, 1f);
        }
    }
}
