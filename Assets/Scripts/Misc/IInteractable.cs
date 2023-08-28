using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    public void Interact(int playerId);
    public void StopInteracting();

    public bool IsSelected { get; }
    public void Select();
    public void Unselect();

    public string AnimationType { get; }

    public Vector3 Position { get; }
    public bool CanBeInteractedWith { get; }
}
