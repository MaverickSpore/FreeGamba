using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardController : MonoBehaviour
{
    private SGOCard Card;

    [SerializeField] private TMP_Text Name;
    [SerializeField] private TMP_Text Description;
    [SerializeField] private TMP_Text Cost;
    [SerializeField] private Image Image;
    [SerializeField] private TMP_Text ID;


    public void Initialize(SGOCard card)
    {
        Card = card;
        if (Name != null)
            Name.text = Card.CardName;
        if (Description != null)
            Description.text = Card.CardDescription;
        if (Image != null && Card.CardImage != null)
            Image.sprite = Card.CardImage;
        if (Cost != null)
            Cost.text = Card.CardCost.ToString();
        if (ID != null)
            ID.text = Card.CardID;
    }
}
