using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MiCocheControl : MonoBehaviour
{

	public Transform fuerzaR;
	public Transform fuerzaL;
	Rigidbody car_Rigidbody;
	public float multiplier = 35000f;

	public Slider sliderR;
	public Slider sliderL;
	public ParticleSystem chispas;
	bool particlesPlay = false;
	//public TMPro.TextMeshProUGUI colisionText;

	public GameObject shooter;
	public GameObject bullet;
	public float bulletForce = 20;
	public GameObject granadaPrefab;
	public float granadaForce = 2;


	public MeshRenderer foco1;
	public MeshRenderer foco2;
	public MeshRenderer foco3;
	public Material matFocoVerde;
	public Material matFocoGris;

	//shake area
	public float umbralShake = 3.5f;
	public float MinShakeInterval;
	private float timeSinceLastShake;
	public int health;
	public int lives;
	public bool isDead;
	public bool isInvincible;
	public bool isPupGranada;
	private Vector3 startPos;

	private Cinemachine.CinemachineVirtualCamera vcam1;

	void Start()
	{
		lives = Grid.playerStats.GetLives();
		health = 3;
		isDead = false;
		isInvincible = false;
		isPupGranada = false;
		startPos = transform.position;
		car_Rigidbody = GetComponent<Rigidbody>();
		vcam1 = GameObject.FindObjectOfType<Cinemachine.CinemachineVirtualCamera>();
		vcam1.Follow = this.transform;
		vcam1.LookAt = this.transform;
		sliderR = GameObject.Find("HandleR").GetComponent<Slider>();
		sliderL = GameObject.Find("HandleL").GetComponent<Slider>();
		foco1.material = matFocoVerde;
		foco2.material = matFocoVerde;
		foco3.material = matFocoVerde;
		UIController.Instance.UpdateHealthP1();
	}

	public void GameStart()
	{

	}

	void Update()
	{
		// esta primera seccion es necesaria por usar la raiz cuadrada para intensificar los valores de las fuerzas cercanas a 0, y por no poder aplicar raiz a valores negativos
		short signoR = 1;
		short signoL = 1;
		if (sliderR.value < 0)
		{
			signoR = -1;
		}
		if (sliderL.value < 0)
		{
			signoL = -1;
		}

		if (!isDead && !Grid.game.GetNivelTerminado())
		{
			// aplico las fuerzas en los laterales del coche chocador, dependiendo del valor de cada slider en pantalla
			car_Rigidbody.AddForceAtPosition(this.transform.forward * signoR * Mathf.Sqrt(Mathf.Abs(sliderR.value)) * multiplier * Time.deltaTime, fuerzaR.transform.position);
			car_Rigidbody.AddForceAtPosition(this.transform.forward * signoL * Mathf.Sqrt(Mathf.Abs(sliderL.value)) * multiplier * Time.deltaTime, fuerzaL.transform.position);
		}
		// activar las particulas solo si el coche camina
		// calcular velocidad del coche
		Vector3 velCoche = car_Rigidbody.GetPointVelocity(transform.TransformPoint(this.transform.position));

		if (Mathf.Abs(velCoche.x) < 2.0f && Mathf.Abs(velCoche.z) < 2.0f)
		{
			StopChispas();
		}
		else if (!particlesPlay)
		{
			PlayChispas();
			particlesPlay = true;
		}

		//Debug.LogError(" shake !!! ! ! !  " + Input.acceleration.sqrMagnitude + "   " + umbralShake);

		// Shake al telefono, dispara una bala
		if (Input.acceleration.sqrMagnitude >= umbralShake)
		{
			ShootBullet();
		}
	}

	public void ShootBullet()
	{
		if (!isDead && !GameController.Instance.Paused && Time.unscaledTime >= timeSinceLastShake + MinShakeInterval && !Grid.game.GetNivelTerminado())
		{
			if (!isPupGranada)
			{
				timeSinceLastShake = Time.unscaledTime;
				// disparo bala
				GameObject tmpBullet;
				tmpBullet = Instantiate(bullet, shooter.transform.position, Quaternion.identity);
				tmpBullet.transform.up = shooter.transform.forward;
				tmpBullet.GetComponent<Rigidbody>().AddForce(transform.forward *
															bulletForce,
															ForceMode.Impulse);
				Destroy(tmpBullet.gameObject, 3f);

				// efecto patea hacia atras
				car_Rigidbody.AddForce(-transform.forward * bulletForce * 75, ForceMode.Impulse);
			}
			else
			{
				timeSinceLastShake = Time.unscaledTime;
				// lanza granada
				GameObject tmpGranada;
				tmpGranada = Instantiate(granadaPrefab, shooter.transform.position, Quaternion.identity);
				tmpGranada.transform.up = shooter.transform.forward;
				tmpGranada.GetComponent<Rigidbody>().AddForce(transform.forward *
															granadaForce,
															ForceMode.Impulse);
				Grid.sfx.PlaySoundByIndex(17, this.transform.position);
				Destroy(tmpGranada.gameObject, 3f);
			}
		}
	}

	void PlayChispas()
	{
		chispas.Play();
	}

	void StopChispas()
	{
		chispas.Stop();
		particlesPlay = false;
	}

	void OnCollisionEnter(Collision collision)
	{
		if (isInvincible)
		{
			if (collision.transform.tag == "CocheTaliban")
			{
				collision.gameObject.GetComponent<EnemyController>().UpdateEnemyHealth(3);
				collision.gameObject.GetComponent<EnemyController>().Dies();
			}
		}

		//colisionText.text = (int)collision.relativeVelocity.magnitude + "";
		if (collision.relativeVelocity.magnitude > 12 && collision.transform.tag == "Columna")
		{
			UpdateHealth(1);
		}
		if (collision.transform.CompareTag("BulletEnemiga") && !Grid.game.GetNivelTerminado())
		{
			UpdateHealth(1);
		}
	}

	void UpdateHealth(int howManyLost = 1)
	{
		health = health - howManyLost;

		if (health == 2)
		{
			foco1.material = matFocoVerde;
			foco2.material = matFocoVerde;
			foco3.material = matFocoGris;
			Grid.sfx.PlaySoundByIndex(11, this.transform.position);
		}
		else if (health == 1)
		{
			foco1.material = matFocoVerde;
			foco2.material = matFocoGris;
			foco3.material = matFocoGris;
			Grid.sfx.PlaySoundByIndex(11, this.transform.position);
		}
		if (health <= 0)
		{
			foco1.material = matFocoGris;
			foco2.material = matFocoGris;
			foco3.material = matFocoGris;
			PlayerLosesLife();
		}
	}

	public void RestoreFullHealth()
	{
		Grid.playerStats.SetHealth(3);
		health = Grid.playerStats.GetHealth();
		UIController.Instance.UpdateHealthP1();
		foco1.material = matFocoVerde;
		foco2.material = matFocoVerde;
		foco3.material = matFocoVerde;
	}


	void PlayerLosesLife()
	{
		if (!isDead)
		{
			lives--;
			isDead = true;
			Grid.sfx.PlaySoundByIndex(8, this.transform.position);
			GameController.Instance.PlayerLostLife();
			if (lives > 0)
			{
				StartCoroutine("RespawnPlayer");
			}
			else
			{
				GameController.Instance.TerminarPartida();
				UIController.Instance.UpdateHealthP1();
			}
		}
	}

	IEnumerator RespawnPlayer()
	{
		yield return new WaitForSeconds(3);
		UIController.Instance.UpdateHealthP1();
		UIController.Instance.TogglePowerUp(3, false);
		isPupGranada = false;
		health = 3;
		transform.position = startPos;
		foco1.material = matFocoVerde;
		foco2.material = matFocoVerde;
		foco3.material = matFocoVerde;
		yield return new WaitForSeconds(1);
		isDead = false;
	}
}
