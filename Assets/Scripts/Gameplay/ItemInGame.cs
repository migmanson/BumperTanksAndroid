using UnityEngine.AI;
using UnityEngine;
using System.Collections;

public class ItemInGame : MonoBehaviour
{
    GameController gameController;
    void Start()
    {
        gameController = GameObject.Find("GAME CONTROLLER").GetComponent<GameController>();
    }
    void Update()
    {
        transform.Rotate(0, 120 * Time.deltaTime, 0);
    }

    void OnTriggerEnter(Collider other)
    {
        MiCocheControl refScript = other.GetComponent<MiCocheControl>();

        if (!refScript.isDead && !Grid.playerStats.GetIsFinished())
        {
            GetComponent<MeshRenderer>().enabled = false;
            GetComponent<BoxCollider>().enabled = false;

            switch (gameObject.name)
            {
                case "Rayo":
                    {
                        //Debug.LogError("Agarre RAYO");
                        refScript = other.GetComponent<MiCocheControl>();
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
                        refScript = other.GetComponent<MiCocheControl>();
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
    }


    IEnumerator PUpInvencible(MiCocheControl refScript)
    {
        Grid.sfx.PlaySoundByIndex(16, this.transform.position);
        refScript.isInvincible = true;
        MeshRenderer playerMesh = refScript.gameObject.GetComponentInChildren<MeshRenderer>();
        //Debug.LogError("playerMesh")
        Material[] sharedMaterialsCopy = playerMesh.sharedMaterials;
        Material matTemp = playerMesh.sharedMaterials[1];
        sharedMaterialsCopy[1] = refScript.matInvencible;
        playerMesh.sharedMaterials = sharedMaterialsCopy;
        UIController.Instance.TogglePowerUp(1, true);
        MusicController.Instance.SwitchClip(1, 0, 50);

        yield return new WaitForSeconds(15);

        refScript.isInvincible = false;
        sharedMaterialsCopy[1] = matTemp;
        playerMesh.sharedMaterials = sharedMaterialsCopy;
        UIController.Instance.TogglePowerUp(1, false);
        if (!Grid.playerStats.GetIsFinished())
        {
            if (gameController.enemigosPorAparecer == 0)
                MusicController.Instance.SwitchClip(4, 0, 50);
            else
                MusicController.Instance.SwitchClip(0, 0, 50);
        }
        Destroy(gameObject);
    }

    IEnumerator PUpReloj()
    {
        // musica de tiempo MB3
        GameObject[] enemigos = GameObject.FindGameObjectsWithTag("CocheTaliban");

        foreach (GameObject coche in enemigos)
        {           
            coche.GetComponent<NavMeshAgent>().speed = 0.05f;
        }

        UIController.Instance.TogglePowerUp(2, true);

        MusicController.Instance.SwitchClip(3, 0, 50);

        yield return new WaitForSeconds(15);

        enemigos = GameObject.FindGameObjectsWithTag("CocheTaliban");

        foreach (GameObject coche in enemigos)
        {
            if (!coche.transform.name.Contains("BOSS"))
            {
                coche.GetComponent<NavMeshAgent>().speed = 4.0f;
            }
            else
            {
                coche.GetComponent<NavMeshAgent>().speed = 5.0f;
            }
        }

        UIController.Instance.TogglePowerUp(2, false);
        if (!Grid.playerStats.GetIsFinished())
        {
            if (gameController.enemigosPorAparecer == 0)
                MusicController.Instance.SwitchClip(4, 0, 50);
            else
                MusicController.Instance.SwitchClip(0, 0, 50);
        }

        Destroy(gameObject);
    }
}
