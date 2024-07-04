using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAnimator : MonoBehaviour
{
    private int animIsWalkingHash;
    private int animMineHash;

    private Animator _animator;
    private AnimatorStateInfo _animatorStateInfo;

    private void Awake()
    {
        _animator = GetComponent<Animator>();

        animIsWalkingHash = Animator.StringToHash("isWalking");
        animMineHash = Animator.StringToHash("Mine");

        _animatorStateInfo = _animator.GetCurrentAnimatorStateInfo(0);
    }

    public void SetIsWalking(bool cond)
    {
        if (cond == _animator.GetBool(animIsWalkingHash)) return;
        _animator.SetBool(animIsWalkingHash, cond);
    }

    public void TriggerMine()
    {
        _animator.SetTrigger(animMineHash);
    }

    public float GetCurrentAnimationLength()
    {
        return _animatorStateInfo.length;
    }
}
