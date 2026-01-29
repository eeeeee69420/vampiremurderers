using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;

public enum UIDirection { Left, Right, Top, Bottom }

public class MenuManager : MonoBehaviour
{
    public float duration = 0.5f;
    public float transitionDelay = 1.0f;
    public RectTransform title, startMenu, loadingScreen;

    public void SwitchMenus(List<RectTransform> toExit, List<RectTransform> toEnter, UIDirection exitDir, UIDirection enterDir)
    {
        Sequence s = DOTween.Sequence();

        foreach (var menu in toExit)
        {
            s.Join(Exit(menu, exitDir));
        }

        s.AppendInterval(transitionDelay);

        foreach (var menu in toEnter)
        {
            s.Join(Enter(menu, enterDir));
        }
    }

    public void Start()
    {
        List<RectTransform> outMenus = new List<RectTransform> { title, startMenu };
        List<RectTransform> inMenus = new List<RectTransform> { loadingScreen };

        SwitchMenus(outMenus, inMenus, UIDirection.Top, UIDirection.Bottom);
    }

    public Tween Enter(RectTransform menu, UIDirection from)
    {
        menu.anchoredPosition = GetPosForDirection(from);
        menu.gameObject.SetActive(true);
        return menu.DOAnchorPos(Vector2.zero, duration).SetEase(Ease.OutQuint);
    }

    public Tween Exit(RectTransform menu, UIDirection to)
    {
        return menu.DOAnchorPos(GetPosForDirection(to), duration)
            .SetEase(Ease.InQuint)
            .OnComplete(() => menu.gameObject.SetActive(false));
    }

    private Vector2 GetPosForDirection(UIDirection dir)
    {
        return dir switch
        {
            UIDirection.Left => new Vector2(-320, 0),
            UIDirection.Right => new Vector2(320, 0),
            UIDirection.Top => new Vector2(0, 180),
            UIDirection.Bottom => new Vector2(0, -180),
            _ => Vector2.zero
        };
    }
}