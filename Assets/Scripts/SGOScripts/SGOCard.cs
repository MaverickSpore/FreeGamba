using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Cards/Card")]
public class SGOCard : ScriptableObject
{
    [SerializeField] string cardName;
    [SerializeField] string cardDescription;
    [SerializeField] Sprite cardImage;
    [SerializeField] int cardCost;
    [SerializeField] string cardID;
    [SerializeField] CardRarities cardRarity;
    public string CardName => cardName;
    public string CardDescription => cardDescription;
    public Sprite CardImage => cardImage;
    public int CardCost => cardCost;
    public string CardID => cardID;
    public CardRarities CardRarity => cardRarity;
}
