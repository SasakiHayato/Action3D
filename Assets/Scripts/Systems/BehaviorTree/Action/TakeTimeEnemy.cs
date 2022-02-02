using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourAI;

public class TakeTimeEnemy : IAction
{
    public void SetUp()
    {

    }

    public bool Execute()
    {
        return true;
    }

    public GameObject Target { get; set; }
}
