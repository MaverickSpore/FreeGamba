using System.Collections;
using UnityEditor;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    public static SceneManager Instance;
    [SerializeField] private SceneAsset MainMenuScene;
    [SerializeField] private SceneAsset GameChoiceScene;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadMainMenuScene()
    {
        StartCoroutine(LoadMainMenu());
    }
    private IEnumerator LoadMainMenu()
    {
        if (MainMenuScene != null)
        {
            AsyncOperation asyncLoad = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(MainMenuScene.name);
            while (!asyncLoad.isDone)
            {
                yield return null;
            }
            if (GameManager.Instance != null)
            {
                GameManager.Instance.LoadRequiredPrefabsMainMenu();
            }
        }
        else
        {
            Debug.LogError("MainMenuScene is not assigned in the SceneManager.");
        }
    }
    public void LoadGameChoiceScene()
    {
        StartCoroutine(LoadGameChoice());
    }
    private IEnumerator LoadGameChoice()
    {
        if (GameChoiceScene != null)
        {
            AsyncOperation asyncLoad = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(GameChoiceScene.name);
            while (!asyncLoad.isDone)
            {
                yield return null;
            }
            if (GameManager.Instance != null)
            {
                GameManager.Instance.LoadRequiredPrefabsGeneric();
            }
        }
        else
        {
            Debug.LogError("GameChoiceScene is not assigned in the SceneManager.");
        }
    }
}
