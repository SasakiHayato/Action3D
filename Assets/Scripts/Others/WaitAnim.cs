using UnityEngine;

public class WaitAnim : CustomYieldInstruction
{
    Animator _anim;
    int _setPash = 0;

    public override bool keepWaiting
    {
        get
        {
            var info = _anim.GetCurrentAnimatorStateInfo(0);
            int currentPash = _anim.GetCurrentAnimatorStateInfo(0).fullPathHash;
            return info.normalizedTime < 1 && currentPash == _setPash;
        }
    }

    public WaitAnim(Animator anim)
    {
        _anim = anim;
        _setPash = anim.GetCurrentAnimatorStateInfo(0).fullPathHash;
    }
}
