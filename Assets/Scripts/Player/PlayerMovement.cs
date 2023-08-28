using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Config")]
    [SerializeField]
    private float speed;

    public float Speed => speed;

    private new Rigidbody rigidbody;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    public void SetDirection(Vector2 direction)
    {
        if (rigidbody == null)
        {
            return;
        }    
        Vector3 moveDirection = new Vector3(direction.x, 0, direction.y);
        rigidbody.velocity = moveDirection * speed;
    }
}
