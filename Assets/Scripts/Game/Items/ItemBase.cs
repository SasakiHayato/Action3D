using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Item�̊��N���X
/// </summary>

public class ItemBase : MonoBehaviour
{
    public enum Usetiming
    {
        Input,
        OnTrigger,
    }

    [System.Serializable]
    public class ItemData
    {
        public string Name;
        public Usetiming Usetiming;
        public string MSG;
    }

    [SerializeField] ItemData _itemData;
    public ItemData Data => _itemData;

    public virtual void Use()
    {
        if (_itemData.Usetiming == Usetiming.Input)
        {
            Debug.Log("Delete");
            ItemManager.Instance.Delete();
        }

        Destroy(gameObject);
    }

    // Note. ���ɐݒ肷��l������ꍇ�ɃI�[�o�[���C�h����B
    public virtual void SetOtherData(object[] datas) { } 

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (_itemData.Usetiming == Usetiming.OnTrigger) Use();
            else ItemManager.Instance.Save(_itemData, gameObject);
        }
    }
}
