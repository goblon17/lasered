using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Elympics;

public class GameInitializer : ElympicsSingleton<GameInitializer>, IUpdatable
{
    [SerializeField]
    private float timeToStartMatch;

	public ElympicsFloat CurrentTimeToStartMatch { get; } = new ElympicsFloat(0.0f);

	private ElympicsBool gameInitializationEnabled = new ElympicsBool(false);

	private System.Action OnMatchInitializedAssignedCallback = null;

	public void InitializeMatch(System.Action OnMatchInitializedCallback)
	{
		OnMatchInitializedAssignedCallback = OnMatchInitializedCallback;

		CurrentTimeToStartMatch.Value = timeToStartMatch;
		gameInitializationEnabled.Value = true;
	}

	public void ElympicsUpdate()
	{
		if (gameInitializationEnabled.Value)
		{
			CurrentTimeToStartMatch.Value -= Elympics.TickDuration;

			if (CurrentTimeToStartMatch.Value <= 0.0f)
			{
				OnMatchInitializedAssignedCallback?.Invoke();

				gameInitializationEnabled.Value = false;
			}
		}
	}
}
