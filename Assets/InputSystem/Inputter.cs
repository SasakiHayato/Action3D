using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.InputSystem;

public enum InputType
{
    PlayerMove,
    CmMove,

    None,
}

public class Inputter : MonoBehaviour
{
    private static Inputter _instance = null;
    public static Inputter Instance
    {
        get
        {
            object instance = FindObjectOfType(typeof(Inputter));
            if (instance != null) _instance = (Inputter)instance;
            else
            {
                GameObject obj = new GameObject("Inputter");
                _instance = obj.AddComponent<Inputter>();
                obj.hideFlags = HideFlags.HideInHierarchy;
            }

            return _instance;
        }
    }

    public InputData Inputs { get => _inputs; }
    InputData _inputs;

    private void Awake()
    {
        _inputs = new InputData();
        _inputs.Enable();
    }

    public static void Init()
    {
        _instance._inputs.Dispose();
    }

    public static object GetValue(InputType type)
    {
        object obj = null;
        switch (type)
        {
            case InputType.PlayerMove:
                obj = Instance._inputs.Player.Move.ReadValue<Vector2>();
                break;
            case InputType.CmMove:
                obj = Instance._inputs.Player.MoveCm.ReadValue<Vector2>();
                break;
            case InputType.None:
                break;
        }

        return obj;
    }
}
