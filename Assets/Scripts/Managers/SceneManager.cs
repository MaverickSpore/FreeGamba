using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    public static SceneManager Instance;
    [SerializeField] private int MainMenu;
    [SerializeField] private int Settings;
    [SerializeField] private int GameChoice;

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
        AsyncOperation asyncLoad = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(MainMenu);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        if (GameManager.Instance != null)
        {
            GameManager.Instance.LoadRequiredPrefabsMainMenu();
        }
    }
    public void LoadSettingsScene()
    {
        StartCoroutine(LoadSettings());
    }
    private IEnumerator LoadSettings()
    {
        AsyncOperation asyncLoad = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(Settings);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        if (GameManager.Instance != null)
        {
            GameManager.Instance.LoadRequiredPrefabsSettings();
        }
    }
    public void LoadGameChoiceScene()
    {
        StartCoroutine(LoadGameSelection());
    }
    private IEnumerator LoadGameSelection()
    {
        AsyncOperation asyncLoad = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(GameChoice);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        if (GameManager.Instance != null)
        {
            GameManager.Instance.LoadRequiredPrefabsGameSelection();
        }
    }
    public void LoadSceneGeneric(int sceneIndex, List<GameObject> requiredPrefabs)
    {
        if (sceneIndex >= 0)
        {
            StartCoroutine(LoadSceneGenericCoroutine(sceneIndex, requiredPrefabs));
        }
        else
        {
            Debug.LogError("SceneAsset is not assigned in the SceneManager.");
        }
    }
    private IEnumerator LoadSceneGenericCoroutine(int sceneIndex, List<GameObject> requiredPrefabs)
    {
        AsyncOperation asyncLoad = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneIndex);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        if (GameManager.Instance != null)
        {
            GameManager.Instance.LoadRequiredPrefabsGeneric(requiredPrefabs);
        }
    }
}
