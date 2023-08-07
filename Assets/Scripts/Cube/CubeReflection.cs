using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeReflection : MonoBehaviour
{
    [Header("Config")]
    [SerializeField]
    private Vector3 point;
    [SerializeField]
    private Vector3 direction;

    [Header("Debug")]
    [SerializeField]
    private bool drawGizmos;
    [SerializeField]
    private float sphereRadius;

    public Vector3 Point => transform.TransformPoint(point);
    public Vector3 Direction => transform.TransformDirection(direction).normalized;

    private void OnDrawGizmos()
    {
        if (!drawGizmos)
        {
            return;
        }
        Gizmos.color = PlayerData.PlayerColorToColor(PlayerData.PlayerColor.None);
        Gizmos.DrawSphere(transform.TransformPoint(point), sphereRadius);
        Gizmos.DrawLine(transform.TransformPoint(point), transform.TransformPoint(point) + transform.TransformDirection(direction).normalized);
    }
}
