using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class DDOL : MonoBehaviour
{
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        //Debug.LogError("sounds at DDOL> : " + Grid.sfx.GameSounds.Length);
        Application.targetFrameRate = 60;
        StartCoroutine("KeepAccelerometerAlive");
    }

    IEnumerator KeepAccelerometerAlive()
    {
        yield return Input.acceleration.sqrMagnitude;
        yield return new WaitForSeconds(0.1f);
        yield return Input.acceleration.sqrMagnitude;
        yield return new WaitForSeconds(0.1f);
        SceneManager.LoadScene("1_Main Menu", LoadSceneMode.Single);
    }
}
