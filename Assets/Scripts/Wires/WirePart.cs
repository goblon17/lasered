using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class WirePart : MonoBehaviour
{
    private new Renderer renderer;

    public void SetColor(Color color)
    {
        if (renderer == null)
        {
            renderer = GetComponent<Renderer>();
        }

        color.a = 1;
        renderer.material.color = color;
    }
}
