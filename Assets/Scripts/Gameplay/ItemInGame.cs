//using System.Collections;
using UnityEngine;

public class ItemInGame : MonoBehaviour
{
    void Update()
    {
        transform.Rotate(0, 120 * Time.deltaTime, 0);
    }
}
