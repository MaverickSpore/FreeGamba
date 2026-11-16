using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CardPackController : MonoBehaviour
{
    private SGOCardPack CardPack;
    [SerializeField] private Image CardPackImage;
    [SerializeField] private SGOCardPack DefaultCardPack;

    float WaitTime = 0.5f; // Time to wait between spawning cards

    public void Initialize(SGOCardPack cardPack)
    {
        CardPack = cardPack;
        if (CardPackImage != null && CardPack.Image != null)
        {
            CardPackImage.sprite = CardPack.Image;
        }
        else
        {
            Debug.LogWarning("CardPackImage or CardPack.Image is not assigned.");
        }

    }
    // input action for opening a card pack
    private void OnOpenCardPack(InputAction.CallbackContext context) => OpenCardPack();
    private void OpenCardPack()
    {
        if (CardPack == null)
        {
            CardPack = DefaultCardPack;
        }
        gameObject.transform.SetParent(null);
        StartCoroutine(SpawnCards());
    }
    private IEnumerator SpawnCards()
    {
        if (gameObject.TryGetComponent<Button>(out var button))
        {
            button.interactable = false; // Disable the button to prevent multiple clicks
        }
        if (OpenPacksManager.Instance != null)
        {
            OpenPacksManager.Instance.CardsSpawning = true;
            for (int i = 0; i < CardPack.CardCount; i++)
            {
                GameObject newCard = null;
                SGOCard randomCard = GetRandomCardFromPack();
                if (randomCard != null)
                {
                    if (randomCard.CardSpawnEffect != null)
                    {
                        WaitTime = randomCard.CardSpawnEffect.length; // Set WaitTime to the length of the spawn effect
                    }
                    else
                    {
                        WaitTime = 0.5f; // Default wait time if no spawn effect is assigned
                    }
                    newCard = OpenPacksManager.Instance.SpawnCard(randomCard);
                }
                while (WaitTime > 0)
                {
                    WaitTime -= Time.deltaTime;
                    // Wait for the next frame
                    yield return null;
                }
                if (newCard != null)
                {
                    OpenPacksManager.Instance.CardAnimationEnded(newCard);
                }
            }
            OpenPacksManager.Instance.CardsSpawning = false;
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
