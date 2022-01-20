using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCanvas : MonoBehaviour
{
    EnemyBase _enemyBase;
    Canvas _canvas;
    int _saveHp; 

    void Start()
    {
        _enemyBase = transform.parent.GetComponent<EnemyBase>();
        _saveHp = _enemyBase.HP;
        _canvas = gameObject.GetComponent<Canvas>();
        gameObject.SetActive(false);
    }

    void Update()
    {
        if (_enemyBase.HP >= _saveHp) return;
        else
        {
            if (!gameObject.activeSelf) gameObject.SetActive(true);
        }
        _canvas.transform.rotation = Camera.main.transform.rotation;
    }
}
