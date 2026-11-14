using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card Pack Library", menuName = "Cards/Card Pack Library")]
public class SGOCardPackLibrary : ScriptableObject
{
    [SerializeField] List<SGOCardPack> cardPacks;
    public List<SGOCardPack> CardPacks => cardPacks;
}
