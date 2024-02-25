using System;
using System.Threading.Tasks;
using TMPro;
using Unity.Services.Authentication;
using UnityEngine;

public class SettingsSceneManagerScript : Singleton<SettingsSceneManagerScript>
{
    [SerializeField] TMP_InputField usernameField;
    [SerializeField] TMP_InputField oldPasswordField;
    [SerializeField] TMP_InputField newPasswordField;

    [Header("DEBUG")] [Header("Buttons")] [SerializeField]
    private bool updateInspectorVarsBtn;

    [SerializeField] private bool signOutBtn;
    [Header("Values")] [SerializeField] bool isAuthorized;
    [SerializeField] string playerName;
    [SerializeField] string playerId;
    [SerializeField] string accessToken;
    ServiceManagerScript _serviceManagerScript;

    void Awake()
    {
        _serviceManagerScript = ServiceManagerScript.Instance;
        UpdateFields();
    }

    async void UpdateFields()
    {
        await AuthenticationService.Instance.GetPlayerNameAsync()
            .ContinueWith(antecedent => { usernameField.text = antecedent.Result; });
    }

    public async Task UpdateName()
    {
        if (usernameField.text != AuthenticationService.Instance.PlayerName)
        {
            await AuthenticationService.Instance.UpdatePlayerNameAsync(usernameField.text);
        }
    }

    public async Task UpdatePassword()
    {
        if (oldPasswordField.text == newPasswordField.text)
        {
            Debug.LogError("Old password same as new password.");
            return;
        }

        await AuthenticationService.Instance.UpdatePasswordAsync(oldPasswordField.text, newPasswordField.text);
    }

    public void HandlePlayerUpdates()
    {
        Task updateNameTask = UpdateName();
        Task updatePasswordTask = UpdatePassword();

        Task.WaitAll(updateNameTask, updatePasswordTask);
        UpdateFields();
    }

    public void HandleSignout()
    {
        try
        {
            AuthenticationService.Instance.SignOut();
        }
        catch (AuthenticationException e)
        {
            Console.WriteLine(e);
        }
        
        if (!AuthenticationService.Instance.IsSignedIn)
            GameManagerScript.MoveToScene("Login");
    }
}