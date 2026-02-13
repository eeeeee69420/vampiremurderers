using UnityEngine;

public class MenuLink : MonoBehaviour
{
    public MenuState targetMenu;

    public void TriggerNavigation()
    {
        MenuManager.Instance.NavigateTo(targetMenu);
    }
}