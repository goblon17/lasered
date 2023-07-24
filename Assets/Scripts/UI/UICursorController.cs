using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICursorController : MonoBehaviour
{
    [SerializeField]
    private float maxMagnitude;

    public void SetAimDirection(Vector2 aimDirection)
    {
        transform.localPosition = aimDirection * maxMagnitude;
    }
}
