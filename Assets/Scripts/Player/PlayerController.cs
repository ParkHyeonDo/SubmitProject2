using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    public float jumpPower;
    
    private Vector2 curMovementInput;
    public LayerMask groundLayerMask;

    [Header("Look")]
    public Transform cameraContainer;
    public float minXLook;
    public float maxXLook;
    private float camCurXRot;
    public float lookSensitivity;
    private Vector2 mouseDelta;
    public bool canLook = true;

    [Header("Skill")]
    public int skillmana;
    public float boostAmount;
    public int boostTime;
    private bool isSkiiled = false;
    private float hasTime;

    public Action inventory;
    public Action option; //## 꾸준실습
    

    private Rigidbody _rigidbody;

    // Start is called before the first frame update
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Move();
    }

    private void Update()
    {
        DuringSkill();
        
    }

    

    private void LateUpdate()
    {
        if (canLook)
        {
            CameraLook();
        }

    }


    private void DuringSkill()
    {
        if (isSkiiled)
        {
            hasTime += Time.deltaTime;
            if (hasTime > boostTime) 
            {
                moveSpeed /= boostAmount;
                isSkiiled = false;
            }
        }
    }

    private void Move()
    {
       
        Vector3 dir = transform.forward * curMovementInput.y + transform.right * curMovementInput.x;
        dir *= moveSpeed;
        dir.y = _rigidbody.velocity.y;

        _rigidbody.velocity = dir;
    }

    void CameraLook() 
    {
        camCurXRot += mouseDelta.y * lookSensitivity; //좌우를 보는건 Y축에 대비하여 회전이기 때문에 y의 값을 X에 넣는것
        camCurXRot = Mathf.Clamp(camCurXRot, minXLook, maxXLook);
        cameraContainer.localEulerAngles = new Vector3(-camCurXRot, 0, 0); // rotation을 돌려볼경우, 음수여야 위쪽을 봄

        transform.eulerAngles += new Vector3(0, mouseDelta.x * lookSensitivity , 0); //위아래를 보는건 X축에 대비하여 회전이기때문에 x에 y값
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            curMovementInput = context.ReadValue<Vector2>();
            
        }
        else if (context.phase == InputActionPhase.Canceled) 
        {
            curMovementInput = Vector2.zero;
            
        }
        
    }

    public void OnLook(InputAction.CallbackContext context) 
    {
        mouseDelta = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context) 
    {
        if (context.phase == InputActionPhase.Started && IsGrounded()) 
        {
            _rigidbody.AddForce(Vector2.up * jumpPower , ForceMode.Impulse);
        }
    }

    bool IsGrounded() 
    {
        Ray[] rays = new Ray[4]
        {
            new Ray(transform.position + (transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (-transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (-transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down)
        };

        for (int i = 0; i < rays.Length; i++) 
        {
            if (Physics.Raycast(rays[i], 0.1f, groundLayerMask))
            {
                return true;
            
            }
        }

        return false;
    }

    public void OnInventory(InputAction.CallbackContext context) 
    {
        if (context.phase == InputActionPhase.Started) 
        {
            inventory?.Invoke();
            ToggleCursor();
        }
    }

    void ToggleCursor() 
    {
        bool toggle = Cursor.lockState == CursorLockMode.Locked; // Cursor.lockState == 잠겨있으면 1반환
        Cursor.lockState = toggle ? CursorLockMode.None : CursorLockMode.Locked; // 잠겨있으면 None으로 풀고 안잠겨있으면 Locked
        canLook = !toggle;
    }

    public void OnOption(InputAction.CallbackContext context) //## 꾸준실습
    {
        Debug.Log("옵션");
        if (context.phase == InputActionPhase.Started) 
        {
            option?.Invoke();
            ToggleCursor();
        }
    }

    public void OnSkill(InputAction.CallbackContext context) 
    {
        if (context.phase == InputActionPhase.Started && !isSkiiled )  
        {
            Debug.Log("스킬" + isSkiiled);
            moveSpeed *= boostAmount;
            CharacterManager.Instance.Player.condition.UseMana(skillmana);
            isSkiiled = true;
        }
    }
}
