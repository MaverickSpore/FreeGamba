using UnityEngine;

public class AdsManager : MonoBehaviour
{
    public static AdsManager Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void EnableAds()
    {

    }
    public void DisableAds()
    {

    }
}
