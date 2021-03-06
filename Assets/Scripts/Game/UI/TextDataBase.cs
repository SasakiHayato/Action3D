using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Textの表示を設定するクラス
/// </summary>

[CreateAssetMenu(fileName = "TextDatas")]
public class TextDataBase : ScriptableObject
{
    [SerializeField] List<TextData> _datas;
    public List<TextData> GetDatas => _datas;

    [System.Serializable]
    public class TextData
    {
        public int ID;
        public string Text;
    }
}

