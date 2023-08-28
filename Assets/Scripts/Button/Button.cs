using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Button : MonoBehaviour, IInteractable
{
    [SerializeField]
    private IntEvent interactEvent;

    public bool IsSelected { get; private set; } = false;

    public Vector3 Position => transform.position;

    public string AnimationType => "Click";

    public bool CanBeInteractedWith => true;

    public void Interact(int playerId)
    {
        interactEvent.Invoke(playerId);
    }

    public void StopInteracting() { }

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

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        for (int i = 0; i < interactEvent.GetPersistentEventCount(); i++)
        {
            Object o = interactEvent.GetPersistentTarget(i);
            Vector3? start = null;
            switch (o)
            {
                case MonoBehaviour monoBehaviour:
                    start = monoBehaviour.transform.position;
                    break;
                case GameObject gameObject:
                    start = gameObject.transform.position;
                    break;
            }
            if (start != null)
            {
                Gizmos.DrawLine(transform.position, start.Value);
            }
        }
    }
}
