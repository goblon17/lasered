using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Receiver : MonoBehaviour
{
    [SerializeField]
    private Vector3 endPoint;
    [SerializeField]
    private IntEvent activateEvent;
    [SerializeField]
    private UnityEvent deactivateEvent;

    [Header("Debug")]
    [SerializeField]
    private bool drawGizmos;
    [SerializeField]
    private float sphereRadius;

    public Vector3 Point => transform.TransformPoint(endPoint);

    private int activationCounter = 0;

    public void Activate(int playerId)
    {
        activationCounter++;
        activateEvent.Invoke(playerId);
    }

    public void Deactivate()
    {
        activationCounter--;
        if (activationCounter <= 0)
        {
            deactivateEvent.Invoke();
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
}
