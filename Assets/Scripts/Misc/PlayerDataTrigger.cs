using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class PlayerDataTrigger : MonoBehaviour
{
    [SerializeField]
    private UnityEvent onTriggerEnterEvent;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerData playerData))
        {
            onTriggerEnterEvent.Invoke();
        }
    }
}
