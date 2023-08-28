using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Elympics;

[RequireComponent(typeof(PlayerData))]
public class PlayerInteracter : ElympicsMonoBehaviour, IUpdatable
{
    [Header("References")]
    [SerializeField]
    private PlayerAimer playerAimer;

    [Header("Config")]
    [SerializeField]
    private Vector3 interactionPosition;
    [SerializeField]
    private Vector3 interactionSize;

    [Header("Debug")]
    [SerializeField]
    private bool drawGizmos = false;
    [SerializeField]
    private bool debugInfo = false;

    public event System.Action<string, bool> InteractionAnimationEvent;

    public bool IsInteracting => isInteracting;

    private bool isInteracting = false;

    private IInteractable selectedObject = null;
    private List<IInteractable> selectedObjects = new List<IInteractable>();

    public void StartInteracting()
    {
        if (!isInteracting)
        {
            isInteracting = true;
            if (selectedObject != null)
            {
                selectedObject.Interact(GetComponent<PlayerData>().PlayerId);
                InteractionAnimationEvent?.Invoke(selectedObject.AnimationType, true);
            }
            if (debugInfo)
            {
                Debug.Log("Interacting with an object.");
            }
        }
    }

    public void StopInteracting()
    {
        if (isInteracting)
        {
            isInteracting = false;
            if (selectedObject != null)
            {
                selectedObject.StopInteracting();
                InteractionAnimationEvent?.Invoke(selectedObject.AnimationType, false);
            }
            if (debugInfo)
            {
                Debug.Log("Stoping interacting with an object.");
            }
        }
    }

    public void ElympicsUpdate()
    {
        Vector3 boxCenter = transform.position + playerAimer.Rotation * interactionPosition;
        Vector3 halfExtents = interactionSize / 2;
        Collider[] colliders = Physics.OverlapBox(boxCenter, halfExtents, playerAimer.Rotation);
        selectedObjects.Clear();
        foreach (Collider collider in colliders)
        {
            if (collider.TryGetComponent(out IInteractable interactable))
            {
                if (interactable.CanBeInteractedWith)
                {
                    selectedObjects.Add(interactable);
                }
            }
        }
        UpdateSelectedObject();
    }

    private void UpdateSelectedObject()
    {
        if (isInteracting)
        {
            return;
        }

        IInteractable minSelect = null;
        float minDistance = Mathf.Infinity;
        foreach (IInteractable selected in selectedObjects)
        {
            float distance = (selected.Position - transform.position).magnitude;
            if (distance <= minDistance)
            {
                minDistance = distance;
                minSelect = selected;
            }
        }
        bool select = false;
        if (selectedObject != minSelect)
        {
            if (minSelect == null)
            {
                if (debugInfo)
                {
                    Debug.LogFormat("Selected nothing");
                }
            }
            else
            {
                if (debugInfo)
                {
                    Debug.LogFormat("Selected object");
                }
                select = true;
            }
            if (selectedObject != null)
            {
                selectedObject.Unselect();
            }
            selectedObject = minSelect;
            if (selectedObject != null && select)
            {
                selectedObject.Select();
            }
        }
        if (selectedObject != null && !selectedObject.IsSelected)
        {
            selectedObject.Select();
        }
    }

    private void OnDrawGizmos()
    {
        if (!drawGizmos)
        {
            return;
        }

        Gizmos.color = Color.yellow;
        Gizmos.matrix = transform.localToWorldMatrix * Matrix4x4.Rotate(playerAimer.Rotation);
        Gizmos.DrawWireCube(interactionPosition, interactionSize);
    }
}
