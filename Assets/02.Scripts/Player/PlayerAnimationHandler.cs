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

    public AnimationState state;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void SetAnimator(AnimationState state)
    {
        switch (state) 
        {
            case AnimationState.Jump:
                animator.SetTrigger(jump);
                CharacterManager.Instance.isJump = false;
                break;
            case AnimationState.Walk:
                animator.SetBool(walk, CharacterManager.Instance.isWalk);
                break;
            default:
                CharacterManager.Instance.isJump = false;
                CharacterManager.Instance.isWalk = false;
                animator.SetBool(walk, CharacterManager.Instance.isWalk);
                break;
        }
    }
}
