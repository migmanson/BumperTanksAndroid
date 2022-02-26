using UnityEngine.SceneManagement;
using UnityEngine;

public class StartNewGame : MonoBehaviour
{
    // Start is called before the first frame update
    public void StartGame()
    {        
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }
}
