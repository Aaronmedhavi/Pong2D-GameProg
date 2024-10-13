using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.Netcode;

public class GameManager : NetworkBehaviour
{
    public SceneManagement SceneManagement;
    public GameObject Panelwin;
    public string textPlayerL, textPlayerR;
    public TMP_Text UIPlayerWin;
    public GameObject explosionPrefab;
    public TMP_Text txtPlayerScoreL;
    public TMP_Text txtPlayerScoreR;

    private NetworkVariable<int> PlayerScoreL = new NetworkVariable<int>();
    private NetworkVariable<int> PlayerScoreR = new NetworkVariable<int>();

    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public override void OnNetworkSpawn()
    {
        PlayerScoreL.OnValueChanged += OnScoreChanged;
        PlayerScoreR.OnValueChanged += OnScoreChanged;
        UpdateScoreUI();
    }

    public override void OnNetworkDespawn()
    {
        PlayerScoreL.OnValueChanged -= OnScoreChanged;
        PlayerScoreR.OnValueChanged -= OnScoreChanged;
    }

    private void OnScoreChanged(int previousValue, int newValue)
    {
        UpdateScoreUI();
        if (IsServer)
        {
            ScoreCheck();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void ScoreServerRpc(string wallID)
    {
        if (wallID == "BorderLeft")
        {
            PlayerScoreR.Value += 10;
            TriggerExplosionClientRpc(0); // Player 1 (OwnerClientId 0) scored against
        }
        else
        {
            PlayerScoreL.Value += 10;
            TriggerExplosionClientRpc(1); // Player 2 (OwnerClientId 1) scored against
        }
    }

    private void UpdateScoreUI()
    {
        txtPlayerScoreL.text = PlayerScoreL.Value.ToString();
        txtPlayerScoreR.text = PlayerScoreR.Value.ToString();
    }

    private void ScoreCheck()
    {
        if (PlayerScoreL.Value == 50)
        {
            EndGameClientRpc(textPlayerL);
        }
        else if (PlayerScoreR.Value == 50)
        {
            EndGameClientRpc(textPlayerR);
        }
    }

    [ClientRpc]
    private void EndGameClientRpc(string winnerText)
    {
        UIPlayerWin.text = winnerText;
        Panelwin.SetActive(true);
        Invoke(nameof(ChangeSceneToMenu), 2f);
    }

    private void ChangeSceneToMenu()
    {
        SceneManagement.ChangeScene("MainMenu");
    }

    [ClientRpc]
    private void TriggerExplosionClientRpc(int playerScoredAgainst)
    {
        PlayerControl[] players = FindObjectsOfType<PlayerControl>();
        foreach (PlayerControl player in players)
        {
            if ((int)player.OwnerClientId == playerScoredAgainst)
            {
                SpawnExplosion(player.transform.position);
                break;
            }
        }
    }

    private void SpawnExplosion(Vector3 position)
    {
        if (explosionPrefab != null)
        {
            GameObject explosion = Instantiate(explosionPrefab, position, Quaternion.identity);
            AudioSource explosionAudio = explosion.GetComponent<AudioSource>();
            if (explosionAudio != null)
            {
                explosionAudio.Play();
            }
            Destroy(explosion, 1f);
        }
    }
}