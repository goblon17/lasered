using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISelectable
{
    public bool IsSelected { get; }
    public void Select();
    public void Unselect();

    public Vector3 Position { get; }
    public bool CanBeSelected { get; }
}
