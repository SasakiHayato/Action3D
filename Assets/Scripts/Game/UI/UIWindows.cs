using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// �eUIType��UI���̊��N���X
/// </summary>

[System.Serializable]
public abstract class UIWindowParent
{
    [System.Serializable]
    public abstract class UIWindowChild
    {
        public int ID { get; set; }
        public Image ParentPanel { get; set; }

        public abstract void SetUp();
        public abstract void UpDate();
        public abstract void CallBack(object[] data);
    }

    [SerializeField] Image _targetPanel;
    [SerializeField] UIType _type;
    [SerializeReference, SubclassSelector]
    List<UIWindowChild> _windows = new List<UIWindowChild>();

    public Image GetPanel => _targetPanel;
    public Image SetPanel { set { _targetPanel = value; } }
    public UIType GetUIType { get => _type; }

    /// <summary>
    /// UI���̏�����
    /// </summary>
    public virtual void SetUp()
    {
        int setID = 1;
        foreach (UIWindowChild ui in _windows)
        {
            ui.ID = setID;
            ui.ParentPanel = _targetPanel;
            ui.SetUp();
            setID++;
        }
    }

    /// <summary>
    /// ���UI���̍X�V��������
    /// </summary>
    public virtual void UpDate()
    {
        foreach (UIWindowChild ui in _windows)
        {
            ui.UpDate();
        }
    }

    /// <summary>
    /// �C�ӂ̃^�C�~���O�ł�UI���̍X�V
    /// </summary>
    /// <param name="id">UIType�ɑ΂���X�V������UI����ID</param>
    /// <param name="data">�X�V����ۂɓ`���鏔�X��Data�Q</param>
    public virtual void CallBack(int id, object[] data)
    {
        foreach (UIWindowChild ui in _windows)
        {
            if (ui.ID == id)
            {
                ui.CallBack(data);
            }
        }
    }
}