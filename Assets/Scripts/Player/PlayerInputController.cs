using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Elympics;

[RequireComponent(typeof(PlayerInputCollector))]
public class PlayerInputController : ElympicsMonoBehaviour, IInputHandler, IInitializable, IUpdatable
{
    [Header("References")]
    [SerializeField]
    private PlayerMovement playerMovement;
    [SerializeField]
    private PlayerAimer playerAimer;

    private PlayerInputCollector playerInputCollector;

    private Vector2 moveDirection = Vector2.zero;
    private Vector2 aimDirection = Vector2.zero;
    private bool pickUpButton = false;
    private float rotationDirection = 0;

    public void Initialize()
    {
        playerInputCollector = GetComponent<PlayerInputCollector>();
    }

    public void ElympicsUpdate()
    {
        if (ElympicsBehaviour.TryGetInput(PredictableFor, out IInputReader inputReader))
        {
            DeserializeInput(inputReader);
        }
        ApplyInput();
    }

    public void OnInputForBot(IInputWriter inputSerializer)
    {
        SerializeInput(inputSerializer);
    }

    public void OnInputForClient(IInputWriter inputSerializer)
    {
        SerializeInput(inputSerializer);
    }

    private void ApplyInput()
    {
        playerMovement.SetDirection(moveDirection);
        playerAimer.SetAimDirection(aimDirection);
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
