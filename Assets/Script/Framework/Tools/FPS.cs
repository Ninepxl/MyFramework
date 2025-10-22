using UnityEngine;

public class FPS : MonoBehaviour
{
    private Rect labelRect = new Rect(30, 30, 100, 30);
    private float _Interval = 0.5f;
    private int _FrameCount = 0;
    private float _TimeCount = 0;
    private float _FrameRate = 0;

    private GUIStyle style; // 新增 GUIStyle

    void Start()
    {
        style = new GUIStyle();
        style.fontSize = 30;                // 设置字体大小
        style.normal.textColor = Color.white;
        style.fontStyle = FontStyle.Bold;   // 可选：加粗
    }

    void Update()
    {
        _FrameCount++;
        _TimeCount += Time.unscaledDeltaTime;
        if (_TimeCount >= _Interval)
        {
            _FrameRate = _FrameCount / _TimeCount;
            _FrameCount = 0;
            _TimeCount -= _Interval;
        }
    }

    void OnGUI()
    {
        GUI.Label(labelRect, $"FPS: {_FrameRate:F1}", style);
    }
}
