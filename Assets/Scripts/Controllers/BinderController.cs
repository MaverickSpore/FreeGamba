using System.Collections.Generic;
using UnityEngine;

public class BinderController : MonoBehaviour
{
    [SerializeField] GameObject CardSection;
    [SerializeField] GameObject CardPrefab;
    private List<SGOCard> CollectedCards = new List<SGOCard>();
    public void PressBack() => CloseBinder();
    public void PressLeft() => PageLeft();
    public void PressRight() => PageRight();

    public void AddCard(SGOCard card)
    {
        if (card == null)
        {
            Debug.LogWarning("Attempted to add a null card to the binder.");
            return;
        }
        if (CollectedCards.Contains(card))
        {
            Debug.LogWarning($"Card {card.CardName} is already in the binder.");
            return;
        }
        if (CardPrefab != null && CardSection != null && CardSection.TryGetComponent<WidgetSwitcher>(out var widgetSwitcher))
        {
            GameObject newCard = Instantiate(CardPrefab, CardSection.transform.position, Quaternion.identity);
            if (newCard.TryGetComponent<CardController>(out var cardController))
            {
                cardController.Initialize(card);
            }
            if (CardSection.transform.childCount <= 0 || IsLastPageFull())
            {
                widgetSwitcher.AddWidget();
            }
            newCard.transform.SetParent(CardSection.transform.GetChild(CardSection.transform.childCount - 1));
            newCard.transform.localScale = Vector3.one;
            CollectedCards.Add(card);
        }
    }
    private bool IsLastPageFull()
    {
        if (CardSection != null)
        {
            Transform lastPage = CardSection.transform.GetChild(CardSection.transform.childCount - 1);
            return lastPage.childCount >= 16; // Assuming each page can hold 16 cards
        }
        return false;
    }


    private void CloseBinder()
    {
        if (gameObject != null)
        {
            gameObject.SetActive(false);
        }
    }
    private void PageLeft()
    {
        if (CardSection!= null && CardSection.TryGetComponent<WidgetSwitcher>(out var widgetSwitcher))
        {
            widgetSwitcher.PreviousWidget();
        }
    }
    private void PageRight()
    {
        if (CardSection != null && CardSection.TryGetComponent<WidgetSwitcher>(out var widgetSwitcher))
        {
            widgetSwitcher.NextWidget();
        }
    }
}
