using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    bool paused;
    public GameObject explosionPrefab;
    public GameObject playerGO;
    public GameObject talibanGO;
    public GameObject bossGO;
    public Animator portonController;
    public int enemiesInLevel;

    void Start()
    {
        Application.targetFrameRate = 60;
        StartGame();
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
        // do start game functions
        Invoke("SpawnPlayer", 1);
        //InvokeRepeating("SpawnEnemy", 1, 10);
        StartCoroutine("SpawnEnemy");
    }

    IEnumerator SpawnEnemy()
    {
        yield return new WaitForSeconds(0.3f);

        while (enemiesInLevel > 0)
        {

            yield return new WaitForSeconds(8f);
            GameObject enemy = null;
            EnemyController enemyScript;
            // the player needs to be spawned
            if (enemiesInLevel > 1)
            {
                enemy = Instantiate(talibanGO, new Vector3(0.5f, 0.0f, -30.5f), Quaternion.Euler(0, -45, 0));
                //yield return new WaitForSeconds(0.5f);
                enemy.GetComponent<EnemyController>().ApplyDestination();
                enemiesInLevel--;
            }
            else if (enemiesInLevel > 0)
            {
                enemy = Instantiate(talibanGO, new Vector3(0.5f, 0.0f, -30.5f), Quaternion.Euler(0, -45, 0));
                enemy.gameObject.transform.name = enemy.gameObject.transform.name + "BOSS";
                yield return new WaitForSeconds(0.5f);
                enemy.GetComponent<EnemyController>().ApplyDestination();
                enemiesInLevel--;
            }
            portonController.SetTrigger("AbrePorton");
            enemyScript = enemy.GetComponent<EnemyController>();
            //yield return new WaitForSeconds(3f);
            while (!enemyScript.exitGarage && enemyScript.lives >= 0)
            {
                yield return new WaitForSeconds(0.25f);
            }
            //Debug.LogError("CierraPorton");
            portonController.SetTrigger("CierraPorton");
            //enemy.GetComponent<BoxCollider>().enabled = true;
            
        }
        //Debug.LogError("FINISHED SPAWNING ENEMIES ");
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
    public virtual void EnemyDestroyed(Vector3 aPosition, int pointsValue,
    int hitByID)
    {
        // deal with enemy destroyed
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
}

