using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static GameController Instance { get; private set; }

    public List<GameObject> Players = new();
    GameObject hitScreen;
    public Image hpbar;
    public Image xpbar;

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
        UpdateHPBar();
    }
    [SerializeField]
    public void HitScreenAnim()
    {
        if (hitScreen != null)
            hitScreen.GetComponent<Animator>().Play("HitScreen", 0, 0f);
        else
            Debug.LogWarning("HitScreen object not assigned!");
    }
    public void UpdateHPBar()
    {
        hpbar.fillAmount = Players[0].GetComponent<PlayerController>().hp / Players[0].GetComponent<PlayerController>().stats.hpmax;
        if (Players[0].GetComponent<PlayerController>().hp < 0)
        {
        }
    }
}
