using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : Singleton<CursorManager>
{
    public bool Locked { get; private set; } = true;

    private void Start()
    {
        LockCursor();
    }

    public void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Locked = true;
    }

    public void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Locked = false;
    }
}
