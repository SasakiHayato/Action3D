using UnityEngine;

/// <summary>
/// 偏差射撃の計算
/// </summary>

public struct Deviation
{
    // tPos => Player
    // tBeforePos => Playerの1F.later
    // myPos => Enemy
    // speed => Bulletの速度
    public Vector3 DeviationDir(Vector3 tPos, Vector3 myPos, Vector3 tBeforePos, float speed)
    {
        float fps = 1 / Time.deltaTime;
        // Bulletの到達時間　秒
        float t = Vector3.Distance(myPos, tPos) / speed;
        
        // Playerの方向
        Vector3 targetDir = (tPos - tBeforePos).normalized;

        // Playerの1秒の速度
        
        float tSpeed = Vector3.Distance(tPos, tBeforePos) * fps;
        
        // 予測位置
        Vector3 predictPos = tPos + (targetDir * tSpeed) * t;
        if (predictPos == tPos) return (predictPos - myPos).normalized;

        return (predictPos - myPos).normalized;
    }
}
