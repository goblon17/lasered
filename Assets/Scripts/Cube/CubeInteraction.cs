using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Elympics;

[RequireComponent(typeof(Rigidbody))]
public class CubeInteraction : ElympicsMonoBehaviour, IInteractable, IUpdatable
{
    [SerializeField]
    private Vector3 pickupOffset;

    public Vector3 Position => rigidbody.position;

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
    }

    public void StopInteracting()
    {
        playerId.Value = -1;
        rigidbody.isKinematic = false;
    }

    public void Select()
    {

    }

    public void Unselect()
    {
        
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
            playerData = ClientProvider.Instance.GetPlayerById(playerId.Value);
            if (playerData != null && playerAimer == null)
            {
                playerAimer = playerData.GetComponent<PlayerAimer>();
            }
        }
        rigidbody.position = playerAimer.transform.position + playerAimer.Rotation * pickupOffset;
        rigidbody.rotation = playerAimer.Rotation;
    }
}
