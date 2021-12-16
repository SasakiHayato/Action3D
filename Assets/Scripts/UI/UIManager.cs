using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private static UIManager _instance = null;
    public static UIManager Instance => _instance;

    [SerializeReference, SubclassSelector]
    List<UIWindowParent> _windows = new List<UIWindowParent>();

    void Awake()
    {
        _instance = this;
    }

    void Start()
    {
        foreach (UIWindowParent ui in _windows)
        {
            ui.SetUp();
        }
    }

    void Update()
    {
        foreach (UIWindowParent ui in _windows)
        {
            ui.UpDate();
        }
    }

    public static void CallBack(UIType type, int id, object[] data = null)
    {
        foreach (UIWindowParent ui in Instance._windows)
        {
            if (ui.GetUIType == type)
            {
                ui.CallBack(id, data);
                return;
            }
        }
    }
}
