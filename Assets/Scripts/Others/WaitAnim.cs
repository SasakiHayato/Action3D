using UnityEngine;

public class WaitAnim : CustomYieldInstruction
{
    Animator _anim;
    int _setPash = 0;

    bool _tryGet = true;

    public override bool keepWaiting
    {
        get
        {
            if (!_tryGet)
            {
                Debug.Log("Nothing Animator");
                return false;
            }

            var info = _anim.GetCurrentAnimatorStateInfo(0);
            int currentPash = _anim.GetCurrentAnimatorStateInfo(0).fullPathHash;
            return info.normalizedTime < 1 && currentPash == _setPash;
        }
    }

    public WaitAnim(Animator anim)
    {
        if (anim == null)
        {
            _tryGet = false;
            return;
        }

        _anim = anim;
        _setPash = anim.GetCurrentAnimatorStateInfo(0).fullPathHash;
    }
}
