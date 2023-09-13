using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Elympics;

[RequireComponent(typeof(PlayerInputCollector))]
[RequireComponent(typeof(PlayerData))]
[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(AudioListener))]
public class PlayerInputController : ElympicsMonoBehaviour, IInputHandler, IInitializable, IUpdatable
{
    [Header("References")]
    [SerializeField]
    private PlayerMovement playerMovement;
    [SerializeField]
    private PlayerAimer playerAimer;
    [SerializeField]
    private PlayerInteracter playerInteracter;

    private PlayerInputCollector playerInputCollector;
    private PlayerData playerData;
    private PlayerInput playerInput;
    private AudioListener playerAudioListener;

    private Vector2 moveDirection = Vector2.zero;
    private Vector2 aimDirection = Vector2.zero;
    private bool pickUpButton = false;
    private float rotationDirection = 0;

    public void Initialize()
    {
        playerInputCollector = GetComponent<PlayerInputCollector>();
        playerInput = GetComponent<PlayerInput>();
        playerData = GetComponent<PlayerData>();
        playerAudioListener = GetComponent<AudioListener>();

        playerData.InitializedEvent += SetupPlayerInput;
    }

    private void SetupPlayerInput()
    {
        if (!Elympics.IsServer)
        {
            playerInput.enabled = Elympics.Player == playerData.Player;
            playerAudioListener.enabled = Elympics.Player == playerData.Player;
        }

        playerData.InitializedEvent -= SetupPlayerInput;
    }

    public void ElympicsUpdate()
    {
        if (ElympicsBehaviour.TryGetInput(playerData.Player, out IInputReader inputReader))
        {
            DeserializeInput(inputReader);
        }
        if (GameStateManager.Instance.CurrentGameState.Value == (int)GameStateManager.GameState.GameplayMatchRunning)
        {
            ApplyInput();
        }
    }

    public void OnInputForBot(IInputWriter inputSerializer)
    {
        //SerializeInput(inputSerializer);
    }

    public void OnInputForClient(IInputWriter inputSerializer)
    {
        SerializeInput(inputSerializer);
    }

    private void ApplyInput()
    {
        playerMovement.SetDirection(moveDirection);
        playerAimer.SetAimDirectionSynch(aimDirection);
        if (playerInteracter.IsInteracting && !pickUpButton)
        {
            playerInteracter.StopInteracting();
        }
        else if (!playerInteracter.IsInteracting && pickUpButton)
        {
            playerInteracter.StartInteracting();
        }
    }

    private void SerializeInput(IInputWriter inputSerializer)
    {
        inputSerializer.Write(playerInputCollector.MoveDirection.x);
        inputSerializer.Write(playerInputCollector.MoveDirection.y);
        inputSerializer.Write(playerInputCollector.AimDirection.x);
        inputSerializer.Write(playerInputCollector.AimDirection.y);
        inputSerializer.Write(playerInputCollector.PickUpButton);
        inputSerializer.Write(playerInputCollector.RotationDirection);
    }

    private void DeserializeInput(IInputReader inputReader)
    {
        float tmpFloat;
        bool tmpBool;

        inputReader.Read(out tmpFloat);
        moveDirection.x = tmpFloat;
        inputReader.Read(out tmpFloat);
        moveDirection.y = tmpFloat;

        inputReader.Read(out tmpFloat);
        aimDirection.x = tmpFloat;
        inputReader.Read(out tmpFloat);
        aimDirection.y = tmpFloat;

        inputReader.Read(out tmpBool);
        pickUpButton = tmpBool;

        inputReader.Read(out tmpFloat);
        rotationDirection = tmpFloat;
    }
}
