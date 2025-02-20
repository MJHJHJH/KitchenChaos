using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimatior
{
    private Animator animator;

    public PlayerAnimatior(Animator _animator)
    {
        this.animator = _animator;
    }

    private const string Is_Walking = "IsWalking";
    private bool nowState = false;
    public void SetWalking(bool isWalking)
    {
        // if (nowState != isWalking)
        // {

        // }
        animator.SetBool(Is_Walking, isWalking);
        nowState = isWalking;
    }
}
