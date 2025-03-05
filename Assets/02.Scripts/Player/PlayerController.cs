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

    //������ ����
    private void Move()
    {
        //�յ��� �������� w, s�� ���̹Ƿ� y��, �翷�� �������� a, d�� ���̹Ƿ� x��
        Vector3 dir = transform.forward * moveInput.y + transform.right * moveInput.x;
        //�Ÿ� = �ӵ� * ����
        dir *= moveSpeed;
        //dir�� y���� ���� 0�̱� ������ y���� �����;� ��
        dir.y = _rigidbody.velocity.y;

        //��¥�� �����̰� ����� �κ�
        _rigidbody.velocity = dir;
    }

    //������ ���� �о���� ����
    public void OnMove(InputAction.CallbackContext context)
    {
        //�����̴� ������ ��
        if (context.phase == InputActionPhase.Performed)
        {
            moveInput = context.ReadValue<Vector2>();
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            moveInput = Vector2.zero;
        }
    }

    // ����
    private void Jump()
    {

    }
}