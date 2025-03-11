using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using VInspector;

public class PlayerController : MonoBehaviour
{
    #region 움직임관련
    [Foldout("MoveInfo"), ShowInInspector]
    //[Header("MoveInfo")]
    public float jumpPower;
    public float moveSpeed;
    public float defaultSpeed;
    public float runSpeed;
    [ShowInInspector, ReadOnly]
    private Vector2 moveInput;
    [ShowInInspector, ReadOnly]
    private Vector2 mouseDelta;
    public LayerMask groundLayer;

    #endregion

    #region 시야정보
    [Foldout("LookInfo"), ShowInInspector]
    //[Header("LookInfo")]
    public Camera playerCam;
    [ShowInInspector]
    private float CamSensitivity = 0.1f; //민감도가 낮을수록 무빙이 느려짐
    [ShowInInspector, ReadOnly]
    private float camRotX;
    [ShowInInspector, ReadOnly]
    private float camRotY;
    [ShowInInspector, ReadOnly]
    private float minXLook;
    [ShowInInspector, ReadOnly]
    private float maxXLook;
    
    public LayerMask InteractableLayer;
    Quaternion targetRot;
    #endregion

    #region 물리정보
    [Foldout("PhysicsInfo"), ShowInInspector]
    //[Header("PhysicsInfo")]
    public AnimationState playerState;
    public Vector3 playerVelocity;
    [ShowInInspector]
    private Vector3 originGravity;
    [ShowInInspector]
    private Vector3 jumpGravity;
    public float gravity;
    #endregion

    #region 레이정보
    [Foldout("RayInfo"), ShowInInspector]
    //[Header("RayInfo")]
    private Vector2 mousePos;
    [ShowInInspector]
    private Ray drawingRay;
    public ItemObject item;
    #endregion

    #region 잡다한거
    [Foldout("Anything")]
    private Rigidbody _rigidbody;
    [ShowInInspector, ReadOnly]
    private Player _player;
    [ShowInInspector, ReadOnly]
    private Collider _collider;
    [ShowInInspector, ReadOnly]
    private CharacterManager manager;
    [ShowInInspector, ReadOnly]
    private PlayerCondition condition;
    private Transform cameraContainer;
    [ShowInInspector, ReadOnly]
    private GameObject Inventory;
    #endregion

    [Header("Camera"), ShowInInspector, ReadOnly]
    public Transform target;
    public float r;
    public float speed = 35f;
    private float playerCameraAngle;
    private float CamDir;
    private float isRot;

    [ShowInInspector, ReadOnly]

    private void OnValidate()        
    {
        _rigidbody = GetComponent<Rigidbody>();
        _collider = GetComponent<Collider>();
        _player = GetComponent<Player>();
        condition = GetComponent <PlayerCondition>();
        cameraContainer = playerCam.transform.parent;
        minXLook = -45f;
        maxXLook = 45f;
        jumpPower = 1f;
        originGravity = new Vector3(0, -9.8f, 0);

        defaultSpeed = 1f;
        runSpeed = 3f;
        Inventory = FindObjectOfType<InventoryManager>().gameObject;
    }

    private void Awake()
    {
        r = new Vector2(playerCam.transform.position.x - transform.position.x, playerCam.transform.position.z - transform.position.z).magnitude;
        playerCameraAngle = 0;
    }

    private void Start()
    {
        manager = CharacterManager.Instance;
    }

    //속도, 상태, 중력값 계산
    private void Update()
    {
        playerState = manager.state;
        playerVelocity = _rigidbody.velocity;
        gravity = Physics.gravity.y;
    }
    
    //물리 계산
    private void FixedUpdate()
    {
        Move();
        if (IsGround())
        {
            //플레이어의 속도가 
            if (Mathf.Approximately(_rigidbody.velocity.x, 0f) || Mathf.Approximately(_rigidbody.velocity.z, 0f))
            {
                manager.state = AnimationState.Idle;
            }
            else if (moveSpeed == runSpeed)
            {
                manager.state = AnimationState.Run;
                Dash();
            }
            else
            {
                manager.state = AnimationState.Walk;
            }
        }
    }
    
    //카메라 계산
    private void LateUpdate()
    {
        CameraLook();

        playerCameraAngle += Time.deltaTime * 50 * isRot;

        GetAngle();
    }

    private void GetAngle()
    {
        playerCameraAngle = playerCameraAngle % 360;
        Quaternion rotation = Quaternion.AngleAxis(playerCameraAngle, Vector3.up);
        Vector3 direction = rotation * Vector3.forward;
        Vector3 result = direction * 2 + Vector3.up * 1;
        Vector3 cameraPosition = transform.position + result;
        playerCam.transform.position = cameraPosition;

        playerCam.transform.LookAt(transform);
        Vector3 euler = playerCam.transform.eulerAngles;
        euler.x -= 20f; // 원하는 만큼 X축 회전 감소
        playerCam.transform.rotation = Quaternion.Euler(euler);
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

    // 슈퍼 점프
    private void SuperJump()
    {
        _rigidbody.AddForce(10 * jumpPower * Vector3.up, ForceMode.Impulse);
        StartCoroutine(PushDown());
    }

    //점프 입력을 받아오는 역할
    public void OnJump1(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && IsGround())
        {
            SuperJump();
        }
    }

    //점프 입력을 받아오는 역할
    public void OnJump2(InputAction.CallbackContext context)
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
                manager.state = AnimationState.Idle;
                return true;
            }
        }
        Physics.gravity = originGravity;
        manager.state = AnimationState.Jump;
        
        return false;
    }

    void CameraLook()
    {
        //카메라를 커서의 x만큼 회전시킨다.
        camRotX += mouseDelta.y * CamSensitivity; //y축을 회전해야 x가 회전함
        camRotX = Mathf.Clamp(camRotX, minXLook, maxXLook);
        cameraContainer.eulerAngles = new Vector3(-camRotX, 0, 0);

        transform.eulerAngles += new Vector3(0, mouseDelta.x * CamSensitivity); //x축을 회전해야 y가 회전함
    }

    public void OnCameraRot(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            Debug.Log(context.ReadValue<float>());
            isRot = context.ReadValue<float>();
        }
        else
        {
            isRot = 0f;
        }
    }

    //마우스 방향 받아오기
    public void OnLook(InputAction.CallbackContext context)
    {
        mouseDelta = context.ReadValue<Vector2>();
    }

    //바라본 방향의 오브젝트 레이캐스트
    public void OnInteract(InputAction.CallbackContext context)
    {
        mousePos = context.ReadValue<Vector2>();
        
        if (Interact())
        {
            manager.isInteract = true;
        }
        else
        {
            manager.isInteract = false;
        }
    }

    //상호작용하기
    private bool Interact()
    {
        Ray ray = playerCam.ScreenPointToRay(mousePos);
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
        Use();
        Disappear();
    }

    //효과적용
    public void Use()
    {
        item.Buff();
    }

    //아이템 사라지게하는 처리
    private void Disappear()
    {
        Destroy(item.gameObject);
        item = null;
        manager.isInteract = false;
    }


    //인벤토리 열기/닫기
    public void OnInventory(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            Debug.Log("가방열기");
            if (!Inventory.activeSelf)
            {
                Inventory.SetActive(true);
            }
            else
            {
                Inventory.SetActive(false);
            }
        }
    }
    
    //아이템 줍기
    public void OnPick(InputAction.CallbackContext context)
    {
        Debug.Log("아이템 주울까");
        if (context.phase == InputActionPhase.Started)
        {
            if (!manager.isInteract) return;
            Debug.Log("아이템 줍는중");
            InventoryManager.Instance.Input(item);
            Disappear();
        }
    }

    //달리기
    private void Dash()
    {
        if (_player.Dash < 3f)
        {
            Debug.Log("나 파업. 못뛰어");

            return;
        }
        else
        {
            moveSpeed = runSpeed;
            condition.SetStat(Stat.Dash, -0.5f);
        }
    }

    //달리기 입력처리
    public void OnDash(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            if (_player.Dash < 0.5f)
            {
                Debug.Log("나 파업. 못뛰어");
                moveSpeed = defaultSpeed;
                return;
            }
            moveSpeed = runSpeed;
        }
        if (context.phase == InputActionPhase.Canceled)
        {
            moveSpeed = defaultSpeed;
        }
    }
    //레이 시각화
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

    //아래로 내려주는 거
    // TODO : 코루틴을 변수로 관리
    IEnumerator PushDown()
    {
        while (true)
        {
            if (_rigidbody.velocity.y < -0.1f)
            {
                _rigidbody.velocity =  new Vector3(0, 1.1f * _rigidbody.velocity.y, 0);
                Debug.Log("내려드렸습니다");
                yield return null;
            }
        }
    }
}