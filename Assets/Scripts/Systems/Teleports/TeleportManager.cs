using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Telepotor���Ǘ�����N���X
/// </summary>

public class TeleportManager : MonoBehaviour
{
    private static TeleportManager _instance = null;
    public static TeleportManager Instnace => _instance;

    List<Teleportor> _teleportors;

    private void Awake()
    {
        _instance = this;
    }

    public void SetUp(List<FieldManager.SpawnData> spawnDatas)
    {
        _teleportors = new List<Teleportor>();
        GameObject obj = new GameObject("TeleportManager");

        foreach (FieldManager.SpawnData data in spawnDatas)
        {
            Teleportor t = new GameObject($"Teleportor No:{data.ID}").AddComponent<Teleportor>();
            t.SetUp(data);

            t.transform.SetParent(obj.transform);
            _teleportors.Add(t);
        }
    }

    /// <summary>
    /// �e���|�[�g�̐\��
    /// </summary>
    /// <param name="id">�ΏۂƂȂ�ID</param>
    /// <param name="target">���N�G�X�g����Object</param>
    public void RequestTeleport(int id, Transform target)
    {
        foreach (Teleportor t in _teleportors)
        {
            if (t.ID == id)
            {
                target.position = t.Position;
                return;
            }
        }
    }
}
