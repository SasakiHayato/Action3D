using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyCanvas : MonoBehaviour
{
    [SerializeField] GameObject _panel;
    [SerializeField] Slider _slider;

    EnemyBase _enemyBase;
    Canvas _canvas;
    
    void Start()
    {
        _enemyBase = transform.parent.GetComponent<EnemyBase>();
        _slider.maxValue = _enemyBase.MaxHP;
        _canvas = gameObject.GetComponent<Canvas>();
        _panel.SetActive(false);
    }

    void Update()
    {
        if (_enemyBase.HP >= _enemyBase.MaxHP) return;
        else
        {
            if (!_panel.activeSelf) _panel.SetActive(true);
            _slider.value = _enemyBase.HP;
        }
        _canvas.transform.rotation = Camera.main.transform.rotation;
    }
}
