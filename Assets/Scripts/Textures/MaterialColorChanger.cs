using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class MaterialColorChanger : MonoBehaviour
{
    private new Renderer renderer;

    private void Start()
    {
        renderer = GetComponent<Renderer>();
    }

    public void ChangeColorByPlayerId(int playerId)
    {
        if (playerId == -1)
        {
            renderer.material.color = Color.white;
        }
        else
        {
            renderer.material.color = PlayerData.PlayerColorToColor((PlayerData.PlayerColor)(playerId + 1));
        }
    }
}
