using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Granada : MonoBehaviour
{
    public GameObject particle1;
    public GameObject particle2;
    public SphereCollider radio;
    void Start()
    {
        StartCoroutine("GranadaLanzada");
    }

    IEnumerator GranadaLanzada()
    {
        yield return new WaitForSeconds(0.75f);
        particle1.SetActive(true);
        particle2.SetActive(true);
        GetComponent<MeshRenderer>().enabled = false;
        radio.enabled = true;
    }
}
