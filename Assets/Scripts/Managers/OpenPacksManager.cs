using UnityEngine;

public class OpenPacksManager : MonoBehaviour
{
    public static OpenPacksManager Instance;
    [SerializeField] GameObject PackPrefab;
    [SerializeField] private GameObject CardPrefab;
    [SerializeField] SGOCardPack DevPack;
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
            SpawnPack(DevPack);
        }
    }
    public void SpawnPack(SGOCardPack pack)
    {
        if (PackPrefab != null)
        {
            GameObject packObject = Instantiate(PackPrefab, transform.position, Quaternion.identity);
            packObject.transform.SetParent(transform);
            packObject.transform.localScale = Vector3.one;
            if (packObject.TryGetComponent<CardPackController>(out var controller))
            {
                controller.Initialize(pack);
            }
        }
    }
    public void SpawnCard(SGOCard card)
    {
        if (CardPrefab != null)
        {
            GameObject cardObject = Instantiate(CardPrefab, transform.position, Quaternion.identity);
            cardObject.transform.SetParent(transform);
            cardObject.transform.localScale = Vector3.one;
            cardObject.name = card.name;
            if (cardObject.TryGetComponent<CardController>(out var controller))
            {
                controller.Initialize(card);
            }
        }
    }
}
