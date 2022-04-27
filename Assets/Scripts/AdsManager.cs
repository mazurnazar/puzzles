using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdsManager : MonoBehaviour
{
#if UNITY_ANDROID
    private string gameid = "4714887";
#endif
    // Start is called before the first frame update
    void Start()
    {
        Advertisement.Initialize(gameid, true);
    }
    public void ShowInterstitialAd()
    {
        Advertisement.Show("Interstitial_Android");
    }

}
