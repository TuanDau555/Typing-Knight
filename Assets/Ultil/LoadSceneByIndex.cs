using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// This script is attach to the button that has the able to change the scene
/// </summary>
public class LoadSceneByIndex : MonoBehaviour
{
    #region Load Scene

    public void LoadScene(int index)
    {
        int totalSceneInBuild = SceneManager.sceneCountInBuildSettings;
        if(index < 0 && index > totalSceneInBuild)
        {
            Debug.LogError($"Scene {index} is not available for {gameObject.name} in the game or you forget to add to scene list");
            return;    
        }

        Debug.Log(totalSceneInBuild);
        SceneManager.LoadScene(index);
    }
    
    #endregion
}