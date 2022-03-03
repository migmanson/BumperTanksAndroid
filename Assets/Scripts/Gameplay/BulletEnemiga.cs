using UnityEngine;

public class BulletEnemiga : MonoBehaviour
{
    private void Awake()
    {
        Destroy(this.gameObject, 1.0f);
    }

    void OnCollisionEnter(Collision collision)
    {
        Invoke("RemoveTag", 0.1f);
    }

    void RemoveTag()
    {
        this.transform.tag = "Untagged";
        Destroy(GetComponent<CapsuleCollider>());
    }
}
