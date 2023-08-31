using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Elympics;

[RequireComponent(typeof(Rigidbody))]
public class CubeInteraction : ElympicsMonoBehaviour, IInteractable, IUpdatable
{
    [Header("Config")]
    [SerializeField]
    private Vector3 pickupOffset;
    [SerializeField]
    private Vector3 iconOffset;

    [Header("Debug")]
    private bool drawGizmos;
    private float gizmoSize;

    public Vector3 Position => rigidbody.position;
    public bool IsSelected { get; private set; } = false;

    public string AnimationType => "Holding";

    public bool CanBeInteractedWith => playerId.Value < 0;

    private new Rigidbody rigidbody;
    private PlayerData playerData;
    private PlayerAimer playerAimer;

    private ElympicsInt playerId = new ElympicsInt(-1);

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    public void Interact(int playerId)
    {
        this.playerId.Value = playerId;
        rigidbody.isKinematic = true;
        Unselect();
    }

    public void StopInteracting()
    {
        playerId.Value = -1;
        rigidbody.isKinematic = false;
    }

    public void Select()
    {
        UIHudController.Instance.InteractionTooltip.ShowInteraction(this, transform);
        IsSelected = true;
    }

    public void Unselect()
    {
        UIHudController.Instance.InteractionTooltip.HideInteraction(this);
        IsSelected = false;
    }

    public void ElympicsUpdate()
    {
        if (CanBeInteractedWith)
        {
            playerData = null;
            playerAimer = null;
            return;
        }

        if (playerData == null)
        {
            playerData = ClientProvider.Instance.GetPlayer(playerId.Value);
            if (playerData != null && playerAimer == null)
            {
                playerAimer = playerData.GetComponent<PlayerAimer>();
            }
        }
        if (playerAimer == null)
        {
            return;
        }
        rigidbody.position = playerAimer.transform.position + playerAimer.Rotation * pickupOffset;
        rigidbody.rotation = playerAimer.Rotation;
    }

    private void OnDrawGizmos()
    {
        if (!drawGizmos)
        {
            return;
        }

        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere(transform.TransformPoint(iconOffset), gizmoSize);
    }
}
