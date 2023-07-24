using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Elympics;

public class PlayerAimer : ElympicsMonoBehaviour
{
    [SerializeField]
    private float sensitivity;

    public Vector3 Forward => Quaternion.Euler(0, yRotation.Value, 0) * transform.forward;
    public Quaternion Rotation => Quaternion.Euler(0, yRotation.Value, 0);
    public float YRotation => yRotation.Value;

    private ElympicsFloat yRotation = new ElympicsFloat(0, comparer: new ElympicsFloatEqualityComparer(Mathf.Epsilon));

    private Vector2 aimDirection = Vector2.zero;

    public void SetAimDirection(Vector2 aimDir)
    {
        aimDirection += aimDir * sensitivity;
        aimDirection = Vector2.ClampMagnitude(aimDirection, 1);
        yRotation.Value = -Vector2.SignedAngle(new Vector2(transform.forward.x, transform.forward.z), aimDirection);
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + Forward * 5);
    }
}
