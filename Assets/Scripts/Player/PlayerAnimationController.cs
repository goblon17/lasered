using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimationController : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private PlayerAimer playerAimer;
    [SerializeField]
    private PlayerMovement playerMovement;
    [SerializeField]
    private PlayerInteracter playerInteracter;
    [SerializeField]
    private Rigidbody playerRigidbody;

    [Header("Armarure")]
    [SerializeField]
    private Transform spineBone;
    [SerializeField]
    private Transform pelvisBone;

    private Animator animator;

    private float armsYRotation = 0;
    private float legsYRotation = 0;

    private Vector3 armsEuler;
    private Vector3 legsEuler;

    private void Start()
    {
        animator = GetComponent<Animator>();

        armsEuler = spineBone.localEulerAngles;
        legsEuler = pelvisBone.localEulerAngles;

        playerInteracter.InteractionAnimationEvent += OnInteractionAnimation;
    }

    private void OnInteractionAnimation(string type, bool interacting)
    {
        switch (type)
        {
            case "Holding":
                animator.SetBool(type, interacting);
                break;
            case "Click":
                animator.SetTrigger(type);
                break;
            default:
                Debug.LogWarning($"No {type} interaction animation type implemented");
                break;
        }
    }

    private void Update()
    {
        animator.SetFloat("Speed", playerRigidbody.velocity.magnitude / playerMovement.Speed);
    }

    private void LateUpdate()
    {
        armsYRotation = -playerAimer.YRotation;

        if (playerRigidbody.velocity != Vector3.zero)
        {
            legsYRotation = Vector3.SignedAngle(playerRigidbody.transform.forward, playerRigidbody.velocity, playerRigidbody.transform.up);
        }

        spineBone.localEulerAngles = armsEuler + new Vector3(0, 0, armsYRotation);
        pelvisBone.localEulerAngles = legsEuler + new Vector3(0, 0, legsYRotation);
    }
}
