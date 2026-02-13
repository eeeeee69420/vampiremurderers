using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyMenu : MenuManager
{
    public RectTransform title;

    void Start()
    {
        if (loadingScreen != null) Exit(loadingScreen, UIDirection.Top);

        if (startMenu != null)
        {
            activeMenu = startMenu;
            foreach (var part in activeMenu.parts)
            {
                Enter(part.rect, part.entryDirection, entryDelay);
            }
        }
    }
}
