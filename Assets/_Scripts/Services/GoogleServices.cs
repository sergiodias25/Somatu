using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine;

public static class GoogleServices
{
    public static void Authenticate()
    {
        PlayGamesPlatform.Instance.Authenticate(ProcessAuthentication);
    }

    public static void ProcessAuthentication(SignInStatus status)
    {
        if (status == SignInStatus.Success)
        {
            // Continue with Play Games Services
            Social.Active.localUser.Authenticate(success =>
            {
                if (success)
                {
                    UnityEngine.Object.FindObjectOfType<GameManager>().IsLoggedInToGoogle = true;
                }
            });
        }
        else
        {
            // Disable your integration with Play Games Services or show a login button
            // to ask users to authenticate. Clicking it should call
            // PlayGamesPlatform.Instance.ManuallyAuthenticate(ProcessAuthentication).
            Debug.Log("SignIn has failed.");
        }
    }

    public static void UnlockAchievement(string achievementID)
    {
        Social.ReportProgress(achievementID, 100f, null);
    }

    public static void IncrementAchievement(string achievementID, int quantity)
    {
        PlayGamesPlatform.Instance.IncrementAchievement(achievementID, quantity, null);
    }
}
