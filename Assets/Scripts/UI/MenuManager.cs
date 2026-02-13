using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
public enum UIDirection { Left, Right, Top, Bottom }

[System.Serializable]
public class MenuPart
{
    public RectTransform rect;
    public UIDirection entryDirection;
    public UIDirection exitDirection;
}

[System.Serializable]
public class MenuState
{
    public string menuName;
    public List<MenuPart> parts;
}

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance { get; private set; }

    public float enterDuration = 1.25f;
    public float exitDuration = .75f;
    public float entryDelay = .75f;
    public Ease easeIn = Ease.InQuint;
    public Ease easeOut = Ease.OutQuint;

    public RectTransform loadingScreen;

    public Vector2 screenOffset = new(320, 180);

    public CanvasGroup globalCanvasGroup;
    public bool isPaused = false;

    public Stack<MenuState> menuHistory = new();
    public MenuState activeMenu;
    public MenuState startMenu;


    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }


    public void NavigateTo(MenuState nextMenu)
    {
        if (activeMenu != null)
        {
            menuHistory.Push(activeMenu);
            foreach (var part in activeMenu.parts)
            {
                Exit(part.rect, part.exitDirection);
            }
        }

        activeMenu = nextMenu;

        foreach (var part in activeMenu.parts)
        {
            Enter(part.rect, part.entryDirection, entryDelay);
        }
    }

    public void GoBack()
    {
        if (menuHistory.Count == 0) return;
        foreach (var part in activeMenu.parts)
        {
            Exit(part.rect, part.entryDirection);
        }

        activeMenu = menuHistory.Pop();
        foreach (var part in activeMenu.parts)
        {
            Enter(part.rect, part.exitDirection, entryDelay);
        }
    }


    public Tween Enter(RectTransform menu, UIDirection from, float delay = 0f)
    {
        if (menu == null) return null;

        menu.gameObject.SetActive(true);
        menu.anchoredPosition = GetPosForDirection(from);

        return menu.DOAnchorPos(Vector2.zero, enterDuration)
            .SetEase(easeOut)
            .SetDelay(delay)
            .SetUpdate(UpdateType.Late, true)
            .OnStart(() => SetRaycasts(false))
            .OnComplete(() => SetRaycasts(true));
    }

    public Tween Exit(RectTransform menu, UIDirection to)
    {
        if (menu == null) return null;

        return menu.DOAnchorPos(GetPosForDirection(to), exitDuration)
            .SetEase(easeIn)
            .SetUpdate(UpdateType.Late, true)
            .OnStart(() => SetRaycasts(false))
            .OnComplete(() =>
            {
                menu.gameObject.SetActive(false);
                SetRaycasts(true);
            });
    }

    public void SetRaycasts(bool canInteract)
    {
        if (globalCanvasGroup != null) globalCanvasGroup.blocksRaycasts = canInteract;
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
    void Start()
    {
        StartCoroutine(InitMenuSequence());
    }

    private IEnumerator InitMenuSequence()
    {

        yield return new WaitForEndOfFrame();
        if (loadingScreen != null)
        {
            loadingScreen.gameObject.SetActive(true);
            loadingScreen.anchoredPosition = Vector2.zero;
            Exit(loadingScreen, UIDirection.Top);
        }
        if (startMenu.parts.Count != 0)
        {
            activeMenu = startMenu;
            foreach (var part in activeMenu.parts)
            {
                Enter(part.rect, part.entryDirection, entryDelay);
            }
        }
    }
    public void LoadScene(string sceneName)
    {
        loadingScreen.gameObject.SetActive(true);
        loadingScreen.anchoredPosition = GetPosForDirection(UIDirection.Top);

        loadingScreen.DOAnchorPos(Vector2.zero, enterDuration)
            .SetEase(easeOut)
            .SetUpdate(true)
            .OnComplete(() =>
            {
                SceneManager.LoadScene(sceneName);
            });
    }
}