using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Text‚Ì•\¦‚ğİ’è‚·‚éƒNƒ‰ƒX
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

