using TMPro;
using Unity.Services.Authentication;
using UnityEngine;
using UnityEngine.UI;

namespace DR.AuthenticationSystem
{
    public class UI_Authentication : MyMonobehaviour
    {
        [SerializeField] private Button signInButton;
        [SerializeField] private GameObject signInPopup;
        [SerializeField] private TextMeshProUGUI notifyText;
        [SerializeField] private Button closeButton;

        protected override void Start()
        {
            base.Start();
            SetupEvents();
            signInButton.onClick.AddListener(AuthenticationManager.Instance.StartSignInAsync);
            closeButton.onClick.AddListener((() =>
            {
                gameObject.SetActive(false);
            }));
        }
        
        private void SetupEvents() {
            AuthenticationService.Instance.SignedIn += () => {
                signInPopup.SetActive(true);
                signInButton.gameObject.SetActive(false);
                notifyText.SetText("Sign in successful.");
                // Shows how to get a playerID
                Debug.Log($"PlayerID: {AuthenticationService.Instance.PlayerId}");

                // Shows how to get an access token
                Debug.Log($"Access Token: {AuthenticationService.Instance.AccessToken}");
            };

            AuthenticationService.Instance.SignInFailed += (err) => {
                Debug.LogError(err);
                signInPopup.SetActive(true);
                notifyText.SetText(err.Message);
            };

            AuthenticationService.Instance.SignedOut += () => {
                Debug.Log("Player signed out.");
            };

            AuthenticationService.Instance.Expired += () =>
            {
                Debug.Log("Player session could not be refreshed and expired.");
            };
        }

        protected override void LoadComponents()
        {
            base.LoadComponents();
            LoadSignInButton();
            LoadSignInPopup();
            LoadNotifyText();
            LoadCloseButton();
            signInPopup.SetActive(false);
        }

        #region Load Components

        private void LoadSignInButton()
        {
            if(signInButton != null) return;
            signInButton = transform.Find("SignInUnityAccountButton").GetComponent<Button>();
            Debug.LogWarning(transform.name + ": LoadSignInButton", gameObject);
        }
        private void LoadSignInPopup()
        {
            if(signInPopup != null) return;
            signInPopup = transform.Find("SignInPopup").gameObject;
            Debug.LogWarning(transform.name + ": LoadSignInPopup", gameObject);
        }
        private void LoadNotifyText()
        {
            if(notifyText != null) return;
            notifyText = signInPopup.transform.Find("NotifyText").GetComponent<TextMeshProUGUI>();
            Debug.LogWarning(transform.name + ": LoadNotifyText", gameObject);
        }
        private void LoadCloseButton()
        {
            if(closeButton != null) return;
            closeButton = signInPopup.transform.Find("CloseButton").GetComponent<Button>();
            Debug.LogWarning(transform.name + ": LoadCloseButton", gameObject);
        }

        #endregion
        
    }
}
