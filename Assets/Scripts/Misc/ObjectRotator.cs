using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRotator : MonoBehaviour
{
    [SerializeField]
    private Vector3 rotationSpeed;

    private void Update()
    {
        transform.localEulerAngles += rotationSpeed * Time.deltaTime;
    }
}
