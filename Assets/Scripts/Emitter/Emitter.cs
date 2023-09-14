using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Elympics;

public class Emitter : ElympicsMonoBehaviour, IUpdatable
{
    [Header("References")]
    [SerializeField]
    private string laserPrefabResourcePath;
    [SerializeField]
    private SoundModule soundModule;

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

    [Header("Events")]
    [SerializeField]
    private BoolEvent powerEvent;
    [SerializeField]
    private PlayerColorEvent playerColorEvent;

    [Header("Debug")]
    [SerializeField]
    private bool drawGizmos;
    [SerializeField]
    private float sphereRadius;

    private Laser laser = null;

    private new Renderer renderer;

    private ElympicsInt colorMaterialInt = new ElympicsInt(-1);
    private ElympicsBool isShooting = new ElympicsBool(false);

    private Vector3 emissionDirection => Quaternion.Euler(0, emissionRotation, 0) * transform.forward;

    private void ShootLaser()
    {
        if (laser == null)
        {
            laser = ElympicsInstantiate(laserPrefabResourcePath, ElympicsPlayer.All).GetComponent<Laser>();
        }
        laser.Begin(transform.TransformPoint(emissionPosition), emissionDirection, playerColor, damage);
        colorMaterialInt.Value = (int)playerColor;
    }

    private void StopLaser()
    {
        if (laser == null)
        {
            return;
        }
        laser.Stop();
        colorMaterialInt.Value = -1;
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
        if (isShooting.Value)
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
        colorMaterialInt.Value = (int)playerColor;
    }

    public void StartShooting()
    {
        isShooting.Value = true;
    }

    public void StopShooting()
    {
        isShooting.Value = false;
    }

    private void Awake()
    {
        isShooting.ValueChanged += (_, v) => powerEvent.Invoke(v);
        if (shootOnAwake)
        {
            isShooting.Value = true;
        }
        powerEvent.Invoke(shootOnAwake);
        powerEvent.AddListener((v) => soundModule.PlaySound(v ? "On" : "Off"));
        renderer = GetComponent<Renderer>();
        Color c = renderer.materials[2].color;
        c.a = 0;
        renderer.materials[2].color = c;
        colorMaterialInt.ValueChanged += (_, v) => ChangeMaterialColor(v);
        colorMaterialInt.Value = (int)playerColor;
        playerColorEvent.Invoke(playerColor);
        playerColorEvent.AddListener((_) => soundModule.PlaySound("Color"));
    }

    private void ChangeMaterialColor(int val)
    {
        PlayerData.PlayerColor playerColor = (PlayerData.PlayerColor)val;
        if (val == -1)
        {
            Color c = renderer.materials[2].color;
            c.a = 0;
            renderer.materials[2].color = c;
        }
        else
        {
            renderer.materials[2].color = PlayerData.PlayerColorToColor(playerColor);
        }
        playerColorEvent.Invoke(playerColor);
    }

    public void ShowColor()
    {
        UIHudController.Instance.InteractionTooltip.ShowColor(transform, () => (PlayerData.PlayerColor)colorMaterialInt.Value);
    }
    
    public void HideColor()
    {
        UIHudController.Instance.InteractionTooltip.HideColor(transform);
    }

    public void ShowPower(bool inverted = false)
    {
        UIHudController.Instance.InteractionTooltip.ShowPower(transform, () => isShooting.Value ^ inverted);
    }
    
    public void HidePower()
    {
        UIHudController.Instance.InteractionTooltip.HidePower(transform);
    }
}