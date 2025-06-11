using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds;
using GoogleMobileAds.Api;

public class ADInitializer : MonoBehaviour
{
    public void Start()
    {
         //Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize((InitializationStatus initStatus) =>
        {
             //This callback is called once the MobileAds SDK is initialized.
        });
    }
}
