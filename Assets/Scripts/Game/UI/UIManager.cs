using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// UI�Ɋւ�������Ǘ�����N���X
/// </summary>

public class UIManager : MonoBehaviour
{
    private static UIManager _instance = null;
    public static UIManager Instance => _instance;

    [SerializeReference, SubclassSelector]
    List<UIWindowParent> _windows = new List<UIWindowParent>();

    void Awake()
    {
        _instance = this;
        GameObject obj = GameObject.Find("Canvas");
        if (obj == null) obj = CreateCanvas();
        
        foreach (UIWindowParent ui in Instance._windows)
        {
            GameObject parent = Instantiate(ui.GetPanel.gameObject);
            parent.transform.SetParent(obj.transform);
            parent.transform.position = obj.transform.position;
            parent.transform.localScale = Vector3.one;

            ui.SetPanel = parent.GetComponent<Image>();
        }

        Inputter.Instance.Inputs.UI.Check.started += context => IsSelectButton();
    }

    void IsSelectButton()
    {
        if (GameManager.Option.Open != GameManager.Instance.OptionState) return;
        GamePadButtonEvents.Instance.IsSelected();
    }

    GameObject CreateCanvas()
    {
        GameObject canvas = new GameObject("Canvas");
        canvas.AddComponent<Canvas>();

        return canvas;
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

        if (GameManager.Instance.OptionState == GameManager.Option.Open)
        {
            GamePadButtonEvents.Instance.Select();
            GamePadButtonEvents.Instance.SelectChangeScale(Vector2.one, Vector2.one * 1.2f);
        }
    }

    /// <summary>
    /// �X�V���ꂽ�Q�[���̒l�̏����e�����ɒm�点��
    /// </summary>
    /// <param name="type"></param>
    /// <param name="id"></param>
    /// <param name="data"></param>
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

    /// <summary>
    /// �w�肳�ꂽ�e�ƂȂ�Panel�̕\��
    /// </summary>
    /// <param name="type">UIType</param>
    /// <param name="active">�\���A��\��</param>
    public static void SetActivePanel(UIType type, bool active)
    {
        foreach (UIWindowParent ui in Instance._windows)
        {
            if (ui.GetUIType == type)
            {
                ui.GetPanel.gameObject.SetActive(active);
                return;
            }
        }
    }
}
