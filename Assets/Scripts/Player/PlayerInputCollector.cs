using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputCollector : MonoBehaviour
{
    public Vector2 MoveDirection { get; private set; } = Vector2.zero;
    public Vector2 AimDirection { get; private set; } = Vector2.zero;
    public bool PickUpButton { get; private set; } = false;
    public float RotationDirection { get; private set; } = 0;

    public void Move(InputAction.CallbackContext context)
    {
        MoveDirection = context.ReadValue<Vector2>();
    }

    public void Aim(InputAction.CallbackContext context)
    {
        AimDirection = context.ReadValue<Vector2>();
    }

    public void PickUp(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            PickUpButton = true;
        }
        else if (context.canceled)
        {
            PickUpButton = false;
        }
    }

    public void Rotate(InputAction.CallbackContext context)
    {
        RotationDirection = context.ReadValue<float>();
    }
}