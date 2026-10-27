using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameTypeButtonController : MonoBehaviour
{
    private SGOGameType GameType;
    [SerializeField] TMP_Text Name;
    [SerializeField] Image Image;

    public void Initialize(SGOGameType gameType)
    {
        GameType = gameType;
        if (Name != null)
            Name.text = GameType.Name;
        if (Image != null && GameType.Image != null)
            Image.sprite = GameType.Image.sprite;
    }
    public void OnButtonClick()
    {
        if (GameType != null && GameType.Scene != null)
        {
            if (SceneManager.Instance != null)
            {
                SceneManager.Instance.LoadSceneGeneric(GameType.Scene, GameType.RequiredPrefabs);
            }
            else
            {
                Debug.LogError("SceneManager instance is not available.");
            }
        }
        else
        {
            Debug.LogError("GameType or Scene is not assigned.");
        }
    }
}
