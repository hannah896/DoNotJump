using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using VInspector;
using static UnityEngine.Rendering.DebugUI;

public class PlayerController : MonoBehaviour
{
    #region 스크립트 내 사용 컴포넌트
    [Header("ComponentInfo"), ShowInInspector, ReadOnly]
    private Rigidbody _rigidbody; //물리 관련 사용
    
    [ShowInInspector, ReadOnly]
    private Player _player; // 현재 플레이어의 대쉬 스텟상태 체크시 호출
    
    [ShowInInspector, ReadOnly]
    private CharacterManager manager; //현재 거의 게임매니저 급의 중요한 컴포넌트임.그러나 참조시에 일일이 Instance붙이기 귀찮아서 단순화를 위해 사용
    
    [ShowInInspector, ReadOnly]
    private PlayerCondition condition; //플레이어의 상태를 변화시켜줄 때(아이템 사용과 같이) 사용되는 컴포넌트
    #endregion

    #region 움직임(걷기/뛰기)
    [Header("Camera"), ShowInInspector, ReadOnly]
    public float moveSpeed; //걷기 속도
    
    [ShowInInspector, ReadOnly]
    private Vector2 moveInput; //어느 방향으로 갈건지 입력을 저장하기 위한 방향 인풋벡터
    
    [ShowInInspector, ReadOnly]
    public float defaultSpeed; //걷기의 속도
    
    [ShowInInspector, ReadOnly]
    public float runSpeed; //달리기의 속도
    #endregion

    #region 점프
    [Header("Jump"), ShowInInspector, ReadOnly]
    public LayerMask groundLayer; //땅으로 판정할 레이어 설정
    
    [ShowInInspector, ReadOnly]
    public float jumpPower; //점프력
    #endregion

    #region 카메라 회전
    [Header("Camera"), ShowInInspector, ReadOnly]
    public Transform target; //중심점이 될 타겟, 근데 플레이어라 삭제할 수도 있음 **
    
    public Camera playerCam; //메인 카메라
    
    [ShowInInspector, ReadOnly] 
    private Vector2 mouseDelta; //delta동안 마우스움직임 변화량
    
    public float r; //원형으로 돌게될건데 그 원의 반지름
    
    public float speed = 35f; //1초에 몇 도를 돌건지의 속도
    
    [ShowInInspector, ReadOnly] 
    private float playerCameraAngle; //현재 카메라의 각도
    
    [ShowInInspector, ReadOnly] 
    private float isRot; //카메라회전의 값을 현재 value로 받고있어서(카메라가 도는 방향에 따라 +, - 로 조절하기 위해)

    [ShowInInspector, ReadOnly]
    public Coroutine FastDown; //속도가 0이 되었을때 물리적으로 내려주는 코루틴 땅에 떨어지면 그라운드에서 스탑해줌
    #endregion

    //CameraLook작동 확인후 작동하지않으면 삭제 예정 **
    #region 시야 회전 관련
    [Header("Look")] 
    private Transform cameraContainer; 
    // TODO: 애트리부트 게이지바 추가하기
    [ShowInInspector, Range(0.1f, 1f)]
    private float camSensitivity; //민감도가 낮을수록 무빙이 느려짐
    
    [ShowInInspector, ReadOnly]
    private float camRotX;
    
    [ShowInInspector, ReadOnly]
    private float camRotY;
    
    [ShowInInspector, ReadOnly]
    private float minXLook;
    
    [ShowInInspector, ReadOnly]
    private float maxXLook;

    #endregion

    #region 상호작용
    [Header("Interact")]
    public ItemObject item; //현재 상호작용중인 아이템 오브젝트(인벤토리에 넣을 수 있는 아이템), 다른 스크립트에서 접근하기 위해 사용
    public LayerMask InteractableLayer; //상호작용 오브젝트 찾는 레이어 마스크
    [ShowInInspector, ReadOnly] 
    private Vector2 mousePos; //마우스 위치값(x, y), 카메라 기준으로 마우스의 위치를 월드 좌표로 바꾸어 카메라위치에서 해당위치로 레이를 쏠 예정
    Ray drawingRay;
    #endregion

    #region 인벤토리
    [Header("Inventory"), ShowInInspector, ReadOnly]
    private GameObject Inventory; //인벤토리 validate에서 연결하기
    #endregion

    private void OnValidate()        
    {
        //component
        if (_rigidbody == null) _rigidbody = GetComponent<Rigidbody>();
        if (_player == null) _player = GetComponent<Player>();
        if (condition == null) condition = GetComponent<PlayerCondition>();
        
        //camera
        if (playerCam == null) playerCam = Camera.main;
        camSensitivity = 0.15f;

        //look
        if (playerCam != null) cameraContainer = playerCam.transform.parent;
        minXLook = -45f;
        maxXLook = 45f;

        //jump
        jumpPower = 1f;
        
        //run
        defaultSpeed = 1f;
        runSpeed = 3f;

        //inventory
        if (Inventory == null) Inventory = FindObjectOfType<InventoryManager>().gameObject;
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
    
    //물리 계산
    private void FixedUpdate()
    {
        Move();
        if (IsGround())
        {
            //플레이어의 속도가 
            if (Mathf.Approximately(_rigidbody.velocity.x, 0f) && Mathf.Approximately(_rigidbody.velocity.z, 0f))
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
        playerCameraAngle += Time.deltaTime * speed * isRot;
        GetAngle();
        CameraLook();
    }

    //카메라 회전 
    private void GetAngle()
    {
        playerCameraAngle = playerCameraAngle % 360;
        Quaternion rotation = Quaternion.AngleAxis(playerCameraAngle, Vector3.up);
        Vector3 direction = rotation * Vector3.forward;
        Vector3 result = direction * r + Vector3.up * 1;
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
        FastDown = StartCoroutine(PushDown());
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

    //땅인지 체크함
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
                if (FastDown != null)
                {
                    StopCoroutine(FastDown);
                    FastDown = null;
                }
                return true;
            }
        }
        
        manager.state = AnimationState.Jump;
        
        return false;
    }

    void CameraLook()
    {
        // 카메라를 y축으로 회전시켜서 상하 회전 처리
        camRotX += mouseDelta.y * camSensitivity; // y축을 회전해야 x가 회전함
        camRotX = Mathf.Clamp(camRotX, minXLook, maxXLook);
        cameraContainer.localRotation = Quaternion.Euler(-camRotX, 0, 0); // 카메라는 X축만 회전

        // 캐릭터를 y축으로 회전시켜서 좌우 회전 처리
        float targetYRotation = mouseDelta.x * camSensitivity;
        transform.Rotate(Vector3.up * targetYRotation); // 캐릭터는 Y축을 기준으로 회전
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
            moveSpeed = defaultSpeed;
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
    IEnumerator PushDown()
    {
        while (true)
        {
            if (_rigidbody.velocity.y < -0.1f)
            {
                _rigidbody.velocity =  new Vector3(0, 1.1f * _rigidbody.velocity.y, 0);
                Debug.Log("내려드렸습니다");
            }
            yield return null;
        }
    }
}