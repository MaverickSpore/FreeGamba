using UnityEngine;

public class AdPopupController : MonoBehaviour
{
    public void PressAllowAds() => EnableAds();
    public void PressDenyAds() => DisableAds();
    private void Awake()
    {
        if (PlayerPrefs.GetInt("AdsPopupShown", 0) == 1)
        {
            Destroy(gameObject);
        }
    }
    private void EnableAds()
    {
        if (AdsManager.Instance != null)
        {
            AdsManager.Instance.EnableAds();
        }
        else
        {
            Debug.LogError("AdsManager instance is not available.");
        }
        PlayerPrefs.SetInt("AdsPopupShown", 1);
        Destroy(gameObject);
    }
    private void DisableAds()
    {
        if (AdsManager.Instance != null)
        {
            AdsManager.Instance.DisableAds();
        }
        else
        {
            Debug.LogError("AdsManager instance is not available.");
        }
        PlayerPrefs.SetInt("AdsPopupShown", 1);
        Destroy(gameObject);
    }
}
