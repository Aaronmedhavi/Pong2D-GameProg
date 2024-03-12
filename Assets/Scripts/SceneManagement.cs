using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneManagement : MonoBehaviour
{
    // Change scene by using the scene name (MenuScene) / (MainScene)
    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    // Exit game
    public void QuitApp()
    {
        Application.Quit();
    }
}
