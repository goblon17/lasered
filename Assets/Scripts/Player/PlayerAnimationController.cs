using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Elympics;

[RequireComponent(typeof(Animator))]
public class PlayerAnimationController : ElympicsMonoBehaviour, IUpdatable
{
    [Header("References")]
    [SerializeField]
    private PlayerAimer playerAimer;
    [SerializeField]
    private PlayerMovement playerMovement;
    [SerializeField]
    private PlayerInteracter playerInteracter;
    [SerializeField]
    private PlayerHealth playerHealth;
    [SerializeField]
    private PlayerData playerData;
    [SerializeField]
    private Rigidbody playerRigidbody;

    [Header("Armarure")]
    [SerializeField]
    private Transform spineBone;
    [SerializeField]
    private Transform pelvisBone;

    [Header("Config")]
    [SerializeField]
    private int damagedTickDuration;

    private Animator animator;

    private float armsYRotation = 0;
    private float legsYRotation = 0;

    private Vector3 armsEuler;
    private Vector3 legsEuler;

    private float previousHealth = 0;

    private ElympicsInt damagedSetCounter = new ElympicsInt(0);

    private void Start()
    {
        animator = GetComponent<Animator>();

        armsEuler = spineBone.localEulerAngles;
        legsEuler = pelvisBone.localEulerAngles;

        playerInteracter.InteractionAnimationEvent += OnInteractionAnimation;
        playerHealth.HealthChangedEvent += OnHealthChanged;
    }

    private void OnInteractionAnimation(string type, bool interacting)
    {
        if (string.IsNullOrEmpty(type))
        {
            return;
        }

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

    private void OnHealthChanged(float currentHealth, float maxHealth)
    {
        if (previousHealth > currentHealth)
        {
            animator.SetTrigger("Damaged");
            animator.SetFloat("Damaged Float", 1);
            damagedSetCounter.Value = 0;
        }
        previousHealth = currentHealth;
    }

    private void Update()
    {
        animator.SetFloat("Speed", playerRigidbody.velocity.magnitude / playerMovement.Speed);
    }

    private void LateUpdate()
    {
        if (Elympics.Player == playerData.Player)
        {
            armsYRotation = -playerAimer.YRotation;
        }
        else
        {
            armsYRotation = -playerAimer.YRotationSynch;
        }

        if (playerRigidbody.velocity != Vector3.zero)
        {
            legsYRotation = Vector3.SignedAngle(playerRigidbody.transform.forward, playerRigidbody.velocity, playerRigidbody.transform.up);
        }

        spineBone.localEulerAngles = armsEuler + new Vector3(0, 0, armsYRotation);
        pelvisBone.localEulerAngles = legsEuler + new Vector3(0, 0, legsYRotation);

        if (damagedSetCounter.Value >= damagedTickDuration)
        {
            animator.SetFloat("Damaged Float", 0);
        }
    }

    public void ElympicsUpdate()
    {
        if (damagedSetCounter.Value < damagedTickDuration)
        {
            damagedSetCounter.Value++;
        }
    }
}
