using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Elympics;

public class CubeDiffraction : MonoBehaviour
{
    public struct LaserDiffraction
    {
        public Vector3 StartPoint;
        public Vector3 StartDirection;
        public Vector3 EndPoint;
        public Vector3 EndDirection;
        public Vector3 AdditionalPoint;
        public Vector3 AdditionalDirection;
    }

    [Header("Config")]
    [SerializeField]
    private Vector3 pointFront;
    [SerializeField]
    private Vector3 directionFront;
    [SerializeField]
    private Vector3 pointLeft;
    [SerializeField]
    private Vector3 directionLeft;
    [SerializeField]
    private Vector3 pointRight;
    [SerializeField]
    private Vector3 directionRight;

    [Header("Debug")]
    [SerializeField]
    private bool drawGizmos;
    [SerializeField]
    private float sphereRadius;

    public LaserDiffraction HandleLaserDiffraction(Vector3 direction)
    {
        LaserDiffraction diffraction = new LaserDiffraction();
        Vector3 localDirection = transform.InverseTransformDirection(direction);
        float angle = Vector3.SignedAngle(Vector3.forward, localDirection, Vector3.up);
        switch (angle)
        {
            case > -180 and <= -60:
                diffraction.StartPoint = transform.TransformPoint(pointRight);
                diffraction.StartDirection = Vector3.zero;
                diffraction.EndPoint = transform.TransformPoint(pointLeft);
                diffraction.EndDirection = transform.TransformDirection(directionLeft);
                diffraction.AdditionalPoint = transform.TransformPoint(pointFront);
                diffraction.AdditionalDirection = transform.TransformDirection(directionFront);
                break;
            case > -60 and <= 60:
                diffraction.StartPoint = transform.TransformPoint(pointFront);
                diffraction.StartDirection = Vector3.zero;
                diffraction.EndPoint = transform.TransformPoint(pointRight);
                diffraction.EndDirection = transform.TransformDirection(directionRight);
                diffraction.AdditionalPoint = transform.TransformPoint(pointLeft);
                diffraction.AdditionalDirection = transform.TransformDirection(directionLeft);
                break;
            case > 60:
                diffraction.StartPoint = transform.TransformPoint(pointLeft);
                diffraction.StartDirection = Vector3.zero;
                diffraction.EndPoint = transform.TransformPoint(pointFront);
                diffraction.EndDirection = transform.TransformDirection(directionFront);
                diffraction.AdditionalPoint = transform.TransformPoint(pointRight);
                diffraction.AdditionalDirection = transform.TransformDirection(directionRight);
                break;
        }
        return diffraction;
    }

    private void OnDrawGizmos()
    {
        if (!drawGizmos)
        {
            return;
        }
        Gizmos.color = PlayerData.PlayerColorToColor(PlayerData.PlayerColor.None);
        Gizmos.DrawSphere(transform.TransformPoint(pointFront), sphereRadius);
        Gizmos.DrawLine(transform.TransformPoint(pointFront), transform.TransformPoint(pointFront) + transform.TransformDirection(directionFront).normalized);

        Gizmos.DrawSphere(transform.TransformPoint(pointLeft), sphereRadius);
        Gizmos.DrawLine(transform.TransformPoint(pointLeft), transform.TransformPoint(pointLeft) + transform.TransformDirection(directionLeft).normalized);

        Gizmos.DrawSphere(transform.TransformPoint(pointRight), sphereRadius);
        Gizmos.DrawLine(transform.TransformPoint(pointRight), transform.TransformPoint(pointRight) + transform.TransformDirection(directionRight).normalized);
    }
}
