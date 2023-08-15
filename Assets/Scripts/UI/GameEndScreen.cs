using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameEndScreen : MonoBehaviour
{
	[SerializeField]
	private TextMeshProUGUI gameWinnerText;
	[SerializeField]
	private CanvasGroup screenCanvasGroup;

	[SerializeField]
    [TextArea]
	private string winText;
	[SerializeField]
	private string playerKey;
	[SerializeField]
	private float endGameDelay;

	private void Awake()
	{
		if (GameManager.IsInstanced)
		{
			GameManager.Instance.WinnerPlayerId.ValueChanged += SetWinnerInfo;
		}
		else
        {
			GameManager.OnInstantiate += (_, _) =>
			{
				GameManager.Instance.WinnerPlayerId.ValueChanged += SetWinnerInfo;
			};
        }

		if (GameStateManager.IsInstanced)
		{
			GameStateManager.Instance.CurrentGameState.ValueChanged += SetScreenDisplayBasedOnCurrentGameState;
		}
		else
		{
			GameStateManager.OnInstantiate += (_, _) =>
			{
				GameStateManager.Instance.CurrentGameState.ValueChanged += SetScreenDisplayBasedOnCurrentGameState;
			};
		}
	}

	private void SetWinnerInfo(int lastValue, int newValue)
	{
		gameWinnerText.text = winText.Replace(playerKey, PlayerData.GetNameById(newValue));
	}

	private void SetScreenDisplayBasedOnCurrentGameState(int lastGameState, int newGameState)
	{
		if ((GameStateManager.GameState)newGameState == GameStateManager.GameState.MatchEnded)
        {
			screenCanvasGroup.alpha = 1;
			UIHudController.Instance.DeathScreen.SetActive(false);
			CursorManager.Instance.UnlockCursor();
			StartCoroutine(DelayedReturnToMenu());
        }
		else
        {
			screenCanvasGroup.alpha = 0;
		}
	}

	private IEnumerator DelayedReturnToMenu()
    {
		yield return new WaitForSeconds(endGameDelay);
		SceneManager.LoadScene(0);
    }
}
