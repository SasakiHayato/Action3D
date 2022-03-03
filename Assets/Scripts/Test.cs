using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Test : MonoBehaviour
{
    [SerializeField] Button _button1;
    [SerializeField] Button _button2;
    [SerializeField] Button _button3;

    void Start()
    {
        GamePadButtonEvents events = new GamePadButtonEvents();
        GamePadButtonEvents.SetInstance(events, events);

        GamePadButtonEvents.Instance.CreateList(0)
            .AddEvents(_button1, CallBackA)
            .AddEvents(_button2, CallBackB)
            .AddEvents(_button3, CallBackC)
            .FirstSetUp();

        GamePadButtonEvents.Instance.PickUpRequest(0);

        Inputter.Instance.Inputs.UI.Check.started += context => Selected();
    }

    void Update()
    {
        //Select();
        GamePadButtonEvents.Instance.SelectChangeScale(Vector2.one, Vector2.one * 1.5f);
    }

    void Selected()
    {
        GamePadButtonEvents.Instance.IsSelected();
    }

    int _saveID = 0;
    int _setID = 0;

    void CallBackA()
    {
        Debug.Log("A");
    }

    void CallBackB()
    {
        Debug.Log("B");
    }

    void CallBackC()
    {
        Debug.Log("C");
    }
}
