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
    [SerializeField]
    private bool shootOnAwake;

    [Header("Debug")]
    [SerializeField]
    private bool drawGizmos;
    [SerializeField]
    private float sphereRadius;

    private Laser laser = null;

    private bool isShooting = false;

    private Vector3 emissionDirection => Quaternion.Euler(0, emissionRotation, 0) * transform.forward;

    private void ShootLaser()
    {
        if (laser == null)
        {
            laser = ElympicsInstantiate(laserPrefabResourcePath, ElympicsPlayer.All).GetComponent<Laser>();
        }
        laser.Begin(transform.TransformPoint(emissionPosition), emissionDirection, playerColor, damage);
    }

    private void StopLaser()
    {
        if (laser == null)
        {
            return;
        }
        laser.Stop();
    }

    private void OnDrawGizmos()
    {
        if (!drawGizmos)
        {
            return;
        }
        Gizmos.color = PlayerData.PlayerColorToColor(playerColor);
        Gizmos.DrawSphere(transform.TransformPoint(emissionPosition), sphereRadius);
        Gizmos.DrawLine(transform.TransformPoint(emissionPosition), transform.TransformPoint(emissionPosition) + emissionDirection);
    }

    public void ElympicsUpdate()
    {
        if (isShooting)
        {
            ShootLaser();
        }
        else
        {
            StopLaser();
        }
    }

    public void ChangeColor(int playerId)
    {
        ChangeColor((PlayerData.PlayerColor)(playerId + 1));
    }

    public void ChangeColor(PlayerData.PlayerColor playerColor)
    {
        this.playerColor = playerColor;
    }

    public void StartShooting()
    {
        isShooting = true;
    }

    public void StopShooting()
    {
        isShooting = false;
    }

    private void Awake()
    {
        if (shootOnAwake)
        {
            isShooting = true;
        }
    }
}