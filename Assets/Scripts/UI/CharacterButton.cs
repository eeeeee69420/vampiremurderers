using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterButton : MonoBehaviour
{
    public CharacterData data;
    public Image iconImage;
    public TextMeshProUGUI characterName;
    public Image weaponImage;
    private CharacterSelectionManager manager;

    public void Setup(CharacterData newData, CharacterSelectionManager mngr)
    {
        data = newData;
        manager = mngr;
        iconImage.sprite = data.icon;
        weaponImage.sprite = data.weaponData.icon;
        characterName.text = data.characterName;
    }

    public void OnClick()
    {
        manager.DisplayCharacter(data);
    }
}