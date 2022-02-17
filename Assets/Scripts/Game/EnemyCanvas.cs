/// <summary>
/// Enemyの上部に表示するCanvasの管理クラス
/// </summary>

using UnityEngine;
using UnityEngine.UI;

public class EnemyCanvas : MonoBehaviour
{
    [SerializeField] GameObject _panel;
    [SerializeField] Slider _slider;
    [SerializeField] Text _text;

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

    /// <summary>
    /// Fieldのレベルが上がった際のUI情報の書き換え
    /// </summary>
    /// <param name="hp">対象の変化</param>
    /// <param name="level"></param>
    public void UpdateState(int hp, int level)
    {
        _slider.maxValue = hp;
        _text.text = $"Level:{level.ToString("000")}";
    }
}
