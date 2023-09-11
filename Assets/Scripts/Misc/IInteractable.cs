using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable : ISelectable
{
    public void Interact(int playerId);
    public void StopInteracting();

    public string AnimationType { get; }
}
