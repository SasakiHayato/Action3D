using UnityEngine;

public struct Deviation
{
    public Vector3 DeviationDir(Vector3 tPos, Vector3 myPos, Vector3 tBeforePos, float speed)
    {
        float distance = Vector3.Distance(myPos, tPos);
        // Bullet‚Ì“ž’BŽžŠÔ
        float t = distance / speed;

        Vector3 tDir = (tPos - tBeforePos).normalized;

        float tSpeed = Vector3.Distance(tPos, tBeforePos) * 1 / Time.deltaTime;
        Vector3 predictPos = (tDir * tSpeed) * t;
        Vector3 afterPos = predictPos - myPos;

        return (afterPos + tPos).normalized;
    }

    public Vector3 DeviationPos(Vector3 tPos, Vector3 myPos, Vector3 tBeforePos, float speed)
    {
        float distance = Vector3.Distance(myPos, tPos);
        // Bullet‚Ì“ž’BŽžŠÔ
        float t = distance / speed;

        Vector3 tDir = (tPos - tBeforePos).normalized;

        float tSpeed = Vector3.Distance(tPos, tBeforePos) * 60;

        Vector3 predictPos = (tDir * tSpeed) * t;
        Vector3 afterPos = predictPos - myPos;

        return afterPos + tPos;
    }
}
