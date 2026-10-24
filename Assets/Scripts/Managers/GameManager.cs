using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] List<GameObject> RequiredPrefabsGeneric;
    [SerializeField] List<GameObject> RequiredPrefabsMainMenu;

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
    public void LoadRequiredPrefabsMainMenu()
    {
        LoadRequiredPrefabsGeneric();
        LoadRequirePrefabs(RequiredPrefabsMainMenu);
    }
}
