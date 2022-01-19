using UnityEngine;

public struct Deviation
{
    const float FleamRate = 60;

    public Vector3 DeviationDir(Vector3 tPos, Vector3 myPos, Vector3 tBeforePos, float speed)
    {
        float distance = Vector3.Distance(myPos, tPos);
        // Bullet‚Ì“ž’BŽžŠÔ
        float t = distance / speed;

        Vector3 targetDir = (tPos - tBeforePos).normalized;
        
        float tSpeed = Vector3.Distance(tPos, tBeforePos) * FleamRate;
        // —\‘ªˆÊ’u
        Vector3 predictPos = tPos + (targetDir * tSpeed) * t;
        Vector3 afterPos = predictPos - myPos;

        return afterPos.normalized;
    }
}
