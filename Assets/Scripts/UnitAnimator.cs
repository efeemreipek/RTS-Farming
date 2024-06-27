using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAnimator : MonoBehaviour
{
    private int animIsWalkingHash;
    private int animMineHash;

    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();

        animIsWalkingHash = Animator.StringToHash("isWalking");
        animMineHash = Animator.StringToHash("Mine");
    }

    public void SetIsWalking(bool cond)
    {
        _animator.SetBool(animIsWalkingHash, cond);
    }

    public void TriggerMine()
    {
        _animator.SetTrigger(animMineHash);
    }
}
