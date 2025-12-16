using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    
    void Start()
    {
        SceneManager.LoadSceneAsync("UIScene", LoadSceneMode.Additive);
    }

   
}
