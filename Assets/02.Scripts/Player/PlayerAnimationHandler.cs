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

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        SetAnimator(CharacterManager.Instance.state);
    }

    public void SetAnimator(AnimationState state)
    {
        switch (state) 
        {
            case AnimationState.Jump:
                animator.SetBool(walk, false);
                animator.SetTrigger(jump);
                break;
            case AnimationState.Walk:
                animator.SetBool(walk, true);
                break;
            case AnimationState.Idle:
                animator.SetBool(walk, false);
                animator.SetBool(jump, false);
                break;
        }
    }
}
