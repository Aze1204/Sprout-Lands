using System.Collections;
using Sprout.Inventory;
using UnityEngine;
// ���������ڹ����ɫ�ƶ���
// �����ߣ�Aze
// ����ʱ�䣺2024-12-29
public class PlayerControl : MonoBehaviour
{
    private Rigidbody2D rb;
    private float inputX;
    private float inputY;
    public float speed;

    private Vector2 moveInput;

    private Animator animator;

    private bool isMoveing;

    private bool inputDisable;
    
    private float mouseX;
    private float mouseY;

    private bool isUseTool;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        EventHandler.BeforeSceneUnloadEvent += OnBeforeSceneUnloadEvent;
        EventHandler.AfterSceneUnloadEvent += OnAfterSceneUnloadEvent;
        EventHandler.MoveToPosition += OnMoveToPosition;
        EventHandler.MouseClickedEvent += OnMouseClickedEvent;
        EventHandler.HarvestAtPlayerPosition += OnHarvestAtPlayerPosition;
    }
    
    private void OnDisable()
    {
        EventHandler.BeforeSceneUnloadEvent -= OnBeforeSceneUnloadEvent;
        EventHandler.AfterSceneUnloadEvent -= OnAfterSceneUnloadEvent;
        EventHandler.MoveToPosition -= OnMoveToPosition;
        EventHandler.MouseClickedEvent -= OnMouseClickedEvent;
        EventHandler.HarvestAtPlayerPosition -= OnHarvestAtPlayerPosition;
    }

    private void Update()
    {
        if (inputDisable == false)
            PlayerInput();
        else
            isMoveing = false;
        SwitchAnimation();
    }

    private void FixedUpdate()
    {
        if(!inputDisable)
            Movement();
    }

    private void OnBeforeSceneUnloadEvent()
    {
        inputDisable = true;
    }

    private void OnAfterSceneUnloadEvent()
    {
        inputDisable = false;
    }

    private void OnHarvestAtPlayerPosition(int id)
    {
        Sprite itemSprite = InventoryManager.Instance.GetItemDetails(id).itemIcon;
    }
    
    private void OnMoveToPosition(Vector3 targetPosition)
    {
        transform.position = targetPosition;
    }
    private void OnMouseClickedEvent(Vector3 pos, ItemDetails details)
    {
        //TODO: ���Ž�ɫ����
        if (details.itemType!=ItemType.Seed && 
            details.itemType!=ItemType.Commodity && 
            details.itemType!=ItemType.Furniture)
        {
            mouseX = pos.x - transform.position.x;
            mouseY = pos.y - transform.position.y;

            if (Mathf.Abs(mouseX)>Mathf.Abs(mouseY))
            {
                mouseY = 0;
            }
            else
            {
                mouseX = 0;
            }
            StartCoroutine(UseToolRoutine(pos,details));
        }
        else
        {
            EventHandler.CallExecuteActionAfterAnimationEvent(pos, details);
        }
        
    }

    private IEnumerator UseToolRoutine(Vector3 mouseWorldPos, ItemDetails details)
    {
        isUseTool = true;
        inputDisable = true;
        yield return null;
        //�����ת������
        animator.SetTrigger("UseTool");
        animator.SetFloat("InputX", mouseX);
        animator.SetFloat("InputY", mouseY);
        yield return new WaitForSeconds(0.5f);
        EventHandler.CallExecuteActionAfterAnimationEvent(mouseWorldPos, details);
        yield return new WaitForSeconds(0.2f);
        isUseTool = false;
        inputDisable = false;
    }
    
    private void PlayerInput()
    {
        inputX = Input.GetAxisRaw("Horizontal");
        inputY = Input.GetAxisRaw("Vertical");

        if (inputX != 0 && inputY != 0)
        {
            inputX *= 0.6f;
            inputY *= 0.6f;
        }

        //��·�ٶ�
        if (Input.GetKey(KeyCode.LeftShift))
        {
            inputX = inputX * 0.5f;
            inputY = inputY * 0.5f;
        }

        moveInput = new Vector2(inputX, inputY);

        isMoveing = moveInput != Vector2.zero;
    }

    private void Movement()
    {
        rb.MovePosition(rb.position+moveInput*speed*Time.deltaTime);
    }

    private void SwitchAnimation()
    {
        animator.SetBool("isMoving",isMoveing);
        animator.SetFloat("mouseX",mouseX);
        animator.SetFloat("mouseY",mouseY);
        if (isMoveing)
        {
            animator.SetFloat("InputX",inputX);
            animator.SetFloat("InputY",inputY);
        }
    }
}
