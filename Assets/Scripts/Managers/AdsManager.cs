using Unity.Services.LevelPlay;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdsManager : MonoBehaviour
{
    public static AdsManager Instance;


    [Header("LevelPlay Keys")]
    [SerializeField] private string androidAppKey;
    [SerializeField] private string iosAppKey;

    [Header("Banner IDs")]
    [SerializeField] private string androidBannerID;
    [SerializeField] private string iosBannerID;

    private LevelPlayBannerAd bannerAd;
    private bool adsEnabled;
    private bool sdkInitialized;

    private const string AdsPreference = "AdsEnabled";

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

        adsEnabled = PlayerPrefs.GetInt(AdsPreference, 1) == 1;
    }

    private void Start()
    {
        InitializeAds();
    }

    private void InitializeAds()
    {
        string appKey = GetAppKey();

        if (string.IsNullOrEmpty(appKey))
        {
            Debug.Log("Ads: No App Key configured.");
            return;
        }

        LevelPlay.OnInitSuccess += OnInitSuccess;
        LevelPlay.OnInitFailed += OnInitFailed;

        LevelPlay.Init(appKey);
    }

    private void OnInitSuccess(LevelPlayConfiguration config)
    {
        sdkInitialized = true;
        Debug.Log("Ads: LevelPlay initialized.");

        if (adsEnabled)
            CreateBanner();
    }

    private void OnInitFailed(LevelPlayInitError error)
    {
        Debug.LogError($"Ads initialization failed: {error}");
    }

    private void CreateBanner()
    {
        if (!sdkInitialized || bannerAd != null)
            return;

        string bannerID = GetBannerID();

        if (string.IsNullOrEmpty(bannerID))
            return;

        var config = new LevelPlayBannerAd.Config.Builder()
            .SetSize(LevelPlayAdSize.BANNER)
            .SetPosition(LevelPlayBannerPosition.TopCenter)
            .SetDisplayOnLoad(false)
            .SetRespectSafeArea(true)
            .Build();

        bannerAd = new LevelPlayBannerAd(bannerID, config);

        bannerAd.OnAdLoaded += OnBannerLoaded;
        bannerAd.OnAdLoadFailed += OnBannerFailed;

        bannerAd.LoadAd();
    }

    private void OnBannerLoaded(LevelPlayAdInfo info)
    {
        Debug.Log("Ads: Banner loaded.");

        if (adsEnabled)
            bannerAd?.ShowAd();
    }

    private void OnBannerFailed(LevelPlayAdError error)
    {
        Debug.LogWarning($"Banner failed: {error}");
    }

    public void EnableAds()
    {
        adsEnabled = true;
        PlayerPrefs.SetInt(AdsPreference, 1);
        PlayerPrefs.Save();

        if (!sdkInitialized)
            return;

        if (bannerAd == null)
            CreateBanner();
        else
            bannerAd.ShowAd();
    }

    public void DisableAds()
    {
        adsEnabled = false;
        PlayerPrefs.SetInt(AdsPreference, 0);
        PlayerPrefs.Save();

        bannerAd?.HideAd();
        bannerAd?.PauseAutoRefresh();
    }

    private string GetAppKey()
    {
#if UNITY_ANDROID
        return androidAppKey;
#elif UNITY_IOS
        return iosAppKey;
#else
        return string.Empty;
#endif
    }

    private string GetBannerID()
    {
#if UNITY_ANDROID
        return androidBannerID;
#elif UNITY_IOS
        return iosBannerID;
#else
        return string.Empty;
#endif
    }

    private void OnDestroy()
    {
        if (Instance != this)
            return;

        LevelPlay.OnInitSuccess -= OnInitSuccess;
        LevelPlay.OnInitFailed -= OnInitFailed;

        if (bannerAd != null)
        {
            bannerAd.OnAdLoaded -= OnBannerLoaded;
            bannerAd.OnAdLoadFailed -= OnBannerFailed;
            bannerAd.DestroyAd();
            bannerAd = null;
        }

        Instance = null;
    }
}
