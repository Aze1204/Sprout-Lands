using UnityEngine;
using Event;
using UnityEngine.UI;

namespace Cursor
{
    public class CursorManager : MonoBehaviour
    {
        // 依赖组件
        private CursorVisuals cursorVisuals;
        private CursorInputHandler inputHandler;
        private CursorValidation cursorValidation;

        // 配置参数
        [SerializeField] private RectTransform cursorCanvas;
        private Camera mainCamera;
        private Vector3 mouseWorldPos;

        // 状态变量
        private bool cursorEnable;
        private bool cursorPositionValid;
        private ItemDetails currentItem;

        private void Awake()
        {
            //cursorCanvas = GameObject.FindWithTag("CursorCanvas").GetComponent<RectTransform>();
            UnityEngine.Cursor.visible = false;
            InitializeDependencies();
        }

        private void InitializeDependencies()
        {
            // 初始化子模块
            cursorVisuals = gameObject.AddComponent<CursorVisuals>();
            inputHandler = gameObject.AddComponent<CursorInputHandler>();
            cursorValidation = gameObject.AddComponent<CursorValidation>();

            // 获取必要引用
            Image cursorImg = cursorCanvas.GetChild(0).GetComponent<Image>();
            Image buildImg = cursorCanvas.GetChild(1).GetComponent<Image>();
            cursorVisuals.Initialize(cursorImg, buildImg);

            mainCamera = Camera.main;
            cursorValidation.Initialize(FindObjectOfType<Grid>(), FindObjectOfType<PlayerControl>().transform);
        }

        private void Update()
        {
            if (!cursorCanvas) return;

            UpdateCursorPosition();
            if (cursorEnable)
            {
                cursorPositionValid = cursorValidation.ValidateCursorPosition(mouseWorldPos, currentItem, out _);
                inputHandler.CheckPlayerInput(mouseWorldPos, currentItem, cursorPositionValid);
                UpdateCursorVisualState();
            }
        }

        private void UpdateCursorPosition()
        {
            mouseWorldPos = mainCamera.ScreenToWorldPoint(
                new Vector3(Input.mousePosition.x, Input.mousePosition.y, -mainCamera.transform.position.z)
            );
        }

        private void UpdateCursorVisualState()
        {
            cursorVisuals.SetCursorImage(GetCurrentCursorState());
            cursorVisuals.SetCursorValid();
        }

        private Sprite GetCurrentCursorState() => currentItem?.itemType switch
        {
            ItemType.Seed => cursorVisuals.normal,
            ItemType.HoeTool => cursorVisuals.hold,
            _ => cursorVisuals.normal
        };

        // 事件处理方法（保持原有逻辑）
        private void OnCursorChangeEvent(CursorStates states, ItemDetails item, bool selected)
        {
            cursorEnable = selected;
            currentItem = selected ? item : null;
        }
    }
}