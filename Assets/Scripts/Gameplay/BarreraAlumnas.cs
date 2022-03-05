using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarreraAlumnas : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Bullet")
        {
            GameController.Instance.AlumnasCapturadas();
        }
    }
}
