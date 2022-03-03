using UnityEngine.SceneManagement;
using UnityEngine;
using System.Collections;

public class UIController : MonoBehaviour
{
    public static UIController Instance;
    public int player_score;
    public int player_lives;
    public int player_highscore;
    public TMPro.TextMeshProUGUI enemiesToSpawn;
    public GameObject life1;
    public GameObject life2;
    public GameObject life3;
    public GameObject life4;

    private GameController gameController;

    public void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        gameController = this.GetComponent<GameController>();
    }

    public void UpdateScoreP1(int aScore)
    {
        player_score = aScore;
        if (player_score > player_highscore)
            player_highscore = player_score;
    }

    public void UpdateHealthP1(int alifeNum)
    {
        player_lives = alifeNum;

        switch (alifeNum)
        {
            case 4:
                {
                    life1.SetActive(true);
                    life2.SetActive(true);
                    life3.SetActive(true);
                    life4.SetActive(true);
                    break;
                }
            case 3:
                {
                    life1.SetActive(true);
                    life2.SetActive(true);
                    life3.SetActive(true);
                    life4.SetActive(false);
                    break;
                }
            case 2:
                {
                    life1.SetActive(true);
                    life2.SetActive(true);
                    life3.SetActive(false);
                    life4.SetActive(false);
                    break;
                }
            case 1:
                {
                    life1.SetActive(true);
                    life2.SetActive(false);
                    life3.SetActive(false);
                    life4.SetActive(false);
                    break;
                }
            default:
                life1.SetActive(false);
                life2.SetActive(false);
                life3.SetActive(false);
                life4.SetActive(false);
                break;
        }


    }

    public void UpdateScore(int aScore)
    {
        player_score = aScore;
    }

    public void UpdateLives(int alifeNum)
    {
        player_lives = alifeNum;
    }

    public void UpdateEnemiesToSpawn(int number)
    {
        enemiesToSpawn.text = "" + number;
    }

    public void LoadHighScore()
    {
        // grab high score from prefs
        if (PlayerPrefs.HasKey("_highScore"))
        {
            player_highscore = PlayerPrefs.GetInt("_highScore");
        }
    }

    public void SaveHighScore()
    {
        // as we know that the game is over, let's save out the high score too
        PlayerPrefs.SetInt("_highScore", player_highscore);
    }

    public void PauseToggle()
    {
        SoundController.Instance.PlaySoundByIndex(0, this.transform.position);
        gameController.Paused = !gameController.Paused;
    }

    public void StartGame()
    {
        StartCoroutine("LoadGame");
    }

    IEnumerator LoadGame()
    {
        GameObject.Find("CanvasMainMenu").transform.GetComponent<AudioSource>().Stop();
        SoundController.Instance.PlaySoundByIndex(12, this.transform.position);
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene("GameScene", LoadSceneMode.Single);
        gameController.Playing = true;
        gameController.StartGame();
    }

public void GoToMainMenu()
    {
        gameController.Playing = false;
        SceneManager.LoadScene("Main Menu", LoadSceneMode.Single);
    }
}
