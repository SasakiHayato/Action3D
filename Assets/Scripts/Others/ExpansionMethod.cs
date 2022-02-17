using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 拡張メソッド達
/// </summary>

public static class CharacterHelper
{
    public static void ChangePos(this CharacterController character, Vector3 nextPos, GameObject t)
    {
        character.enabled = false;
        t.transform.position = nextPos;
        character.enabled = true;
    }

    public static void ChangeLocalPos(this CharacterController character, Vector3 nextPos, GameObject t)
    {
        character.enabled = false;
        t.transform.localPosition = nextPos;
        character.enabled = true;
    }
}

public static class TranceformHelper
{
    public static RectTransform GetRect(this GameObject obj)
    {
        return obj.GetComponent<RectTransform>();
    }
}

public static class GameObjectHlper
{
    public static void RemoveComponentAll(this GameObject obj)
    {
        List<Behaviour> gets = new List<Behaviour>(obj.GetComponents<Behaviour>());
        gets.ForEach(c => c.enabled = false);
    }
}