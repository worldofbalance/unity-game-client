using UnityEngine;
using System.Collections;
using System;

public abstract class InputControllerBase:ScriptableObject
{
    protected COSTouchState eTouchRes;
    protected static int walkableAreaMask;

    public COSTouchState TouchState {
        get { return eTouchRes; }
        set { eTouchRes = value; }
    }

    public InputControllerBase ()
    {
        eTouchRes = COSTouchState.None;

    }

    public abstract RaycastHit InputUpdate (Camera _camera);

    public abstract void InputControllerAwake (Terrain terrain);

}

public enum COSTouchState
{
    None,
    TerrainTapped,
    IsZooming,
    IsRotating,
    IsPanning
}

