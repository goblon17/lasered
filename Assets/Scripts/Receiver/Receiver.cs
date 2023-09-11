using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Receiver : MonoBehaviour, ISelectable
{
    [SerializeField]
    private Vector3 endPoint;
    [SerializeField]
    private IntEvent activateEvent;
    [SerializeField]
    private IntEvent deactivateEvent;
    [SerializeField]
    private List<SerializedPair<ActivationType, Emitter>> activatedEmitters;

    [Header("Debug")]
    [SerializeField]
    private bool drawGizmos;
    [SerializeField]
    private float sphereRadius;

    public event System.Action<Receiver, int> ActivateEvent;
    public event System.Action<Receiver, int> DeactivateEvent;

    public Vector3 Point => transform.TransformPoint(endPoint);

    public bool IsSelected { get; private set; } = false;

    public Vector3 Position => transform.position;
    public bool CanBeSelected => true;

    private Dictionary<int, int> activationCounter = new Dictionary<int, int>();

    public void Activate(int playerId)
    {
        if (!activationCounter.ContainsKey(playerId))
        {
            activationCounter[playerId] = 0;
        }
        activationCounter[playerId]++;
        activateEvent.Invoke(playerId);
        ActivateEvent?.Invoke(this, playerId);
    }

    public void Deactivate(int playerId)
    {
        if (!activationCounter.ContainsKey(playerId))
        {
            activationCounter[playerId] = 0;
        }
        else
        {
            activationCounter[playerId]--;
        }
        if (activationCounter[playerId] <= 0)
        {
            deactivateEvent.Invoke(playerId);
            DeactivateEvent?.Invoke(this, playerId);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (!drawGizmos)
        {
            return;
        }
        Gizmos.color = Color.gray;
        Gizmos.DrawSphere(Point, sphereRadius);

        Gizmos.color = Color.green;
        for (int i = 0; i < activateEvent.GetPersistentEventCount(); i++)
        {
            Object o = activateEvent.GetPersistentTarget(i);
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

        Gizmos.color = Color.red;
        for (int i = 0; i < deactivateEvent.GetPersistentEventCount(); i++)
        {
            Object o = deactivateEvent.GetPersistentTarget(i);
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
        IsSelected = false;
    }
}
