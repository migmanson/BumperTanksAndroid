using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{

	public static GameController Instance;
	bool paused;
	bool jugandoNivel;
	public GameObject explosionPrefab;
	public GameObject playerGO;
	public GameObject talibanGO;
	public GameObject bossGO;
	public GameObject mainVCam;
	public GameObject alumnasVCam;
	public GameObject winVCam;

	public Animator portonController;
	public int enemigosPorAparecer;
	private int enemigosVivosEsteNivel;
	private int maxEnemigosALaVez;
	public Color[] enemyColors;
	public Texture2D[] enemyDecals;
	public List<GameObject> propsSpawnPoints;
	public GameObject[] propPrefabs;

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
		if (!Grid.game.GetPartidaComenzada())
		{
			Invoke("ComenzarPartida", 0.2f);
		}
		else
		{
			ComenzarNivel();
		}
	}

	public virtual void ComenzarPartida()
	{
		Grid.game.GetDefaultData();
		Grid.playerStats.GetDefaultPlayerData();
		//Grid.game.SetLevel(15);
		ComenzarNivel();
	}
	public virtual void TerminarPartida(bool fromPauseMenu = false)
	{
		//Debug.LogError("TerminarPartida");
		enemigosVivosEsteNivel = 0;
		Grid.game.SetPartidaComenzada(false);
		Grid.game.SetPartidaTerminada(true);
		TerminarNivel();
		StartCoroutine("FinDePartida", fromPauseMenu);
	}
	IEnumerator FinDePartida(bool fromPauseMenu)
	{
		if (!fromPauseMenu)
		{
			yield return new WaitForSeconds(3f);
			//Game Over Sound
			Grid.sfx.PlaySoundByIndex(9, this.transform.position);
		}
		yield return new WaitForSecondsRealtime(3f);
		Paused = false;
		UIController.Instance.LoadGameMenu();
	}

	public virtual void ComenzarNivel()
	{
		Grid.game.SetNivelComenzado(true);
		Grid.game.SetNivelTerminado(false);
		enemigosPorAparecer = Mathf.RoundToInt(Grid.game.GetLevel() / 1.5f + 3);
		maxEnemigosALaVez = Mathf.RoundToInt(Grid.game.GetLevel() / 10f + 4);
		SpawnPropsForThisLevel(Mathf.RoundToInt(Grid.game.GetLevel() * 1.5f + 5));
		UpdateGUIValues();
		StartCoroutine("ShowLevelNumber");
		//Debug.LogError("sounds at GameController> : " + Grid.sfx.GameSounds.Length);

		// sonido intro
		Grid.sfx.PlaySoundByIndex(10, Vector3.zero);

		enemigosVivosEsteNivel = 0;
		// do start game functions
		Invoke("SpawnPlayer", 6.0f);
		//InvokeRepeating("SpawnEnemy", 1, 10);
		StartCoroutine("SpawnEnemies");
	}

	public virtual void TerminarNivel()
	{
		//Debug.LogError("TerminarNivel");
		Grid.game.SetNivelTerminado(true);
		Grid.game.SetNivelComenzado(false);
	}


	public virtual void SpawnPlayer()
	{
		// the player needs to be spawned
		//GameObject player = Instantiate(playerGO, new Vector3(-45, 0.0f, -5), Quaternion.Euler(0, 135, 0));
		playerGO.SetActive(true);
	}


	public virtual void SpawnPropsForThisLevel(int level = 1)
	{
		Transform propParent = GameObject.Find("Props").transform;
		//Debug.LogError("PROPS level: " + level);
		for (int i = 0; i < level; i++)
		{
			int index = Random.Range(0, propsSpawnPoints.Count);
			GameObject prop = Instantiate(propPrefabs[Random.Range(0, propPrefabs.Length)], propsSpawnPoints[index].transform.position + Vector3.up, Quaternion.Euler(0, Random.Range(0, 180), 0));
			prop.transform.parent = propParent;
			propsSpawnPoints.RemoveAt(index);
		}
	}

	IEnumerator SpawnEnemies()
	{
		yield return new WaitForSeconds(8f);

		while (enemigosPorAparecer > 0)
		{
			if (enemigosVivosEsteNivel > maxEnemigosALaVez - 1)
			{
				yield return new WaitForSeconds(3f);
				continue;
			}
			yield return new WaitForSeconds(6f);
			GameObject enemy = null;
			EnemyController enemyScript;

			// enemigos normales
			if (enemigosPorAparecer > 1)
			{
				enemy = Instantiate(talibanGO, new Vector3(0.5f, 0.0f, -30.5f), Quaternion.Euler(0, -45, 0));
				EnemyController talibanScript = enemy.GetComponent<EnemyController>();

				// Random color for enemy car
				MeshRenderer enemyMesh = enemy.gameObject.GetComponentInChildren<MeshRenderer>();
				Material[] sharedMaterialsCopy = enemy.gameObject.GetComponentInChildren<MeshRenderer>().materials;
				Material matTemp = enemyMesh.materials[1];
				matTemp.SetColor("_BaseColor", enemyColors[Random.Range(0, enemyColors.Length)]);
				enemyMesh.materials = sharedMaterialsCopy;

				// Random Decals
				talibanScript.decalBack.material.mainTexture = enemyDecals[Random.Range(0, enemyDecals.Length - 1)];
				talibanScript.decalFront.material.mainTexture = enemyDecals[Random.Range(0, enemyDecals.Length - 1)];

				talibanScript.ApplyDestination();
			}
			// ultimo enemigo, BOSS
			else if (enemigosPorAparecer > 0)
			{
				enemy = Instantiate(talibanGO, new Vector3(0.5f, 0.0f, -30.5f), Quaternion.Euler(0, -45, 0));
				enemy.gameObject.transform.name = enemy.gameObject.transform.name + "BOSS";
				EnemyController talibanScript = enemy.GetComponent<EnemyController>();
				yield return new WaitForSeconds(0.5f);

				// Black color for boss car
				MeshRenderer enemyMesh = enemy.gameObject.GetComponentInChildren<MeshRenderer>();
				Material[] sharedMaterialsCopy = enemy.gameObject.GetComponentInChildren<MeshRenderer>().materials;
				Material matTemp = enemyMesh.materials[1];
				matTemp.SetColor("_BaseColor", Color.black);
				enemyMesh.materials = sharedMaterialsCopy;

				// Taliban flag Decal
				talibanScript.decalBack.material.mainTexture = enemyDecals[enemyDecals.Length - 1];
				talibanScript.decalFront.material.mainTexture = enemyDecals[enemyDecals.Length - 1];

				talibanScript.ApplyDestination();
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

	IEnumerator ShowLevelNumber()
	{
		UIController.Instance.ShowLevelNumber("NIVEL " + Grid.game.GetLevel());
		yield return new WaitForSeconds(3.2f);
		UIController.Instance.ShowLevelNumber("");
	}


	public virtual void SpawnBoss()
	{
		// the player needs to be spawned
		GameObject player = Instantiate(talibanGO, new Vector3(-45, 0.0f, -5), Quaternion.Euler(0, 135, 0));
		//playerGO.SetActive(true);
	}

	public virtual void PlayerLostLife()
	{
		Grid.playerStats.ReduceLives(1);
		// deal with player life lost (update U.I. etc.)
		UpdateGUIValues();
	}

	public void Explode(Vector3 aPosition)
	{
		// instantiate an explosion at the position passed into this function
		Instantiate(explosionPrefab, aPosition, Quaternion.identity);
	}
	public virtual void EnemyDestroyed(bool bossKilled)
	{
		enemigosVivosEsteNivel--;

		// deal with enemy destroyed
		if ((enemigosVivosEsteNivel == 0 && enemigosPorAparecer == 0) || bossKilled)
		{
			StartCoroutine("EndOfLevelVictory");
		}
	}

	IEnumerator EndOfLevelVictory()
	{
		Grid.game.SetPartidaComenzada(true);
		Grid.game.SetPartidaTerminada(false);
		Grid.game.AddLevel(1);
		TerminarNivel();

		yield return new WaitForSeconds(1.0f);
		//mission clear Sound
		Grid.sfx.PlaySoundByIndex(3, this.transform.position);
		mainVCam.SetActive(false);
		winVCam.SetActive(true);
		yield return new WaitForSeconds(5.0f);
		SceneManager.LoadScene("2_GameScene");
	}

	public virtual void AlumnasCapturadas()
	{
		Grid.playerStats.SetIsFinished(true);
		Grid.sfx.PlaySoundByIndex(14, this.transform.position);
		Debug.LogError("ALUMNAS CAPTURADAS !!!!!!!!!!!!!!!!");
		mainVCam.SetActive(false);
		alumnasVCam.SetActive(true);
		TerminarPartida();
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

	void UpdateGUIValues()
	{
		UIController.Instance.UpdateEnemiesToSpawn(enemigosPorAparecer);
		UIController.Instance.UpdateHealthP1();
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
			return jugandoNivel;
		}
		set
		{
			// set paused 
			jugandoNivel = value;
		}
	}
}

