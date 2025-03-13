using System;
using Event;
using Sprout.Crop;
using Sprout.Map;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Cursor
{
    public class CursorManager : MonoBehaviour
    {
        public Sprite normal, point, hold;

        private Sprite currentSprite;
        private Image cursorImage;

        private RectTransform cursorCanvas;

        private Image buildImage;
        private Camera mainCamera;
        private Grid currentGrid;
        
        private Vector3 mouseWorldPos;
        private Vector3Int mouseGridPos;

        private bool cursorEnable;
        private bool cursorPositionValid;

        private ItemDetails currentItem;
        private bool isSelected;

        private Transform PlayerTransform => FindObjectOfType<PlayerControl>().transform;
        private void Awake()
        {
            UnityEngine.Cursor.visible = false;
        }

        private void OnEnable()
        {
            CursorEvent.CursorChangeEvent += OnCursorChangeEvent;
            SceneEvent.BeforeSceneUnloadEvent += OnBeforeSceneUnloadEvent;
            SceneEvent.AfterSceneUnloadEvent += OnAfterSceneUnloadEvent;
            CursorEvent.CursorChangeEventWithSelection += OnCursorChangeEventWithSelection;
        }

        private void OnDisable()
        {
            CursorEvent.CursorChangeEvent -= OnCursorChangeEvent;
            SceneEvent.BeforeSceneUnloadEvent -= OnBeforeSceneUnloadEvent;
            SceneEvent.AfterSceneUnloadEvent -= OnAfterSceneUnloadEvent;
            CursorEvent.CursorChangeEventWithSelection += OnCursorChangeEventWithSelection;

        }

        private void Start()
        {
            cursorCanvas = GameObject.FindGameObjectWithTag("CursorCanvas").GetComponent<RectTransform>();
            cursorImage = cursorCanvas.GetChild(0).GetComponent<Image>();
            buildImage = cursorCanvas.GetChild(1).GetComponent<Image>();
            buildImage.gameObject.SetActive(false);
            currentSprite = normal;
            SetCursorImage(normal);
            mainCamera = Camera.main;
        }

        private void Update()
        {
            if (cursorCanvas == null)
                return;
            cursorImage.transform.position = Input.mousePosition;
            if(cursorEnable)
            {
                Debug.Log("cursorEnable");
                SetCursorImage(currentSprite);
                CheckCursorValid();
                CheckPlayerInput();
            }
            else
            {
                Debug.Log("cursor-Disable");
                SetCursorImage(currentSprite);
            }
        }

        private void CheckPlayerInput()
        {
            if (Input.GetMouseButtonDown(0) && cursorPositionValid)
            {
                MouseEvent.CallMouseClickedEvent(mouseWorldPos,currentItem);
            }
        }

        private void SetCursorImage(Sprite sprite)
        {
            cursorImage.sprite = sprite;
            cursorImage.color = new Color(1, 1, 1, 1);
        }

        private void CheckCursorValid()
        {
            mouseWorldPos = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,Input.mousePosition.y,-mainCamera.transform.position.z));
            mouseGridPos = currentGrid.WorldToCell(mouseWorldPos);
            var playerGridPos = currentGrid.WorldToCell(PlayerTransform.transform.position);
            
            if (Mathf.Abs(mouseGridPos.x - playerGridPos.x)>currentItem.itemUseRadius ||
                Mathf.Abs(mouseGridPos.y - playerGridPos.y)>currentItem.itemUseRadius)
            {
                SetCursorInValid();
                return;
            }

            TileDetails currentTile = GridMapManager.Instance.GetTileDetailsOnMousePosition(mouseGridPos);

            if (currentTile != null)
            {
                CropDetails cropDetails = CropManager.Instance.GetCropDetails(currentTile.seedItemID);
                switch (currentItem.itemType)
                {
                    case ItemType.Seed:
                        if (currentTile.daySinceDug>-1&& currentTile.seedItemID ==-1)
                        {
                            SetCursorValid();
                        }
                        else
                        {
                            SetCursorInValid();
                        }
                        break;
                    case ItemType.Commodity:
                        if (currentTile.canDropItem)
                            SetCursorValid();
                        else
                            SetCursorInValid();
                        break;
                    case ItemType.HoeTool:
                        if (currentTile.canDig)
                        {
                            SetCursorValid();
                        }
                        else
                        {
                            Debug.Log(currentTile.canDig);
                            SetCursorInValid();
                        }
                        break;
                    case ItemType.Basket:
                        if (cropDetails!=null)
                        {
                            if (cropDetails.CheckToolAvailable(currentItem.id))
                            {
                                if (currentTile.growthDays >= cropDetails.TotalGrowthDays)
                                {
                                    Debug.Log("�����ո�");
                                    SetCursorValid();
                                }
                                else SetCursorInValid();
                            }
                        }
                        else SetCursorInValid();
                        break;
                    case ItemType.Furniture:
                        SetCursorValid();
                        break;
                }
            }
            else
            {
                SetCursorValid();
            }
        
        }
        
        private void SetCursorValid()
        {
            cursorPositionValid = true;
            cursorImage.color = new Color(1,1,1,1);
            //Debug.Log("Valid");
        }
        
        private void SetCursorInValid()
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }
            cursorPositionValid = false;
            cursorImage.color = new Color(1, 1, 1, 0.5f);
        }

        private void OnCursorChangeEvent(CursorStates states, ItemDetails itemDetails, bool isSelected)
        {
            this.isSelected = isSelected;
            currentSprite = states switch
            {
                CursorStates.Normal => normal,
                CursorStates.Click => point,
                CursorStates.Drag => hold,
                _ => normal
            };
            if (isSelected)
            {
                currentItem = itemDetails;
                Debug.Log(itemDetails.id);
                cursorEnable = true;
            }
            else
            {
                cursorEnable = false;
            }
        }

        private void OnBeforeSceneUnloadEvent()
        {
            cursorEnable = false;
        }

        private void OnAfterSceneUnloadEvent()
        {
            currentGrid = FindObjectOfType<Grid>();
        }

        private void OnCursorChangeEventWithSelection(CursorStates states, ItemDetails details)
        {
            currentSprite = states switch
            {
                CursorStates.Normal => normal,
                CursorStates.Click => point,
                CursorStates.Drag => hold,
                _ => normal
            };
        }

        private bool InteractWithUI()
        {
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            {
                return true;
            }
            return false;
        }
    }
}
