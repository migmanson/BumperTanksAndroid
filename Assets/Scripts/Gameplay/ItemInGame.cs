using UnityEngine.AI;
using UnityEngine;
using System.Collections;

public class ItemInGame : MonoBehaviour
{
	void Update()
	{
		transform.Rotate(0, 120 * Time.deltaTime, 0);
	}

	void OnTriggerEnter(Collider other)
	{
		GetComponent<MeshRenderer>().enabled = false;
		GetComponent<BoxCollider>().enabled = false;

		switch (gameObject.name)
		{
			case "Rayo":
				{
					//Debug.LogError("Agarre RAYO");
					MiCocheControl refScript = other.GetComponent<MiCocheControl>();
					StartCoroutine("PUpInvencible", refScript);
					break;
				}
			case "Reloj":
				{
					//Debug.LogError("Agarre RELOJ");
					StartCoroutine("PUpReloj");
					break;
				}
			case "Granada":
				{
					//Debug.LogError("Agarre GRANADA");
					MiCocheControl refScript = other.GetComponent<MiCocheControl>();					
					Grid.sfx.PlaySoundByIndex(16, this.transform.position);
					refScript.GetComponent<MiCocheControl>().isPupGranada = true;
					UIController.Instance.TogglePowerUp(3, true);					
					Destroy(gameObject, 1);
					break;
				}
			case "Llave":
				{
					//Debug.LogError("Agarre LLAVE");
					Grid.sfx.PlaySoundByIndex(15, this.transform.position);
					other.GetComponent<MiCocheControl>().RestoreFullHealth();
					Destroy(gameObject);
					break;
				}
		}
	}


	IEnumerator PUpInvencible(MiCocheControl refScript)
	{
		Grid.sfx.PlaySoundByIndex(16, this.transform.position);
		refScript.isInvincible = true;
		MeshRenderer playerMesh = refScript.gameObject.GetComponentInChildren<MeshRenderer>();
		Material matTemp = playerMesh.material;
		playerMesh.material = refScript.matInvencible;
		UIController.Instance.TogglePowerUp(1, true);
		yield return new WaitForSeconds(15);
		refScript.isInvincible = false;
		playerMesh.material = matTemp;
		UIController.Instance.TogglePowerUp(1, false);
		Destroy(gameObject);
	}

	IEnumerator PUpReloj()
	{
		// musica de tiempo MB3
		GameObject[] enemigos = GameObject.FindGameObjectsWithTag("CocheTaliban");

		foreach (GameObject coche in enemigos)
		{
			coche.GetComponent<NavMeshAgent>().speed = 0.25f;
		}

		UIController.Instance.TogglePowerUp(2, true);
		yield return new WaitForSeconds(15);
		enemigos = GameObject.FindGameObjectsWithTag("CocheTaliban");

		foreach (GameObject coche in enemigos)
		{
			coche.GetComponent<NavMeshAgent>().speed = 4.0f;
		}

		UIController.Instance.TogglePowerUp(2, false);
		Destroy(gameObject);
	}
}
