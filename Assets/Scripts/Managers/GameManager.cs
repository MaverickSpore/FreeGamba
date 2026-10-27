using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public PlayerInput PlayerInput;

    [SerializeField] List<GameObject> RequiredPrefabsGeneric;
    [SerializeField] List<GameObject> RequiredPrefabsMainMenu;
    [SerializeField] List<GameObject> RequiredPrefabsGameSelection;

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
        PlayerInput = new PlayerInput();
    }
    private void OnEnable()
    {
        PlayerInput.Enable();
    }
    private void Start()
    {
        LoadRequiredPrefabsMainMenu();
    }

    private void LoadRequirePrefabs(List<GameObject> prefabs)
    {
        foreach (var prefab in prefabs)
        {
            if (prefab != null && !GameObject.Find(prefab.name))
            {
                GameObject temp = Instantiate(prefab);
                temp.name = prefab.name;
            }
        }
    }
    public void LoadRequiredPrefabsGeneric()
    {
        LoadRequirePrefabs(RequiredPrefabsGeneric);
    }
    public void LoadRequiredPrefabsGeneric(List<GameObject> prefabs)
    {
        LoadRequirePrefabs(RequiredPrefabsGeneric);
        LoadRequirePrefabs(prefabs);
    }
    public void LoadRequiredPrefabsMainMenu()
    {
        LoadRequiredPrefabsGeneric();
        LoadRequirePrefabs(RequiredPrefabsMainMenu);
    }
    public void LoadRequiredPrefabsGameSelection()
    {
        LoadRequiredPrefabsGeneric();
        LoadRequirePrefabs(RequiredPrefabsGameSelection);
    }
}
