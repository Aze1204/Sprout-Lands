using DG.Tweening;
using Event;
using TMPro;
using UnityEngine;
// ������ʱ��UI���ơ�
// �����ߣ�Aze
// ����ʱ�䣺2025-01-12
public class TimeUI : MonoBehaviour
{
    public RectTransform pointer;
    public TextMeshProUGUI dateText;
    public TextMeshProUGUI timeText;

    private void OnEnable()
    {
        TimeEvent.GameMinuteEvent += OnGameMinuteEvent;
        TimeEvent.GameDateEvent += OnGameDateEvent;
    }

    private void OnDisable()
    {
        TimeEvent.GameMinuteEvent -= OnGameMinuteEvent;
        TimeEvent.GameDateEvent -= OnGameDateEvent;
    }

    private void OnGameMinuteEvent(int minute, int hour)
    {
        timeText.text = hour.ToString("00")+":"+minute.ToString("00");
    }

    private void OnGameDateEvent(int hour, int day, int month, int year, Season season)
    {
        dateText.text = year + "��" + month.ToString("00") + "��" + day.ToString("00") + "��";
        PointerRotate(hour);
    }

    private void PointerRotate(int hour)
    {
        int hourRange = (hour - 6) % 18;
        float rotateZ = Mathf.Lerp(60, -60, (float)hourRange / 18);
        var target = new Vector3(0,0,rotateZ);
        pointer.DORotate(target,1f).SetEase(Ease.OutQuad);
    }
}
