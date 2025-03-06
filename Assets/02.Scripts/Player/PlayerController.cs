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


    //�ΰ����� �������� ������ ������
    private float CamSensitivity = 0.1f;

    private void Awake()
    {
        minXLook = -45f;
        maxXLook = 45f;
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

    //������ ����
    private void Move()
    {
        //�յ��� �������� w, s�� ���̹Ƿ� y��, �翷�� �������� a, d�� ���̹Ƿ� x��
        Vector3 dir = transform.forward * moveInput.y + transform.right * moveInput.x;
        //�Ÿ� = �ӵ� * ����
        dir *= moveSpeed;
        //dir�� y���� ���� 0�̱� ������ y���� �����;� ��
        dir.y = _rigidbody.velocity.y;
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
            CharacterManager.Instance.state = AnimationState.Walk;
            moveInput = context.ReadValue<Vector2>();
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            moveInput = Vector2.zero;
            CharacterManager.Instance.state = AnimationState.Idle;
        }
    }

    // ����
    private void Jump()
    {
        jumpPower = 0.5f;
        _rigidbody.AddForce(jumpPower * Vector3.up, ForceMode.Impulse);
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
        if(context.phase == InputActionPhase.Started && IsGround())
        {
            Debug.Log("«Ǫ");
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


    // TODO : �ӽ�ī�޶�!!!! ���߿� �� �����Ұ�
    void CameraLook()
    {
        //ī�޶� Ŀ���� x��ŭ ȸ����Ų��.
        camRotX += mouseDelta.y * CamSensitivity; //y���� ȸ���ؾ� x�� ȸ����
        camRotX = Mathf.Clamp(camRotX, minXLook, maxXLook);
        cameraContainer.eulerAngles = new Vector3(-camRotX, 0, 0);
         
        transform.eulerAngles += new Vector3(0, mouseDelta.x * CamSensitivity); //x���� ȸ���ؾ� y�� ȸ����
    }

    ////����� ī�޶� ȸ��
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