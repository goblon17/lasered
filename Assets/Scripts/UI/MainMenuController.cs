using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Elympics;
using Elympics.Models.Authentication;

public class MainMenuController : MonoBehaviour
{
    [SerializeField]
    private GameObject mainPanel;
    [SerializeField]
    private LoadingScreen loadingScreen;
    [SerializeField]
    private GameObject failedPanel;

    [SerializeField]
    private string authenticationLoadingMessage;
    [SerializeField]
    private string authenticationFailedLoadingMessage;
    [SerializeField]
    private string matchmakingLoadingMessage;

    [SerializeField]
    private float failedPanelShowTime;

    private void Start()
    {
        failedPanel.SetActive(false);
        if (ElympicsLobbyClient.Instance.IsAuthenticated)
        {
            loadingScreen.Hide();
        }
        else
        {
            loadingScreen.Show(authenticationLoadingMessage);
            SubscribeAuthenticationCallbacks();
        }
        SubscribeMatchmakerCallback();
    }

    private void OnDestroy()
    {
        UnsubscribeMatchmakerCallback();
    }

    private void SubscribeAuthenticationCallbacks()
    {
        ElympicsLobbyClient.Instance.AuthenticationSucceeded += OnAuthenticationSucceeded;
        ElympicsLobbyClient.Instance.AuthenticationFailed += OnAuthenticationFailed;
    }

    private void UnsubscribeAuthenticationCallbacks()
    {
        ElympicsLobbyClient.Instance.AuthenticationSucceeded -= OnAuthenticationSucceeded;
        ElympicsLobbyClient.Instance.AuthenticationFailed -= OnAuthenticationFailed;
    }

    private void SubscribeMatchmakerCallback()
    {
        ElympicsLobbyClient.Instance.Matchmaker.MatchmakingStarted += OnMatchmakingStarted;
        ElympicsLobbyClient.Instance.Matchmaker.MatchmakingFailed += OnMatchmakingFailed;
    }

    private void UnsubscribeMatchmakerCallback()
    {
        ElympicsLobbyClient.Instance.Matchmaker.MatchmakingStarted -= OnMatchmakingStarted;
        ElympicsLobbyClient.Instance.Matchmaker.MatchmakingFailed -= OnMatchmakingFailed;
    }

    public void PlayTutorial()
    {
        ElympicsConfig.Load().SwitchGame(1);
        ElympicsLobbyClient.Instance.PlayOffline();
    }

    public void PlayOnline()
    {
        ElympicsConfig.Load().SwitchGame(0);
        ElympicsLobbyClient.Instance.PlayOnlineInRegion(null, null, null, "Normal");
    }

    public void Quit()
    {
        Application.Quit();
    }

    private void OnAuthenticationSucceeded(AuthData args)
    {
        UnsubscribeAuthenticationCallbacks();
        loadingScreen.Hide();
    }

    private void OnAuthenticationFailed(string error)
    {
        loadingScreen.Show(authenticationFailedLoadingMessage);
        ElympicsLobbyClient.Instance.AuthenticateWith(AuthType.ClientSecret);
    }

    private void OnMatchmakingStarted()
    {
        loadingScreen.Show(matchmakingLoadingMessage);
    }

    private void OnMatchmakingFailed((string Error, System.Guid MatchId) args)
    {
        loadingScreen.Hide();
        failedPanel.SetActive(true);
        this.CallDelayed(failedPanelShowTime, () =>
        {
            failedPanel.SetActive(false);
        });
    }
}
