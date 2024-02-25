using System.Threading.Tasks;
using TMPro;
using Unity.Services.Authentication;
using UnityEngine;

public class LoginSceneManagerScript : Singleton<LoginSceneManagerScript>
{
    [SerializeField] TMP_InputField usernameField;
    [SerializeField] TMP_InputField passwordField;

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

    public void HandleSignup()
    {
        Task signupTask = new Task(SignUpWithFieldCredentials);
        signupTask.RunSynchronously();
    }

    public void HandleSignIn()
    {
        Task signinTask = new Task(SignInWithFieldCredentials);
        signinTask.RunSynchronously(TaskScheduler.FromCurrentSynchronizationContext());
    }

    async void SignUpWithFieldCredentials()
    {
        string username = usernameField.text;
        string password = passwordField.text;
        Debug.Log("Username: " + username + "\n" + "Password: " + password);
        Debug.Log("Signing Up");
        await _serviceManagerScript.SignUpWithUsernamePasswordAsync(username, password)
            .ContinueWith(_ =>
            {
                UpdateInspectorVars();
                if (AuthenticationService.Instance.IsSignedIn)
                {
                    Debug.Log("Moving Scene to Camera");
                    GameManagerScript.MoveToScene("SCamera");
                }
            });
    }

    async void SignInWithFieldCredentials()
    {
        string username = usernameField.text;
        string password = passwordField.text;
        Debug.Log("Username: " + username + "\n" + "Password: " + password);
        Debug.Log("Signing In");
        await _serviceManagerScript.SignInWithUsernamePasswordAsync(username, password)
            .ContinueWith(_ =>
            {
                UpdateInspectorVars();
                if (AuthenticationService.Instance.IsSignedIn)
                {
                    Debug.Log("Moving Scene to Camera");
                    GameManagerScript.MoveToScene("SCamera");
                }
            });
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