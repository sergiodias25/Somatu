﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Assets.SimpleLocalization.Scripts;
using Assets.Scripts.AnalyticsEvent;
using System;

public class ShareButton : MonoBehaviour
{
    public Button shareButton;
    private bool isFocus = false;
    private bool isProcessing = false;

    void Start()
    {
        shareButton.onClick.AddListener(ShareText);
    }

    void OnApplicationFocus(bool focus)
    {
        isFocus = focus;
    }

    private void ShareText()
    {
#if UNITY_ANDROID

        if (!isProcessing)
        {
            StartCoroutine(ShareTextInAnroid());
        }

#else
        Debug.Log("No sharing set up for this platform.");
#endif
    }

#if UNITY_ANDROID

    public IEnumerator ShareTextInAnroid()
    {
        var shareSubject = "Play SOMATU on your phone";

        var shareMessage = LocalizationManager.Localize(
            "popup-challenge-share-text",
            Timer.FormatTimeForText(FindObjectOfType<Timer>().LastElapsedTime),
            FindObjectOfType<GameManager>()._timesSolvedText + 1,
            "https://play.google.com/store/apps/"
        );

        Debug.Log(shareMessage);
        ChallengeShared.SendAnalyticsEvent(
            Math.Round(FindObjectOfType<Timer>().LastElapsedTime, 2),
            FindObjectOfType<GameManager>()._timesSolvedText + 1
        );

        isProcessing = true;

        if (!Application.isEditor)
        {
            //Create intent for action send
            AndroidJavaClass intentClass = new AndroidJavaClass("android.content.Intent");
            AndroidJavaObject intentObject = new AndroidJavaObject("android.content.Intent");
            intentObject.Call<AndroidJavaObject>(
                "setAction",
                intentClass.GetStatic<string>("ACTION_SEND")
            );
            //put text and subject extra
            intentObject.Call<AndroidJavaObject>("setType", "text/plain");

            intentObject.Call<AndroidJavaObject>(
                "putExtra",
                intentClass.GetStatic<string>("EXTRA_SUBJECT"),
                shareSubject
            );
            intentObject.Call<AndroidJavaObject>(
                "putExtra",
                intentClass.GetStatic<string>("EXTRA_TEXT"),
                shareMessage
            );

            //call createChooser method of activity class
            AndroidJavaClass unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");

            AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject>(
                "currentActivity"
            );
            AndroidJavaObject chooser = intentClass.CallStatic<AndroidJavaObject>(
                "createChooser",
                intentObject,
                "Share your high score"
            );
            currentActivity.Call("startActivity", chooser);
        }

        yield return new WaitUntil(() => isFocus);
        isProcessing = false;
    }

#endif
}
