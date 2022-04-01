using System.Collections;
using UnityEngine;
using System;

public class PlayerAnimController : MonoBehaviour
{
    Animator _anim;
    Coroutine _coroutine;

    public bool EndAnim { get; private set; } = false;

    const float Duration = 0.1f;

    void Awake()
    {
        _anim = GetComponent<Animator>();
    }

    public void RequestAnim(string animName)
    {
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
            EndAnim = false;
        }

        if (_anim == null) _anim = GetComponent<Animator>();
        _anim.CrossFade(animName, Duration);
    }

    public void RequestAnimCallBackEvent(string animName, Action action)
    {
        if (EndAnim) return;
        EndAnim = true;
        if (_anim == null) _anim = GetComponent<Animator>();
        _anim.CrossFade(animName, Duration);
        _coroutine = StartCoroutine(AnimCoroutine(action));
    }

    IEnumerator AnimCoroutine(Action action)
    {
        yield return null;
        yield return new WaitAnim(_anim);

        action?.Invoke();
        EndAnim = false;
        _coroutine = null;
    }
}
