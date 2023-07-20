using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Elympics;

public class Emitter : ElympicsMonoBehaviour, IUpdatable
{
    [Header("References")]
    [SerializeField]
    private string laserPrefabResourcePath;

    [Header("Config")]
    [SerializeField]
    private Vector3 emissionPosition;
    [SerializeField]
    private float emissionRotation;
    [SerializeField]
    private PlayerData.PlayerColor playerColor;
    [SerializeField]
    private float damage;

    [Header("Debug")]
    [SerializeField]
    private bool drawGizmos;
    [SerializeField]
    private float sphereRadius;

    private Laser laser = null;

    private Vector3 emissionDirection => Quaternion.Euler(0, emissionRotation, 0) * transform.forward;

    private void ShootLaser()
    {
        if (laser == null)
        {
            laser = ElympicsInstantiate(laserPrefabResourcePath, ElympicsPlayer.All).GetComponent<Laser>();
        }
        laser.Begin(transform.position + emissionPosition, emissionDirection, playerColor, damage);
    }

    private void OnDrawGizmos()
    {
        if (!drawGizmos)
        {
            return;
        }
        Gizmos.color = PlayerData.PlayerColorToColor(playerColor);
        Gizmos.DrawSphere(transform.position + emissionPosition, sphereRadius);
        Gizmos.DrawLine(transform.position + emissionPosition, transform.position + emissionPosition + emissionDirection);
    }

    public void ElympicsUpdate()
    {
        ShootLaser();
    }
}