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
    //민감도가 낮을수록 무빙이 느려짐
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

    //움직임 구현
    private void Move()
    {
        //앞뒤의 움직임은 w, s의 값이므로 y값, 양옆의 움직임은 a, d의 값이므로 x값
        Vector3 dir = transform.forward * moveInput.y + transform.right * moveInput.x;
        //거리 = 속도 * 방향
        dir *= moveSpeed;
        //dir의 y값은 현재 0이기 때문에 y값을 가져와야 함
        dir.y = _rigidbody.velocity.y;

        transform.forward = dir;
        aniHandler.SetAnimator(AnimationState.Walk);
        //진짜로 움직이게 만드는 부분
        _rigidbody.velocity = dir;
    }

    //움직임 값을 읽어오는 역할
    public void OnMove(InputAction.CallbackContext context)
    {
        //움직이는 상태일 때
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

    // 점프
    private void Jump()
    {
        jumpPower = 0.5f;
        _rigidbody.AddForce(jumpPower * Vector3.up, ForceMode.Impulse);
        CharacterManager.Instance.isJump = false;
    }

    //// 슈퍼 점프
    //private void SuperJump()
    //{
    //    jumpPower = 0.5f;
    //    _rigidbody.AddForce(10 * jumpPower * Vector3.up, ForceMode.Impulse);
    //}

    //점프여부 받아오는 역할

    //점프 입력을 받아오는 역할
    public void OnJump(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Started)
        {
            CharacterManager.Instance.isJump = true;
            Debug.Log("짬푸");
            aniHandler.SetAnimator(AnimationState.Jump);
            Jump();
        }
    }

    //뒷통수 카메라 회전
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

    //마우스 방향 받아오기
    public void OnLook(InputAction.CallbackContext context)
    {
        mouseDelta = context.ReadValue<Vector2>();

    }

    //바라본 방향의 오브젝트 레이캐스트
    void Interact()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        Physics.Raycast(ray, out hit, rayLength);
    }
}