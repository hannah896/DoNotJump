using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class PlayerController : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private PlayerAnimationHandler aniHandler;

    [Header("MoveInfo")]
    public float jumpPower;
    public float moveSpeed;
    private Vector2 moveInput;
    private Vector2 mouseDelta;

    [Header("LookInfo")]
    public Camera playerCam;
    private float camRotX;
    private float camRotY;
    //�ΰ����� �������� ������ ������
    private float CamSensitivity = 0.1f;
    float rayLength = 3f;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        aniHandler = GetComponent<PlayerAnimationHandler>();
        playerCam = Camera.main;
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void LateUpdate()
    {
        CameraLook();
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

        transform.forward = dir;
        aniHandler.SetAnimator(AnimationState.Walk);
        //��¥�� �����̰� ����� �κ�
        _rigidbody.velocity = dir;
    }

    //������ ���� �о���� ����
    public void OnMove(InputAction.CallbackContext context)
    {
        //�����̴� ������ ��
        if (context.phase == InputActionPhase.Performed)
        {
            CharacterManager.Instance.isWalk = true;
            moveInput = context.ReadValue<Vector2>();
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            moveInput = Vector2.zero;
            CharacterManager.Instance.isWalk = false;
        }
    }

    // ����
    private void Jump()
    {
        jumpPower = 0.5f;
        _rigidbody.AddForce(jumpPower * Vector3.up, ForceMode.Impulse);
        CharacterManager.Instance.isJump = false;
    }

    //// ���� ����
    //private void SuperJump()
    //{
    //    jumpPower = 0.5f;
    //    _rigidbody.AddForce(10 * jumpPower * Vector3.up, ForceMode.Impulse);
    //}

    //�������� �޾ƿ��� ����

    //���� �Է��� �޾ƿ��� ����
    public void OnJump(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Started)
        {
            CharacterManager.Instance.isJump = true;
            Debug.Log("«Ǫ");
            aniHandler.SetAnimator(AnimationState.Jump);
            Jump();
        }
    }

    //����� ī�޶� ȸ��
    void CameraLook()
    {
        camRotX += mouseDelta.y * CamSensitivity;
        camRotY += mouseDelta.x * CamSensitivity;

        camRotX = Mathf.Clamp(camRotX, -45, 45);
        camRotY = Mathf.Clamp(camRotY, -45, 45);
        Quaternion targetRot = Quaternion.Euler(-camRotX, camRotY, 0);
        playerCam.transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, 0.5f);
        playerCam.transform.parent.gameObject.transform.position = transform.position;
    }

    //���콺 ���� �޾ƿ���
    public void OnLook(InputAction.CallbackContext context)
    {
        mouseDelta = context.ReadValue<Vector2>();

    }

    //�ٶ� ������ ������Ʈ ����ĳ��Ʈ
    void Interact()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        Physics.Raycast(ray, out hit, rayLength);
    }
}