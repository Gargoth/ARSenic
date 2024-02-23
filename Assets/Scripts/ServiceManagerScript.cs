using System;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;

public class ServiceManagerScript : Singleton<ServiceManagerScript>
{
    async void Awake()
    {
        try
        {
            await UnityServices.InitializeAsync();
            Debug.Log("Unity Services SDK Initialized");
            SetupEvents();
            Debug.Log("Authentication Service Event Handlers Setup Complete");
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
 
    }
    
    void SetupEvents() {
        AuthenticationService.Instance.SignedIn += () => {
            // Shows how to get a playerID
            Debug.Log($"PlayerID: {AuthenticationService.Instance.PlayerId}");

            // Shows how to get an access token
            Debug.Log($"Access Token: {AuthenticationService.Instance.AccessToken}");

        };

        AuthenticationService.Instance.SignInFailed += (err) => {
            Debug.LogError(err);
        };

        AuthenticationService.Instance.SignedOut += () => {
            Debug.Log("Player signed out.");
        };

        AuthenticationService.Instance.Expired += () =>
        {
            Debug.Log("Player session could not be refreshed and expired.");
        };
    }

    
    public async Task SignUpWithUsernamePasswordAsync(string username, string password)
    {
        try
        {
            await AuthenticationService.Instance.SignUpWithUsernamePasswordAsync(username, password);
            Debug.Log("SignUp is successful.");
        }
        catch (AuthenticationException ex)
        {
            // Compare error code to AuthenticationErrorCodes
            // Notify the player with the proper error message
            Debug.LogException(ex);
        }
        catch (RequestFailedException ex)
        {
            // Compare error code to CommonErrorCodes
            // Notify the player with the proper error message
            Debug.LogException(ex);
        }
    }

    public async Task SignInWithUsernamePasswordAsync(string username, string password)
    {
        try
        {
            await AuthenticationService.Instance.SignInWithUsernamePasswordAsync(username, password);
            Debug.Log("SignIn is successful.");
        }
        catch (AuthenticationException ex)
        {
            // Compare error code to AuthenticationErrorCodes
            // Notify the player with the proper error message
            Debug.LogException(ex);
        }
        catch (RequestFailedException ex)
        {
            // Compare error code to CommonErrorCodes
            // Notify the player with the proper error message
            Debug.LogException(ex);
        }
    }

    public void DebugAuth()
    {
        IAuthenticationService authInstance = AuthenticationService.Instance;
        Debug.Log(
            "isAuthorized: " + authInstance.IsAuthorized + "\n" +
            "playerID: " + authInstance.PlayerId + "\n" +
            "playerInfo: " + authInstance.PlayerInfo + "\n" +
            "playerName: " + authInstance.PlayerName + "\n"
            );
    }
}
