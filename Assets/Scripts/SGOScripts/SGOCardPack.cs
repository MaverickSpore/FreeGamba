using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Card Pack", menuName = "Cards/Card Pack")]
public class SGOCardPack : ScriptableObject
{
    [SerializeField] List<SGOCard> cards;
    [SerializeField] Image image;
    [SerializeField] int cardCount;
    public List<SGOCard> Cards => cards;
    public Image Image => image;
    public int CardCount => cardCount;
}
