using TMPro;
using Unity.Services.Authentication;
using UnityEngine;

public class LoginSceneManagerScript : Singleton<LoginSceneManagerScript>
{
    [SerializeField] TMP_InputField usernameField;
    [SerializeField] TMP_InputField passwordField;

    [Header("DEBUG")]
    [Header("Buttons")]
    [SerializeField] private bool updateInspectorVarsBtn;
    [SerializeField] private bool signOutBtn;
    [Header("Values")]
    [SerializeField] bool isAuthorized;
    [SerializeField] string playerName;
    [SerializeField] string playerId;
    [SerializeField] string accessToken;
    ServiceManagerScript _serviceManagerScript;

    void Awake()
    {
        _serviceManagerScript = ServiceManagerScript.Instance;
    }

    void Update()
    {
        if (updateInspectorVarsBtn)
        {
            UpdateInspectorVars();
            updateInspectorVarsBtn = false;
        }

        if (signOutBtn)
        {
            AuthenticationService.Instance.SignOut();
            signOutBtn = false;
        }
    }

    public void SignUpWithFieldCredentials()
    {
        string username = usernameField.text;
        string password = passwordField.text;
        Debug.Log("Username: " + username + "\n" + "Password: " + password);
        Debug.Log("Signing Up");
        _serviceManagerScript.SignUpWithUsernamePasswordAsync(username, password)
            .ContinueWith(_ => UpdateInspectorVars());
    }

    public void SignInWithFieldCredentials()
    {
        string username = usernameField.text;
        string password = passwordField.text;
        Debug.Log("Username: " + username + "\n" + "Password: " + password);
        Debug.Log("Signing In");
        _serviceManagerScript.SignInWithUsernamePasswordAsync(username, password)
            .ContinueWith(_ => UpdateInspectorVars());
    }

    void UpdateInspectorVars()
    {
        isAuthorized = AuthenticationService.Instance.IsAuthorized;
        playerId = AuthenticationService.Instance.PlayerId;
        accessToken = AuthenticationService.Instance.AccessToken;
        AuthenticationService.Instance.GetPlayerNameAsync()
            .ContinueWith(antecedent => playerName = antecedent.Result);
    }
}
