using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Button : MonoBehaviour, IInteractable
{
    [SerializeField]
    private IntEvent interactEvent;
    [SerializeField]
    private List<SerializedPair<ActivationType, Emitter>> activatedEmitters;

    public bool IsSelected { get; private set; } = false;

    public Vector3 Position => transform.position;

    public string AnimationType => "Click";

    public bool CanBeSelected => true;

    public void Interact(int playerId)
    {
        interactEvent.Invoke(playerId);
    }

    public void StopInteracting() { }

    public void Select()
    {
        foreach (SerializedPair<ActivationType, Emitter> activatedEmitter in activatedEmitters)
        {
            switch (activatedEmitter.Key)
            {
                case ActivationType.Power:
                case ActivationType.PowerInverted:
                    activatedEmitter.Value.ShowPower(activatedEmitter.Key == ActivationType.PowerInverted);
                    break;
                case ActivationType.Color:
                    activatedEmitter.Value.ShowColor();
                    break;
            }
        }
        UIHudController.Instance.InteractionTooltip.ShowInteraction(transform);
        IsSelected = true;
    }

    public void Unselect()
    {
        foreach (SerializedPair<ActivationType, Emitter> activatedEmitter in activatedEmitters)
        {
            switch (activatedEmitter.Key)
            {
                case ActivationType.Power:
                case ActivationType.PowerInverted:
                    activatedEmitter.Value.HidePower();
                    break;
                case ActivationType.Color:
                    activatedEmitter.Value.HideColor();
                    break;
            }
        }
        UIHudController.Instance.InteractionTooltip.HideInteraction(transform);
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
