using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class EnemyController : MonoBehaviour
{
    public Transform[] controlpoints;
    public NavMeshAgent agent;
    private Transform player;
    private Animator animatorcontroller;
    public int health;
    public bool exitGarage = false;
    public MeshRenderer foco1;
    public MeshRenderer foco2;
    public MeshRenderer foco3;
    public MeshRenderer carBody;
    public SkinnedMeshRenderer talibanMesh;
    public Material matFocoVerde;
    //public Material matFocoGris;
    public Material matCocheBodyDissolve;
    public Material matCocheAccesDissolve;
    public Material matTalibanDissolve;
    public bool isDead;
    public bool dest;
    public bool isDissolved;
    public float t;
    public MeshRenderer decalFront;
    public MeshRenderer decalBack;

    public GameObject shooter;
    public GameObject bullet;
    public float bulletForce = 20;

    void Start()
    {
        health = 3;
        agent = GetComponent<NavMeshAgent>();
        agent.speed = 7;
        isDead = false;
        isDissolved = false;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent.SetDestination(player.transform.position);
        animatorcontroller = GetComponentInChildren<Animator>();
        foco1.material.SetColor("_EmissionColor", Color.green);
        foco2.material.SetColor("_EmissionColor", Color.green);
        foco3.material.SetColor("_EmissionColor", Color.green);
        if (!this.transform.name.Contains("BOSS"))
        {
            InvokeRepeating("ShootBullet", 5, 5);
            health = 3;
        }
        else
        {
            InvokeRepeating("ShootBullet", 4, 3.5f);
            health = 5;
        }
    }

    public void ApplyDestination()
    {
        if (!isDead)
        {
            if (controlpoints.Length == 0)
            {
                controlpoints = GameObject.Find("Controlpoints").GetComponentsInChildren<Transform>();
            }
            if (!this.transform.name.Contains("BOSS"))
            {
                agent.SetDestination(controlpoints[Random.Range(0, controlpoints.Length)].position);
                agent.speed = 4;
                agent.acceleration = 1;
            }
            else
            {
                agent.SetDestination(controlpoints[Random.Range(0, 4)].position);
                agent.speed = 5;
                agent.acceleration = 2;
            }
        }
    }

    void ShootBullet()
    {
        if (!isDead)
        {
            // disparo bala
            GameObject tmpBullet;
            tmpBullet = Instantiate(bullet, shooter.transform.position, Quaternion.identity);
            tmpBullet.transform.up = shooter.transform.forward;
            tmpBullet.GetComponent<Rigidbody>().AddForce(transform.forward *
                                                        bulletForce,
                                                        ForceMode.Impulse);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Granada")
        {

        }
        else if (collision.relativeVelocity.magnitude > 10)
        // choques fuertes
        {
            //Debug.LogError("OnCollisionEnter: " + collision.collider.name);
            if (collision.relativeVelocity.magnitude > 20)
            {
                //sonido delicious
                Grid.sfx.PlaySoundByIndex(13, this.transform.position);
                UpdateEnemyHealth(3);
            }
            else if (collision.relativeVelocity.magnitude > 15)
            {
                UpdateEnemyHealth(2);
            }
            else
            {
                UpdateEnemyHealth(1);
            }

            //Debug.LogError("lives " + lives);
            if (health > 0)
            {
                //sonido daño
                Grid.sfx.PlaySoundByIndex(5, this.transform.position);
                animatorcontroller.SetBool("duele", true);
                Invoke("ResetAnimationHit", 1.5f);
            }
            else
            {
                Dies();
            }
        }
        else if (collision.relativeVelocity.magnitude > 7 && collision.transform.tag == "Player" && health > 0)
        {
            // Sonido risa
            Grid.sfx.PlaySoundByIndex(7, this.transform.position);
        }

    }

    void ResetAnimationHit()
    {
        animatorcontroller.SetBool("duele", false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Granada" && !animatorcontroller.GetBool("duele"))
        {
            UpdateEnemyHealth(2);

            if (health > 0)
            {
                //sonido daño
                Grid.sfx.PlaySoundByIndex(5, this.transform.position);
                animatorcontroller.SetBool("duele", true);
                Invoke("ResetAnimationHit", 1.5f);
            }
            else
            {
                Dies();
            }
        }
        else if (other.tag == "Bullet")
        {
            // colision con la bala
            UpdateEnemyHealth(1);

            if (health > 0)
            {
                // sonido chilla
                Grid.sfx.PlaySoundByIndex(6, this.transform.position);
                animatorcontroller.SetBool("duele", true);
                Invoke("ResetAnimationHit", 0.2f);
            }
            else
            {
                Dies();
            }
        }
        else if (other.transform.name == "ExitGarageTrigger")
        {
            exitGarage = true;
            agent.speed = 4;
            InvokeRepeating("ApplyDestination", 0, 4.0f);
        }
        else if (other.transform.name == "Barrera Alumnas" && !isDead && !Grid.game.GetNivelTerminado())
        {
            agent.destination = this.transform.position;
            GameController.Instance.AlumnasCapturadas();
        }
    }

    public void UpdateEnemyHealth(int howManyLost = 1)
    {
        health = health - howManyLost;

        if (health == 2)
        {
            foco1.material.SetColor("_EmissionColor", Color.green);
            foco2.material.SetColor("_EmissionColor", Color.green);
            foco3.material.SetColor("_EmissionColor", Color.gray);
        }
        else if (health == 1)
        {
            foco1.material.SetColor("_EmissionColor", Color.green);
            foco2.material.SetColor("_EmissionColor", Color.gray);
            foco3.material.SetColor("_EmissionColor", Color.gray);
        }
        if (health <= 0)
        {
            foco1.material.SetColor("_EmissionColor", Color.gray);
            foco2.material.SetColor("_EmissionColor", Color.gray);
            foco3.material.SetColor("_EmissionColor", Color.gray);
        }
    }

    public void Dies()
    {
        if (!isDead)
        {
            isDead = true;
            agent.destination = this.transform.position;
            animatorcontroller.SetBool("muere", true);
            Grid.sfx.PlaySoundByIndex(4, this.transform.position);
            GameController.Instance.EnemyDestroyed(this.transform.name.Contains("BOSS"));
            if (!isDissolved)
            {
                isDissolved = true;
                StartCoroutine("DissolvedAnim");
            }
            Destroy(this.gameObject, 2.5f);
        }
    }

    IEnumerator DissolvedAnim()
    {
        yield return new WaitForSeconds(1.5f);

        foco1.enabled = false;
        foco2.enabled = false;
        foco3.enabled = false;
        GetComponent<BoxCollider>().enabled = false;
        Material[] carMaterialsCopy = carBody.materials;
        Material modifiedDissolved = matCocheBodyDissolve;
        modifiedDissolved.SetColor("_BaseColor", carMaterialsCopy[1].color);
        carMaterialsCopy[0] = matCocheAccesDissolve;
        carMaterialsCopy[1] = modifiedDissolved;
        carBody.materials = carMaterialsCopy;
        talibanMesh.material = matTalibanDissolve;

        while (t < 0.7f)
        {
            carBody.materials[0].SetFloat("_Vector", Mathf.Lerp(0, 1, t));
            carBody.materials[1].SetFloat("_Vector", Mathf.Lerp(0, 1, t));
            matTalibanDissolve.SetFloat("_Vector", Mathf.Lerp(0, 1, t));
            t += 0.8f * Time.deltaTime;
            yield return null;
        }
    }

}
