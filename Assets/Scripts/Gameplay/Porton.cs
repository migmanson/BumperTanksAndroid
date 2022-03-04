using System.Collections;
using UnityEngine;

public class Porton : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        if (collision.relativeVelocity.magnitude > 6)
        {
            // choque FUERTE
            Grid.sfx.PlaySoundByIndex(2, this.transform.position);

            if (collision.transform.tag == "Player" && GameController.Instance.enemigosPorAparecer <= 0)
                StartCoroutine("AbrePorton");
        }
        else if (collision.relativeVelocity.magnitude > 3)
        {
            // choque LEVE
            Grid.sfx.PlaySoundByIndex(1, this.transform.position);
        }
    }


    IEnumerator AbrePorton()
    {
        GetComponentInParent<Animator>().SetTrigger("AbrePorton");
        yield return new WaitForSeconds(3.0f);
        GetComponentInParent<Animator>().SetTrigger("CierraPorton");
    }

}
