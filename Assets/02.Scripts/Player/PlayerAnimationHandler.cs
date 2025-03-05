using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
public enum AnimationState
{
    Idle,
    Walk,
    Jump
}
public class PlayerAnimationHandler : MonoBehaviour
{
    Animator animator;

    private int walk = Animator.StringToHash("isWalk");
    private int jump = Animator.StringToHash("Jump");

    private bool iswalk;
    private bool isjump;

    public AnimationState state;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    void SetAnimator(AnimationState state)
    {
        switch (state) 
        {
            case AnimationState.Idle:
                iswalk = false;
                break;
            case AnimationState.Jump:
                isjump = true;
                break;
            case AnimationState.Walk: 
                iswalk = true;
                break;
        }

    }
}
