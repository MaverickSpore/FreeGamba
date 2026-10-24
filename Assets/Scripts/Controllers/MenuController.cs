using UnityEngine;

public class MenuController : MonoBehaviour
{
    public void MainMenuPressPlay()
    {
        if (SceneManager.Instance != null)
        {
            SceneManager.Instance.LoadGameChoiceScene();
        }
        else
        {
            Debug.LogError("SceneManager instance is not available.");
        }
    }
    public void MainMenuPressQuit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
