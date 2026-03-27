using UnityEngine;
using UnityEngine.SceneManagement;
public class LevelLoading : MonoBehaviour
{
 public void LoadScene(string sceneName)
    {
        SceneManager.LoadSceneAsync(sceneName);
    }
}
