//using System.Collections;
using UnityEngine;

public class ItemInGame : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        transform.Rotate(0, 120 * Time.deltaTime, 0);
    }
}
