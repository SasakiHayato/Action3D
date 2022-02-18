using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Telepotorを管理するクラス
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
    /// テレポートの申請
    /// </summary>
    /// <param name="id">対象となるID</param>
    /// <param name="target">リクエストしたObject</param>
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
