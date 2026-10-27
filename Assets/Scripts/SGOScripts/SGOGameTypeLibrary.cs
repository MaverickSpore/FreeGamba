using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Game Type Library", menuName = "Game/Game Type Library")]
public class SGOGameTypeLibrary : ScriptableObject
{
    [SerializeField] List<SGOGameType> gameTypes;
    public List<SGOGameType> Types => gameTypes;
}
