using UnityEngine;
using DG.Tweening;

public enum UIDirection { Left, Right, Top, Bottom }

public class MenuManager : MonoBehaviour
{
    [Header("Animation Settings")]
    public float enterDuration = 2f;
    public float exitDuration = 1f;
    public Ease easeIn = Ease.InQuint;
    public Ease easeOut = Ease.OutQuint;

    [Header("Menu References")]
    public RectTransform title;
    public RectTransform startMenu;
    public RectTransform loadingScreen;

    [Header("Position Offsets")]
    public Vector2 screenOffset = new Vector2(320, 180);

    public void Start()
    {
        Sequence s = DOTween.Sequence();
        s.Append(Exit(loadingScreen, UIDirection.Top));
        s.Append(Enter(title, UIDirection.Top));
        s.Append(Enter(startMenu, UIDirection.Left));
    }


    public void OpenFromBottom(RectTransform menu) => Enter(menu, UIDirection.Bottom);
    public void OpenFromTop(RectTransform menu) => Enter(menu, UIDirection.Top);
    public void OpenFromLeft(RectTransform menu) => Enter(menu, UIDirection.Left);
    public void OpenFromRight(RectTransform menu) => Enter(menu, UIDirection.Right);

    public void ExitToBottom(RectTransform menu) => Exit(menu, UIDirection.Bottom);
    public void ExitToTop(RectTransform menu) => Exit(menu, UIDirection.Top);
    public void ExitToLeft(RectTransform menu) => Exit(menu, UIDirection.Left);
    public void ExitToRight(RectTransform menu) => Exit(menu, UIDirection.Right);


    public Tween Enter(RectTransform menu, UIDirection from)
    {
        if (menu == null) return null;
        menu.gameObject.SetActive(true);
        menu.anchoredPosition = GetPosForDirection(from);
        return menu.DOAnchorPos(Vector2.zero, enterDuration).SetEase(easeOut);
    }

    public Tween Exit(RectTransform menu, UIDirection to)
    {
        if (menu == null) return null;
        return menu.DOAnchorPos(GetPosForDirection(to), exitDuration)
            .SetEase(easeIn)
            .OnComplete(() => menu.gameObject.SetActive(false));
    }

    private Vector2 GetPosForDirection(UIDirection dir)
    {
        return dir switch
        {
            UIDirection.Left => new Vector2(-screenOffset.x, 0),
            UIDirection.Right => new Vector2(screenOffset.x, 0),
            UIDirection.Top => new Vector2(0, screenOffset.y),
            UIDirection.Bottom => new Vector2(0, -screenOffset.y),
            _ => Vector2.zero
        };
    }
}