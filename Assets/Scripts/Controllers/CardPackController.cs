using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CardPackController : MonoBehaviour
{
    private SGOCardPack CardPack;
    [SerializeField] private Image CardPackImage;
    [SerializeField] private SGOCardPack DefaultCardPack;

    Dictionary<CardRarities, int> TimesSpawned;
    public void Initialize(SGOCardPack cardPack)
    {
        CardPack = cardPack;
        if (CardPackImage != null && CardPack.Image != null)
        {
            CardPackImage.sprite = CardPack.Image.sprite;
        }
        else
        {
            Debug.LogWarning("CardPackImage or CardPack.Image is not assigned.");
        }
        TimesSpawned = new Dictionary<CardRarities, int>();

    }
    // input action for opening a card pack
    private void OnOpenCardPack(InputAction.CallbackContext context) => OpenCardPack();
    private void OpenCardPack()
    {
        if (CardPack == null)
        {
            CardPack = DefaultCardPack;
        }

        for (int i = 0; i < CardPack.CardCount; i++)
        {
            SGOCard randomCard = GetRandomCardFromPack();
            if (randomCard != null && OpenPacksManager.Instance != null)
            {
                TimesSpawned[randomCard.CardRarity] = TimesSpawned.ContainsKey(randomCard.CardRarity) ? TimesSpawned[randomCard.CardRarity] + 1 : 1;


                OpenPacksManager.Instance.SpawnCard(randomCard);
            }
        }

        // Sort the dictionary by rarity for better readability
        var sortedTimesSpawned = new List<KeyValuePair<CardRarities, int>>(TimesSpawned);
        sortedTimesSpawned.Sort((x, y) => x.Key.CompareTo(y.Key));
        foreach (var rarity in sortedTimesSpawned)
        {
            Debug.Log($"Spawned {rarity.Value} cards of rarity {rarity.Key}, which is {rarity.Value / (float)CardPack.CardCount * 100:F2}% of the total.");
        }


        Destroy(gameObject); // Destroy the card pack after opening
    }

    private SGOCard GetRandomCardFromPack()
    {
        bool luckSucceeded = false;
        CardRarities rarity = CardRarities.Legendary; // start here and go down
        SGOCard cardData = CardPack.Cards[0];
        float chance = MavRandom.GeoRange((float)CardRarities.MIN, (float)CardRarities.MAX, 3.0f);
        while (!luckSucceeded)
        {
            List<SGOCard> cards = CardPack.Cards.FindAll(card => card.CardRarity == rarity);

            if (cards.Count <= 0)
            {
                rarity--;
                if (rarity < 0)
                {
                    rarity = 0;
                    luckSucceeded = true;
                }
                continue;
            }

            cardData = cards[Random.Range(0, cards.Count)];
            if (chance >= (int)rarity)
            {
                luckSucceeded = true;
            }
            else
            {
                rarity--;
                if (rarity < 0)
                {
                    rarity = 0;
                    luckSucceeded = true;
                }
            }
        }
        return cardData;
    }

    public void OnPackClicked()
    {
        OpenCardPack();
    }
    private void Start()
    {
        if (GameManager.Instance != null && GameManager.Instance.PlayerInput != null)
        {
            GameManager.Instance.PlayerInput.CardPacks.OpenPack.performed += OnOpenCardPack;
        }
        else
        {
            Debug.LogError("GameManager instance or PlayerInput is not available.");
        }
    }



    private void OnDestroy()
    {
        if (GameManager.Instance != null && GameManager.Instance.PlayerInput != null)
        {
            GameManager.Instance.PlayerInput.CardPacks.OpenPack.performed -= OnOpenCardPack;
        }
    }
}
