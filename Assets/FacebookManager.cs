using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Facebook.Unity;

public class FacebookManager : MonoBehaviour
{
    [SerializeField] private TMP_InputField facebookUsername;
    [SerializeField] private RawImage profilePicture;
    [SerializeField] private Button loginButton;
    [SerializeField] private Button logoutButton;
    
    private void Awake()
    {
        loginButton.onClick.AddListener(Facebook_Login);
        //logoutButton.onClick.AddListener(Facebook_LogOut);
        
        FB.Init(() => GetUserData(FB.IsLoggedIn), OnHiddenUnity);

        if (!FB.IsInitialized)
        {
            FB.Init(() =>
            {
                if (FB.IsInitialized)
                    FB.ActivateApp();
                else
                    Debug.Log("Couldn't initialize");
            },
            OnHiddenUnity);
        }
        else
            FB.ActivateApp();
    }

    private void OnHiddenUnity(bool isGameShown)
    {
        Time.timeScale = isGameShown ? 1 : 0;
    }

    private void GetUserData(bool isLoggedIn)
    {
        if (isLoggedIn)
        {
            FB.API("/me?fields=first_name", HttpMethod.GET, DisplayUsername);
            FB.API("/me/picture?type=square&height=128&width=128", HttpMethod.GET, DisplayProfilePicture);
        }
        else
        {
            Debug.Log("Not logged in");
        }
    }

    private void DisplayUsername(IResult result)
    {
        if (result.Error == null)
        {
            var username = "" + result.ResultDictionary["first_name"];
            if (facebookUsername != null) facebookUsername.text = username;
            facebookUsername.text = username;
        }
        else
        {
            Debug.Log(result.Error);
        }
    }

    private void DisplayProfilePicture(IGraphResult result)
    {
        if (result.Texture != null)
        {
            profilePicture.texture = result.Texture;
        }
        else
        {
            Debug.Log(result.Error);
        }
    }
    
    private void Facebook_Login() 
    {
        var permissions = new List<string> { "gaming_profile" };

        FB.LogInWithReadPermissions(permissions, AuthCallBack);
    }

    private void AuthCallBack(IResult result)
    {
        if (FB.IsLoggedIn)
        {
            GetUserData(FB.IsLoggedIn);

            var accessToken = AccessToken.CurrentAccessToken;

            Debug.Log(accessToken.UserId);

            foreach (var perm in accessToken.Permissions)
            {
                Debug.Log(perm);
            }
        }
        else
        {
            Debug.Log("Failed to log in");
        }

    }
    
    private void Facebook_Logout()
    {
        StartCoroutine(Logout());
    }

    private IEnumerator Logout()
    {
        FB.LogOut();
        while (FB.IsLoggedIn)
        {
            yield return null;
        }
        
        if (facebookUsername != null) facebookUsername.text = "";
        if (profilePicture != null) profilePicture.texture = null;
    }
}