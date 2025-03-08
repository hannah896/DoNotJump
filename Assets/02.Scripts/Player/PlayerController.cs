using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

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
    private float CamSensitivity = 0.1f; //민감도가 낮을수록 무빙이 느려짐
    private float camRotX;
    private float camRotY;
    private float minXLook;
    private float maxXLook;
    public LayerMask InteractableLayer;
    Quaternion targetRot;

    [Header("PhysicsInfo")]
    public AnimationState playerState;
    public Vector3 playerVelocity;
    private Vector3 originGravity;
    private Vector3 jumpGravity;
    public float gravity;

    [Header("RayInfo")]
    private Vector2 mousePos;
    float rayLength = 0.1f;
    private Ray drawingRay;
    ItemObject item;

    private void Awake()
    {
        minXLook = -45f;
        maxXLook = 45f;
        jumpPower = 1f;

        originGravity = new Vector3(0, -9.8f, 0);
        jumpGravity = originGravity * jumpPower;
    }

    private void Start()
    {
        CharacterManager.Instance.Controller = this;
        _rigidbody = GetComponent<Rigidbody>();
        aniHandler = GetComponent<PlayerAnimationHandler>();
        playerCam = Camera.main;
        cameraContainer = playerCam.transform.parent;

        //Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        playerState = CharacterManager.Instance.state;
        playerVelocity = _rigidbody.GetPointVelocity(transform.position);
        gravity = Physics.gravity.y;
    }

    private void FixedUpdate()
    {
        Move();
        if (IsGround())
        {
            if (Mathf.Abs(playerVelocity.x) > 0.3f || Mathf.Abs(playerVelocity.z) > 0.1f)
            {
                CharacterManager.Instance.state = AnimationState.Walk;
            }
            else
            {
                CharacterManager.Instance.state = AnimationState.Idle;
            }
        }
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
        //진짜로 움직이게 만드는 부분
        _rigidbody.velocity = dir;
    }

    //움직임 값을 읽어오는 역할
    public void OnMove(InputAction.CallbackContext context)
    {
        //움직이는 상태일 때
        if (context.phase == InputActionPhase.Performed)
        {
            Debug.Log("어디서나 당당하게 걷기");
            moveInput = context.ReadValue<Vector2>();
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            moveInput = Vector2.zero;
        }
    }

    // 점프
    public void Jump(float jumpPower)
    {
        _rigidbody.AddForce(jumpPower * Vector3.up, ForceMode.Impulse);
        StartCoroutine(PushDown());
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
            Jump(jumpPower);
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
            if(Physics.Raycast(rays[i], 0.05f, groundLayer))
            {
                CharacterManager.Instance.state = AnimationState.Idle;
                return true;
            }
        }
        Physics.gravity = originGravity;
        CharacterManager.Instance.state = AnimationState.Jump;
        
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
    public void OnInteract(InputAction.CallbackContext context)
    {
        if (Interact(context.ReadValue<Vector2>()))
        {
            UIManager.Instance.Display(item.itemInfo);
        }
        else
        {
            UIManager.Instance.Display();
        }
    }

    //상호작용하기
    private bool Interact()
    {
        Ray ray = Camera.main.ScreenPointToRay(mousePos);
        drawingRay = ray;
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, InteractableLayer) &&
            hit.collider.gameObject.TryGetComponent<ItemObject>(out item) == true)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    //아이템 사용
    public void OnUse(InputAction.CallbackContext context)
    {
        if (!Interact()) return;
        CharacterManager.Instance.Condition.UseItem(item.itemInfo);
        Destroy(item.gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red; // 기즈모 색상 설정

        Ray[] rays = new Ray[4]
        {
        new Ray(transform.position + (transform.forward * 0.2f) + Vector3.up * 0.1f, Vector3.down),
        new Ray(transform.position + (-transform.forward * 0.2f) + Vector3.up * 0.1f, Vector3.down),
        new Ray(transform.position + (transform.right * 0.2f) + Vector3.up * 0.1f, Vector3.down),
        new Ray(transform.position + (-transform.right * 0.2f) + Vector3.up * 0.1f, Vector3.down)
        };

        foreach (var ray in rays)
        {
            Gizmos.DrawLine(ray.origin, ray.origin + ray.direction * 0.05f); // 시작점 + 방향 * 길이
        }

        Gizmos.DrawLine(drawingRay.origin, drawingRay.origin + drawingRay.direction * 10f);
    }

    IEnumerator PushDown()
    {
        if (_rigidbody.velocity.y < 0.19f)
        {
            Physics.gravity = jumpGravity;
            Debug.Log("내려드렸습니다");
            yield break;
        }
        else
        {
            yield return null;
        }
    }
}