using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyMenu : MenuManager
{
    public RectTransform title;
    public RectTransform startMenu;

    public void Start()
    {
        Sequence s = DOTween.Sequence();

        s.OnStart(() => SetRaycasts(false));

        s.Append(Exit(loadingScreen, UIDirection.Top));
        s.Append(Enter(title, UIDirection.Top));
        s.Append(Enter(startMenu, UIDirection.Left));

        s.OnComplete(() => SetRaycasts(true));
    }
}
