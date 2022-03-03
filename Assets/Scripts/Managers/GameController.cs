using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{

    public static GameController Instance;
    bool paused;
    bool mainMenu;
    bool playing;
    public GameObject explosionPrefab;
    public GameObject playerGO;
    public GameObject talibanGO;
    public GameObject bossGO;
    public Animator portonController;
    public int enemigosPorAparecer;
    private int enemigosVivosEsteNivel;
    public int maxEnemigosALaVez;

    public void Awake()
    {
        Instance = this;
        Application.targetFrameRate = 60;
        StartCoroutine("KeepAccelerometerAlive");
        Playing = true;
    }

    IEnumerator KeepAccelerometerAlive()
    {
        yield return Input.acceleration.sqrMagnitude;
        yield return new WaitForSeconds(1.0f);
    }

    void Start()
    {
        if (playing)
        {
            StartGame();
        }
    }

    public virtual void PlayerLostLife()
    {
        // deal with player life lost (update U.I. etc.)
    }

    public virtual void SpawnPlayer()
    {
        // the player needs to be spawned
        //GameObject player = Instantiate(playerGO, new Vector3(-45, 0.0f, -5), Quaternion.Euler(0, 135, 0));
        playerGO.SetActive(true);
    }
    public virtual void Respawn()
    {
        // the player is respawning
    }
    public virtual void StartGame()
    {
        UpdateGUIValues();
        SoundController.Instance.PlaySoundByIndex(10, this.transform.position);
        enemigosVivosEsteNivel = 0;
        // do start game functions
        Invoke("SpawnPlayer", 1);
        //InvokeRepeating("SpawnEnemy", 1, 10);
        StartCoroutine("SpawnEnemies");
    }

    public virtual void GameOver()
    {
        enemigosVivosEsteNivel = 0;
        StartCoroutine("FinDePartida");
    }

    IEnumerator FinDePartida()
    {
        yield return new WaitForSeconds(3f);
        SoundController.Instance.PlaySoundByIndex(9, this.transform.position);
        yield return new WaitForSeconds(4f);
        UIController.Instance.GoToMainMenu();
    }

    IEnumerator SpawnEnemies()
    {
        yield return new WaitForSeconds(0.3f);

        while (enemigosPorAparecer > 0 )
        {

            if (enemigosVivosEsteNivel > maxEnemigosALaVez-1)
            {
                yield return new WaitForSeconds(3f);
                continue;
            }
            yield return new WaitForSeconds(4f);
            GameObject enemy = null;
            EnemyController enemyScript;
            // the player needs to be spawned
            if (enemigosPorAparecer > 1)
            {
                enemy = Instantiate(talibanGO, new Vector3(0.5f, 0.0f, -30.5f), Quaternion.Euler(0, -45, 0));
                //yield return new WaitForSeconds(0.5f);
                enemy.GetComponent<EnemyController>().ApplyDestination();
                
            }
            else if (enemigosPorAparecer > 0)
            {
                enemy = Instantiate(talibanGO, new Vector3(0.5f, 0.0f, -30.5f), Quaternion.Euler(0, -45, 0));
                enemy.gameObject.transform.name = enemy.gameObject.transform.name + "BOSS";
                yield return new WaitForSeconds(0.5f);
                enemy.GetComponent<EnemyController>().ApplyDestination();
                
            }
            enemigosPorAparecer--;
            enemigosVivosEsteNivel++;
            UpdateGUIValues();
            portonController.SetTrigger("AbrePorton");
            enemyScript = enemy.GetComponent<EnemyController>();
            //yield return new WaitForSeconds(3f);
            while (!enemyScript.exitGarage && !enemyScript.isDead)
            {
                yield return new WaitForSeconds(0.25f);
            }
            //Debug.LogError("CierraPorton");
            portonController.SetTrigger("CierraPorton");
            //enemy.GetComponent<BoxCollider>().enabled = true;

        }
        //Debug.LogError("FINISHED SPAWNING ENEMIES ");
    }

    void UpdateGUIValues()
    {
        UIController.Instance.UpdateEnemiesToSpawn(enemigosPorAparecer);
    }



    public virtual void SpawnBoss()
    {
        // the player needs to be spawned
        GameObject player = Instantiate(talibanGO, new Vector3(-45, 0.0f, -5), Quaternion.Euler(0, 135, 0));
        //playerGO.SetActive(true);
    }

    public void Explode(Vector3 aPosition)
    {
        // instantiate an explosion at the position passed into this function
        Instantiate(explosionPrefab, aPosition, Quaternion.identity);
    }
    public virtual void EnemyDestroyed()
    {
        enemigosVivosEsteNivel--;

        // deal with enemy destroyed
        if (enemigosVivosEsteNivel == 0 && enemigosPorAparecer == 0)
        {
            StartCoroutine("EndOfLevelVictory");
        }
    }

    IEnumerator EndOfLevelVictory()
    {
        yield return new WaitForSeconds(1.5f);
        SoundController.Instance.PlaySoundByIndex(3, this.transform.position);
        yield return new WaitForSeconds(4.0f);
        SceneManager.LoadScene("GameScene");
    }

    public virtual void BossDestroyed()
    {
        // deal with the end of a boss battle
    }
    public virtual void RestartGameButtonPressed()
    {
        // deal with restart button (default behavior re-loads the currently loaded scene)
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public bool Paused
    {
        get
        {
            // get paused
            return paused;
        }
        set
        {
            // set paused 
            paused = value;
            if (paused)
            {
                // pause time
                Time.timeScale = 0f;
            }
            else
            {
                // unpause Unity
                Time.timeScale = 1f;
            }
        }
    }

    public bool Playing
    {
        get
        {
            // get paused
            return playing;
        }
        set
        {
            // set paused 
            playing = value;
        }
    }
}

