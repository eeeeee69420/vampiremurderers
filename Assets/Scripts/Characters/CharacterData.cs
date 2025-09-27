using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCharacter", menuName = "Game/Character")]
public class CharacterData : ScriptableObject
{
    public string characterName;
    public Sprite icon;
    public string description;

    public string weapon;
    public string ability;
    public PlayerStats stats;
}