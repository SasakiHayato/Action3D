using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public void OnClick()
    {
        Debug.Log("OnClick");
        UIManager.CallBack(UIType.EnemyConnect, 1, new object[] { "Šæ’£‚Á‚½EnemyAI" });
    }
}
