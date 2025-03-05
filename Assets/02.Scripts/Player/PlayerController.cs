using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class PlayerController : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private PlayerAnimationHandler _aniHandler;

    private float jumpPower;
    public float moveSpeed;
    private Vector2 moveInput;
    private Vector2 mouseDelta;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _aniHandler = GetComponent<PlayerAnimationHandler>();
    }

    private void FixedUpdate()
    {
        Move();
    }

    //움직임 구현
    private void Move()
    {
        //앞뒤의 움직임은 w, s의 값이므로 y값, 양옆의 움직임은 a, d의 값이므로 x값
        Vector3 dir = transform.forward * moveInput.y + transform.right * moveInput.x;
        //거리 = 속도 * 방향
        dir *= moveSpeed;
        //dir의 y값은 현재 0이기 때문에 y값을 가져와야 함
        dir.y = _rigidbody.velocity.y;

        //진짜로 움직이게 만드는 부분
        _rigidbody.velocity = dir;
    }

    //움직임 값을 읽어오는 역할
    public void OnMove(InputAction.CallbackContext context)
    {
        //움직이는 상태일 때
        if (context.phase == InputActionPhase.Performed)
        {
            moveInput = context.ReadValue<Vector2>();
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            moveInput = Vector2.zero;
        }
    }

    // 점프
    private void Jump()
    {

    }
}