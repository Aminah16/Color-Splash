using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;
using System.Collections;

public class RewardedAds : MonoBehaviour, IUnityAdsShowListener
{
    [SerializeField] Button _showAdButton;
    [SerializeField] string _androidAdUnitId = "Rewarded_Android";
    [SerializeField] string _iOSAdUnitId = "Rewarded_iOS";
    string _adUnitId = null; // This will remain null for unsupported platforms
    bool _adCompleted = false;

    void Awake()
    {
        // Get the Ad Unit ID for the current platform:
#if UNITY_IOS
        _adUnitId = _iOSAdUnitId;
#elif UNITY_ANDROID
        _adUnitId = _androidAdUnitId;
#endif

    }

    // Implement a method to execute when the user clicks the button:
    public void ShowAd()
    {
        // If the ad has already been completed, do not play it again
        if (_adCompleted)
            return;

        // Load and show the ad:
        Advertisement.Show(_adUnitId, this);
    }

    // Implement the Show Listener's OnUnityAdsShowComplete callback method to determine if the user gets a reward:
    public void OnUnityAdsShowComplete(string adUnitId, UnityAdsShowCompletionState showCompletionState)
    {
        if (adUnitId.Equals(_adUnitId) && showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED))
        {
            Debug.Log("Unity Ads Rewarded Ad Completed");
            _adCompleted = true;
            StartCoroutine(ShowCloseButtonAfterDelay(30f)); // Show close button after 30 seconds
        }
    }

    IEnumerator ShowCloseButtonAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        // Enable the close button for users to click:
        _showAdButton.gameObject.SetActive(true);
    }

    // Implement Load and Show Listener error callbacks:
    public void OnUnityAdsShowFailure(string adUnitId, UnityAdsShowError error, string message)
    {
        Debug.Log($"Error showing Ad Unit {adUnitId}: {error.ToString()} - {message}");
        // Use the error details to determine whether to try to load another ad.
    }

    // Implement the method required by the IUnityAdsShowListener interface (can be left empty):
    public void OnUnityAdsShowStart(string adUnitId)
    {
        // This method is invoked when the ad starts showing.
        // You can leave it empty if you don't need to perform any specific actions.
    }

    // Implement the method required by the IUnityAdsShowListener interface (can be left empty):
    public void OnUnityAdsShowClick(string adUnitId)
    {
        // This method is invoked when the ad is clicked.
        // You can leave it empty if you don't need to perform any specific actions.
    }

    void OnDestroy()
    {
        // Clean up the button listeners:
        _showAdButton.onClick.RemoveAllListeners();
    }
}
