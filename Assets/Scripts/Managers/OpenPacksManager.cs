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
    [SerializeField] GameObject PackMenu;
    [SerializeField] GameObject PickPackPrefab;

    private GameObject CurrentSpawnedPack;
    private List<GameObject> SpawnedCards = new List<GameObject>();
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
    public void SpawnCard(SGOCard card)
    {
        if (CardPrefab != null)
        {
            GameObject cardObject = Instantiate(CardPrefab, OpenPackCanvas.transform.position, Quaternion.identity);
            cardObject.transform.SetParent(OpenPackCanvas.transform);
            cardObject.transform.localScale = Vector3.one;
            cardObject.name = card.name;
            if (cardObject.TryGetComponent<CardController>(out var controller))
            {
                controller.Initialize(card);
            }
            SpawnedCards.Add(cardObject);
        }
    }
}
