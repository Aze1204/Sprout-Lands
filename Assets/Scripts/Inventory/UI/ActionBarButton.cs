using Sprout.Inventory;
using UnityEngine;
[RequireComponent(typeof(SlotUI))]
public class ActionBarButton : MonoBehaviour
{
    public KeyCode key;
    private SlotUI slotUI;

    private void Awake()
    {
        slotUI = GetComponent<SlotUI>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(key))
        {
            if (slotUI.itemDetails != null)
            {
                slotUI.isSelected = !slotUI.isSelected;
                if (slotUI.isSelected)
                {
                    slotUI.inventoryUI.UpdateSlotHightLight(slotUI.slotIndex);
                }
                else
                {
                    slotUI.inventoryUI.UpdateSlotHightLight(-1);
                }
            }
        }
    }
}
