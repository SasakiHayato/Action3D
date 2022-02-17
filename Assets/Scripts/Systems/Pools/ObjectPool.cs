using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ObjectPoolの管理クラス
/// </summary>
/// <typeparam name="T"></typeparam>

public class ObjectPool<T> where T : Object, IPool
{
    Transform _parent;
    int _setCount;
    List<T> _pool = new List<T>();
    T _getT;

    /// <summary>
    /// Poolさせる対象の設定
    /// </summary>
    /// <param name="type">IPool</param>
    /// <param name="parent">Poolの保持者</param>
    /// <param name="setCount">CreateCount</param>
    public void SetUp(T type, Transform parent = null, int setCount = 30)
    {
        _getT = type;
        _parent = parent;
        _setCount = setCount;

        Create();
    }

    void Create()
    {
        for (int i = 0; i < _setCount; i++)
        {
            T t = Object.Instantiate(_getT, _parent);
            t.SetUp(_parent);
            _pool.Add(t);
        }
    }

    /// <summary>
    /// 使われてないPoolの申請
    /// </summary>
    /// <returns>対象Object</returns>
    public T Respons()
    {
        foreach (T t in _pool)
            if (!t.IsUse) return t;

        Create();
        return Respons();
    }
}
