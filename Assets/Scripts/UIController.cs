using DG.Tweening;
using UnityEditor;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public RectTransform startMenu;
    public RectTransform title;
    public RectTransform loadingScreen;
    public float duration;

    public void Start()
    {
        DOTween.Kill(loadingScreen);
        DOTween.Kill(startMenu);
        DOTween.Kill(title);
        Sequence introSeq = DOTween.Sequence();
        introSeq.Append(ExitToTop(loadingScreen));
        introSeq.Append(EnterFromTop(title));
        introSeq.Append(EnterFromLeft(startMenu));
    }
    public Tween EnterFromLeft(RectTransform menu)
    {
        menu.anchoredPosition = new Vector2(-320, 0);
        menu.gameObject.SetActive(true);
        return menu.DOAnchorPos(Vector2.zero, duration).SetEase(Ease.OutQuint);
    }
    public Tween ExitToRight(RectTransform menu)
    {
        return menu.DOAnchorPos(new Vector2(320, 0), duration)
            .SetEase(Ease.InQuint)
            .OnComplete(() => menu.gameObject.SetActive(false));
    }
    public Tween EnterFromRight(RectTransform menu)
    {
        menu.anchoredPosition = new Vector2(320, 0);
        menu.gameObject.SetActive(true);
        return menu.DOAnchorPos(Vector2.zero, duration).SetEase(Ease.OutQuint);
    }
    public Tween ExitToLeft(RectTransform menu)
    {
        return menu.DOAnchorPos(new Vector2(-320, 0), duration)
            .SetEase(Ease.InQuint)
            .OnComplete(() => menu.gameObject.SetActive(false));
    }
    public Tween ExitToTop(RectTransform menu)
    {
        return menu.DOAnchorPos(new Vector2(0, 180), duration)
            .SetEase(Ease.InQuint)
            .OnComplete(() => menu.gameObject.SetActive(false));
    }

    public Tween EnterFromTop(RectTransform menu)
    {
        menu.anchoredPosition = new Vector2(0, 180);
        menu.gameObject.SetActive(true);
        return menu.DOAnchorPos(Vector2.zero, duration).SetEase(Ease.OutQuint);
    }
}