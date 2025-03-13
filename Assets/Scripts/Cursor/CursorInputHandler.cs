using Event;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Cursor
{
    public class CursorInputHandler : MonoBehaviour
    {
        public bool IsPointerOverUI => EventSystem.current.IsPointerOverGameObject();

        public void CheckPlayerInput(Vector3 mouseWorldPos, ItemDetails currentItem, bool cursorPositionValid)
        {
            if (Input.GetMouseButtonDown(0) && cursorPositionValid)
            {
                MouseEvent.CallMouseClickedEvent(mouseWorldPos, currentItem);
            }
        }
    }
}