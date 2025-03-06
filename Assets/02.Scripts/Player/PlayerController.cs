using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
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
    public LayerMask groundLayer;

    [Header("LookInfo")]
    public Transform cameraContainer;
    public Camera playerCam;
    private float camRotX;
    private float camRotY;
    private float minXLook;
    private float maxXLook;
    Quaternion targetRot;

    float rayLength = 0.1f;


    //민감도가 낮을수록 무빙이 느려짐
    private float CamSensitivity = 0.1f;

    private void Awake()
    {
        minXLook = -45f;
        maxXLook = 45f;
        jumpPower = 5f;
    }

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        aniHandler = GetComponent<PlayerAnimationHandler>();
        playerCam = Camera.main;
        cameraContainer = playerCam.transform.parent;
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
            CharacterManager.Instance.state = AnimationState.Walk;
            moveInput = context.ReadValue<Vector2>();
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            moveInput = Vector2.zero;
            CharacterManager.Instance.state = AnimationState.Idle;
        }
    }

    // 점프
    private void Jump()
    {
        _rigidbody.AddForce(jumpPower * Vector3.up, ForceMode.Impulse);
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
        if(context.phase == InputActionPhase.Started && IsGround())
        {
            Debug.Log("짬푸");
            Jump();
        }
    }

    bool IsGround()
    {
        Ray[] rays = new Ray[4]
        {
            new Ray(transform.position + (transform.forward* 0.2f) + transform.up * 0.01f, Vector3.down),
            new Ray(-transform.position + (transform.forward* 0.2f) + transform.up * 0.01f, Vector3.down),
            new Ray(transform.position + (transform.right* 0.2f) + transform.up * 0.01f, Vector3.down),
            new Ray(transform.position + (-transform.right* 0.2f) + transform.up * 0.01f, Vector3.down)
        };

        for (int i = 0; i < rays.Length; i++) 
        {
            if(Physics.Raycast(rays[i], 0.1f, groundLayer))
            {
                CharacterManager.Instance.state = AnimationState.Jump;
                return true;
            }
        }
        CharacterManager.Instance.state = AnimationState.Idle;
        return false;
    }


    // TODO : 임시카메라!!!! 나중에 꼭 수정할것
    void CameraLook()
    {
        //카메라를 커서의 x만큼 회전시킨다.
        camRotX += mouseDelta.y * CamSensitivity; //y축을 회전해야 x가 회전함
        camRotX = Mathf.Clamp(camRotX, minXLook, maxXLook);
        cameraContainer.eulerAngles = new Vector3(-camRotX, 0, 0);
         
        transform.eulerAngles += new Vector3(0, mouseDelta.x * CamSensitivity); //x축을 회전해야 y가 회전함
    }

    ////뒷통수 카메라 회전
    //void CameraLook()
    //{
    //    camRotX += mouseDelta.y * CamSensitivity;
    //    camRotY += mouseDelta.x * CamSensitivity;

    //    camRotX = Mathf.Clamp(camRotX, -90, 90);
    //    camRotY = Mathf.Clamp(camRotY, -45, 45);
    //    targetRot = Quaternion.Euler(-camRotX, camRotY, 0);
    //    playerCam.transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, 0.5f);
    //    playerCam.transform.parent.gameObject.transform.position = new Vector3(transform.position.x, 0, transform.position.z);
    //}

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
    //필요할때만 쓰기
    IEnumerator Down()
    {
        yield return new WaitForSeconds(3f);
        _rigidbody.AddForce(Vector3.down * jumpPower, ForceMode.Impulse);
        Debug.Log("내려주는중~");
    }
}