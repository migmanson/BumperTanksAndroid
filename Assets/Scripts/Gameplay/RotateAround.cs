using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAround : MonoBehaviour
{

    public GameObject orbitAround;
    public float speed = 20f;

    void Update()
    {
        transform.RotateAround(orbitAround.transform.position, Vector3.up, -speed * Time.deltaTime);
    }
}
