using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum UIDirection { Left, Right, Top, Bottom }

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance { get; private set; }

    [Header("Animation Settings")]
    public float enterDuration = 1.25f;
    public float exitDuration = .75f;
    public float entryDelay = .75f;
    public Ease easeIn = Ease.InQuint;
    public Ease easeOut = Ease.OutQuint;

    [Header("Menu References")]
    public RectTransform loadingScreen;

    [Header("Position Offsets")]
    public Vector2 screenOffset = new(320, 180);

    [Header("Raycast Control")]
    public CanvasGroup globalCanvasGroup;
    public bool isPaused = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void OpenFromBottom(RectTransform menu) => Enter(menu, UIDirection.Bottom, entryDelay);
    public void OpenFromTop(RectTransform menu) => Enter(menu, UIDirection.Top, entryDelay);
    public void OpenFromLeft(RectTransform menu) => Enter(menu, UIDirection.Left, entryDelay);
    public void OpenFromRight(RectTransform menu) => Enter(menu, UIDirection.Right, entryDelay);

    public void ExitToBottom(RectTransform menu) => Exit(menu, UIDirection.Bottom);
    public void ExitToTop(RectTransform menu) => Exit(menu, UIDirection.Top);
    public void ExitToLeft(RectTransform menu) => Exit(menu, UIDirection.Left);
    public void ExitToRight(RectTransform menu) => Exit(menu, UIDirection.Right);

    public Tween Enter(RectTransform menu, UIDirection from, float delay = 0f)
    {
        if (menu == null) return null;

        menu.gameObject.SetActive(true);
        menu.anchoredPosition = GetPosForDirection(from);

        return menu.DOAnchorPos(Vector2.zero, enterDuration)
            .SetEase(easeOut)
            .SetDelay(delay)
            .OnStart(() => SetRaycasts(false))
            .OnComplete(() => SetRaycasts(true));
    }

    public Tween Exit(RectTransform menu, UIDirection to)
    {
        if (menu == null) return null;

        return menu.DOAnchorPos(GetPosForDirection(to), exitDuration)
            .SetEase(easeIn)
            .OnStart(() => SetRaycasts(false))
            .OnComplete(() =>
            {
                menu.gameObject.SetActive(false);
                SetRaycasts(true);
            });
    }

    public void SetRaycasts(bool canInteract)
    {
        globalCanvasGroup.blocksRaycasts = canInteract;
    }

    public Vector2 GetPosForDirection(UIDirection dir)
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
    public void Pause()
    {
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void Resume()
    {
        Time.timeScale = 1f;
        isPaused = false;
    }
}