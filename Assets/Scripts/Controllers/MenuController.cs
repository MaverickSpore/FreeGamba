using UnityEngine;

public class MenuController : MonoBehaviour
{
    public void PressAllowAds() => EnableAds();
    public void PressDenyAds() => DisableAds();
    public void MainPressPlay() => MainMenuPlay();
    public void MainPressSettings() => MainMenuSettings();
    public void MainPressQuit() => MainMenuQuit();
    public void MainMenuPlay()
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
    public void MainMenuSettings()
    {
        if (SceneManager.Instance != null)
        {
            SceneManager.Instance.LoadSettingsScene();
        }
        else
        {
            Debug.LogError("SceneManager instance is not available.");
        }
    }
    public void MainMenuQuit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
    private void EnableAds()
    {
        if (AdsManager.Instance != null)
        {
            AdsManager.Instance.EnableAds();
        }
        else
        {
            Debug.LogError("AdsManager instance is not available.");
        }
    }
    private void DisableAds()
    {
        if (AdsManager.Instance != null)
        {
            AdsManager.Instance.DisableAds();
        }
        else
        {
            Debug.LogError("AdsManager instance is not available.");
        }
    }
}
