using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Elympics;

public class PlayerAimer : ElympicsMonoBehaviour
{
    [SerializeField]
    private float sensitivity;

    public Vector3 Forward =>  Rotation * transform.forward;
    public Quaternion Rotation => Quaternion.FromToRotation(transform.forward, new Vector3(aimDirectionSynch.Value.x, 0, aimDirectionSynch.Value.y));
    public float YRotation => yRotation;
    public event System.Action<Vector2> AimDirectionChangedEvent;

    private ElympicsVector2 aimDirectionSynch = new ElympicsVector2(Vector2.zero, comparer: new ElympicsVector2EqualityComparer(Mathf.Epsilon));

    private Vector2 aimDirection = Vector2.zero;
    private float yRotation = 0;

    public void SetAimDirection(Vector2 aimDir)
    {
        aimDirection = aimDirectionSynch.Value;
        aimDirection += aimDir * sensitivity;
        aimDirection = Vector2.ClampMagnitude(aimDirection, 1);
        AimDirectionChangedEvent?.Invoke(aimDirection);
        yRotation = -Vector2.SignedAngle(new Vector2(transform.forward.x, transform.forward.z), aimDirection);
        aimDirectionSynch.Value = aimDirection;
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + Forward * 5);
    }
}
