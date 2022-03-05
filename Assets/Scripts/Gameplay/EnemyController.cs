using UnityEngine;
using UnityEngine.AI;

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
    public Material matFocoVerde;
    public Material matFocoGris;
    public bool isDead;
    public bool dest;

    public GameObject shooter;
    public GameObject bullet;
    public float bulletForce = 20;

    void Start()
    {
        health = 3;
        agent = GetComponent<NavMeshAgent>();
        agent.speed = 7;
        isDead = false;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent.SetDestination(player.transform.position);
        animatorcontroller = GetComponentInChildren<Animator>();
        foco1.material = matFocoVerde;
        foco2.material = matFocoVerde;
        foco3.material = matFocoVerde;
        InvokeRepeating("ShootBullet", 5, 5);
    }

    public void ApplyDestination()
    {
        if (!isDead)
        {
            if (controlpoints.Length == 0)
            {
                controlpoints = GameObject.Find("Controlpoints").GetComponentsInChildren<Transform>();
            }
            agent.SetDestination(controlpoints[Random.Range(0, controlpoints.Length)].position);
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
        // choques fuertes
        if (collision.relativeVelocity.magnitude > 10)
        {
            // Debug.LogError("lives " + lives + " collision: " + collision.collider.name);
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
                Invoke("ResetAnimationHit", 0.2f);
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
        if (other.tag == "Bullet")
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
        } else if (other.transform.name == "Barrera Alumnas" && !isDead && !Grid.game.GetNivelTerminado())
        {
            agent.destination = this.transform.position;
            GameController.Instance.AlumnasCapturadas();
        }
    }

    void UpdateEnemyHealth(int howManyLost = 1)
    {
        health = health - howManyLost;

        if (health == 2)
        {
            foco1.material = matFocoVerde;
            foco2.material = matFocoVerde;
            foco3.material = matFocoGris;
        }
        else if (health == 1)
        {
            foco1.material = matFocoVerde;
            foco2.material = matFocoGris;
            foco3.material = matFocoGris;
        }
        if (health <= 0)
        {
            foco1.material = matFocoGris;
            foco2.material = matFocoGris;
            foco3.material = matFocoGris;
        }
    }

    void Dies()
    {
        if (!isDead)
        {
            isDead = true;
            agent.destination = this.transform.position;
            animatorcontroller.SetBool("muere", true);
            Grid.sfx.PlaySoundByIndex(4, this.transform.position);
            GameController.Instance.EnemyDestroyed();
            Destroy(this.gameObject, 2.5f);
        }
    }
}
