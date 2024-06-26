using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAnimator : MonoBehaviour
{
    private int animIsWalkingHash;

    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();

        animIsWalkingHash = Animator.StringToHash("isWalking");
    }

    public void SetWalking(bool cond)
    {
        _animator.SetBool(animIsWalkingHash, cond);
    }
}
