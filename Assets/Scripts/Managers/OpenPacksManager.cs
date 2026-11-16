using System.Collections.Generic;
using UnityEngine;

public class OpenPacksManager : MonoBehaviour
{
    public static OpenPacksManager Instance;
    [SerializeField] GameObject PackPrefab;
    [SerializeField] private GameObject CardPrefab;
    [SerializeField] SGOCardPackLibrary Packs;
    [SerializeField] SGOCardPack DevPack;
    [SerializeField] GameObject OpenPackCanvas;
    [SerializeField] GameObject CardAnimationCanvas;
    [SerializeField] GameObject PackMenu;
    [SerializeField] GameObject PickPackPrefab;
    [SerializeField] GameObject BinderCanvas;

    public void PressBack() => BackToMainMenu();
    public void PressBinder() => OpenBinder();
    private GameObject CurrentSpawnedPack;
    private List<GameObject> SpawnedCards = new List<GameObject>();
    internal bool CardsSpawning = false;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        if (DevPack != null)
        {
            //SpawnPack(DevPack);
        }
        SpawnPackMenu();
    }
    private void SpawnPackMenu()
    {
        if (PackMenu != null && PickPackPrefab != null)
        {
            foreach (var pack in Packs.CardPacks)
            {
                GameObject packObject = Instantiate(PickPackPrefab, PackMenu.transform.position, Quaternion.identity);
                packObject.transform.SetParent(PackMenu.transform);
                packObject.transform.localScale = Vector3.one;
                
                if (packObject.TryGetComponent<PickPackController>(out var controller))
                {
                    controller.Initialize(pack);
                }
                
            }    
        }
    }
    public void SpawnPack(SGOCardPack pack)
    {
        if (CardsSpawning)
        {
            Debug.LogWarning("Cannot spawn a new pack while cards are still spawning.");
            return;
        }
        if (SpawnedCards.Count > 0)
        {
            foreach (var card in SpawnedCards)
            {
                Destroy(card);
            }
            SpawnedCards.Clear();
        }
        if (CurrentSpawnedPack != null)
        {
            Destroy(CurrentSpawnedPack);
        }
        if (PackMenu != null && PackPrefab != null)
        {
            GameObject packObject = Instantiate(PackPrefab, OpenPackCanvas.transform.position, Quaternion.identity);
            packObject.transform.SetParent(OpenPackCanvas.transform);
            packObject.transform.localScale = Vector3.one;
            if (packObject.TryGetComponent<CardPackController>(out var controller))
            {
                controller.Initialize(pack);
            }
            CurrentSpawnedPack = packObject;
        }
    }
    public GameObject SpawnCard(SGOCard card)
    {
        if (CardPrefab != null)
        {
            GameObject cardObject = Instantiate(CardPrefab, CardAnimationCanvas.transform.position, Quaternion.identity);
            cardObject.transform.SetParent(CardAnimationCanvas.transform);
            cardObject.transform.localScale = Vector3.one;
            cardObject.name = card.name;
            if (cardObject.TryGetComponent<CardController>(out var controller))
            {
                controller.Initialize(card);
            }
            if (card.CardSpawnEffect != null)
            {
                AnimationClip spawnEffect = card.CardSpawnEffect;
                if (cardObject.TryGetComponent<Animator>(out var animator))
                {
                    // Enable the animation component and play the spawn effect
                    animator.enabled = true;
                    animator.Play(spawnEffect.name);

                }
            }
            SpawnedCards.Add(cardObject);
            return cardObject;
        }
        return null;
    }
    public void CardAnimationEnded(GameObject cardObject)
    {
        if (SpawnedCards.Contains(cardObject))
        {
            cardObject.transform.SetParent(OpenPackCanvas.transform);
        }
        if (BinderCanvas != null && cardObject.TryGetComponent<CardController>(out var controller))
        {
            BinderController binderController = BinderCanvas.GetComponent<BinderController>();
            if (binderController != null)
            {
                binderController.AddCard(controller.GetCard());
            }
        }
    }

    private void BackToMainMenu()
    {
        if (SceneManager.Instance != null)
        {
            SceneManager.Instance.LoadGameChoiceScene();
        }
    }
    private void OpenBinder()
    {
        if (CardsSpawning)
        {
            Debug.LogWarning("Cannot open binder while cards are still spawning.");
            return;
        }
        if (BinderCanvas != null)
        {
            BinderCanvas.SetActive(true);
        }
    }
}
