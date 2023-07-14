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

    private PlayerInputCollector playerInputCollector;

    private Vector2 moveDirection = Vector2.zero;

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
    }

    private void SerializeInput(IInputWriter inputSerializer)
    {
        inputSerializer.Write(playerInputCollector.MoveDirection.x);
        inputSerializer.Write(playerInputCollector.MoveDirection.y);
    }

    private void DeserializeInput(IInputReader inputReader)
    {
        float tmp;
        inputReader.Read(out tmp);
        moveDirection.x = tmp;
        inputReader.Read(out tmp);
        moveDirection.y = tmp;
    }
}
