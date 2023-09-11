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

    public bool IsInteracting => isInteracting.Value;
    private IInteractable interactableObject => selectedObject as IInteractable;

    private ElympicsBool isInteracting = new ElympicsBool(false);

    private ElympicsString selectedObjectAnimationType = new ElympicsString("");

    private ISelectable selectedObject = null;
    private List<ISelectable> selectedObjects = new List<ISelectable>();

    private void Start()
    {
        isInteracting.ValueChanged += (old, cur) =>
        {
            InteractionAnimationEvent?.Invoke(selectedObjectAnimationType.Value, cur);
        };
    }

    public void StartInteracting()
    {
        if (!isInteracting.Value)
        {
            isInteracting.Value = true;
            if (interactableObject != null)
            {
                interactableObject.Interact(GetComponent<PlayerData>().PlayerId);
            }
            if (debugInfo)
            {
                Debug.Log("Interacting with an object.");
            }
        }
    }

    public void StopInteracting()
    {
        if (isInteracting.Value)
        {
            isInteracting.Value = false;
            if (interactableObject != null)
            {
                interactableObject.StopInteracting();
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
            if (collider.TryGetComponent(out ISelectable selectable))
            {
                if (selectable.CanBeSelected)
                {
                    selectedObjects.Add(selectable);
                }
            }
        }
        UpdateSelectedObject();
    }

    private void UpdateSelectedObject()
    {
        if (isInteracting.Value)
        {
            return;
        }

        ISelectable minSelect = null;
        float minDistance = Mathf.Infinity;
        foreach (ISelectable selected in selectedObjects)
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
                selectedObjectAnimationType.Value = "";
            }
            selectedObject = minSelect;
            if (selectedObject != null && select)
            {
                selectedObject.Select();
                if (interactableObject != null)
                {
                    selectedObjectAnimationType.Value = interactableObject.AnimationType;
                }
                else
                {
                    selectedObjectAnimationType.Value = "";
                }
            }
        }
        if (selectedObject != null && !selectedObject.IsSelected)
        {
            selectedObject.Select();
            if (interactableObject != null)
            {
                selectedObjectAnimationType.Value = interactableObject.AnimationType;
            }
            else
            {
                selectedObjectAnimationType.Value = "";
            }
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
