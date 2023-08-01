using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class ScrollTexture : MonoBehaviour
{
    [SerializeField]
    private Vector2 scrollSpeed;

    private new Renderer renderer;
    private Vector2 scrollPosition;

    private void Update()
    {
        if (renderer == null)
        {
            renderer = GetComponent<Renderer>();
            scrollPosition = renderer.material.mainTextureOffset;
        }
        renderer.material.mainTextureOffset = scrollPosition + scrollSpeed * Time.time;
    }
}
