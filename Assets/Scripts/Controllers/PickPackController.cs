using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PickPackController : MonoBehaviour
{
    private SGOCardPack Pack;
    [SerializeField] Image Image;
    [SerializeField] TMP_Text Name;
    [SerializeField] TMP_Text CardCount;
    public void Initialize(SGOCardPack pack)
    {
        Pack = pack;
        Image.sprite = pack.Image;
        Name.text = pack.name;
        CardCount.text = "Cards: " + pack.CardCount.ToString();
    }
    public void OnSelect()
    {
        if (OpenPacksManager.Instance != null)
        {
            OpenPacksManager.Instance.SpawnPack(Pack);
        }
    }
}
