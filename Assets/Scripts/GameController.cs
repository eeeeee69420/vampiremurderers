using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static GameController Instance { get; private set; }

    public List<GameObject> Players = new();
    public List<Image> Tools = new();
    public GameObject hitScreen;
    public Color hitScreenColor;
    public Image hpbar;

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

    void Start()
    {
        hitScreen = GameObject.Find("HitScreen");
    }
    [SerializeField]
    public void HitScreenAnim()
    {
        if (hitScreen != null)
            hitScreen.GetComponent<Animator>().Play("HitScreen");
        else
            Debug.LogWarning("HitScreen object not assigned!");
    }
    public void UpdateHPBar()
    {
        hpbar.fillAmount = Players[0].GetComponent<PlayerController>().stats.hp / Players[0].GetComponent<PlayerController>().stats.hpmax;
        if (Players[0].GetComponent<PlayerController>().stats.hp < 0)
        {
        }
    }
}
