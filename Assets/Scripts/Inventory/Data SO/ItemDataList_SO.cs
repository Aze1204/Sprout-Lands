using System.Collections.Generic;
using UnityEngine;
// �������־û���Ʒ��Ϣ��
// �����ߣ�Aze
// ����ʱ�䣺2025-01-02

[CreateAssetMenu(fileName = "ItemDataList_SO",menuName = "Inventory/ItemDataList")]
public class ItemDataList_SO : ScriptableObject
{
    public List<ItemDetails> itemDetailsList;
}
