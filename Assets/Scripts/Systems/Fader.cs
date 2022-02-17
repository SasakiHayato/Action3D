using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Fadeさせるための管理クラス
/// </summary>

public class Fader : MonoBehaviour
{
    public enum FadeType
    {
        In,
        Out,
    }

    private static Fader _instane = null;
    public static Fader Instance
    {
        get
        {
            object instance = FindObjectOfType(typeof(Fader));
            if (instance == null)
            {
                GameObject obj = new GameObject("Fader");
                _instane = obj.AddComponent<Fader>();
                obj.hideFlags = HideFlags.HideInHierarchy;
            }
            else
            {
                _instane = (Fader)instance;
            }

            return _instane;
        }
    }

    public bool IsFade { get; private set; } = false;

    bool _isRequest = false;
    float _fadeSpeed;

    float _timer;
    float _startVal;
    float _endVal;

    Image _fadeImage;

    // const
    Vector2 Aspect = new Vector2(1600, 1000);

    void Update()
    {
        if (!_isRequest) return;

        _timer += Time.deltaTime;
        float alfa = Mathf.Lerp(_startVal, _endVal, _timer * _fadeSpeed);
        Color color = _fadeImage.color;
        color.a = alfa;
        _fadeImage.color = color;

        if (_endVal == alfa)
        {
            Init();
            IsFade = true;
        }
    }

    /// <summary>
    /// Fadeさせる際の申請
    /// </summary>
    /// <param name="type">フェードのタイプ</param>
    /// <param name="fadeSpeed">フェードさせる速度</param>
    public void Request(FadeType type, float fadeSpeed = 1)
    {
        GameObject parent = new GameObject("Canvas");
        Canvas canvas = parent.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 999;

        CanvasScaler canvasScaler = parent.AddComponent<CanvasScaler>();
        canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        canvasScaler.referenceResolution = Aspect;

        GameObject child = new GameObject("FadeImage");
        child.transform.SetParent(parent.transform);
        Image image = child.AddComponent<Image>();
        image.color = Color.black;
        Color color = image.color;
        if (type == FadeType.Out)
        {
            _startVal = 0;
            _endVal = 1;
            color.a = 0;
        }
        else
        {
            _startVal = 1;
            _endVal = 0;
            color.a = 1;
        }

        RectTransform rect = image.GetComponent<RectTransform>();
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;

        IsFade = false;
        _fadeImage = image;
        _fadeSpeed = fadeSpeed;
        _isRequest = true;
    }

    void Init()
    {
        _fadeImage = null;
        _fadeSpeed = 0;
        _isRequest = false;
    }
}
