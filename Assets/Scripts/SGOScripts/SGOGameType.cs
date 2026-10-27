using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Game Type", menuName = "Game/Game Type")]
public class SGOGameType : ScriptableObject
{
    [SerializeField] string gameTypeName;
    [SerializeField] Image gameTypeImage;
    [SerializeField] SceneAsset scene;
    [SerializeField] List<GameObject> requiredPrefabs;
    public string Name => gameTypeName;
    public Image Image => gameTypeImage;
    public SceneAsset Scene => scene;
    public List<GameObject> RequiredPrefabs => requiredPrefabs;
}
