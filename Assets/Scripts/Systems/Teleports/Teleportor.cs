using UnityEngine;

/// <summary>
/// 各テレポートのデータクラス
/// </summary>

public class Teleportor : MonoBehaviour
{
    public Vector3 Position { get; private set; } = Vector3.zero;
    public int ID { get; private set; }

    public void SetUp(FieldManager.SpawnData spawnData)
    {
        transform.position = spawnData.Point.position;

        Position = spawnData.Point.position;
        ID = spawnData.ID;
    }
}
