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
        UpdateXPBar();
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
    public void UpdateXPBar()
    {
        PlayerController playerController = Players[0].GetComponent<PlayerController>();
        if (playerController.xp > playerController.xpMax)
        {
            playerController.level += 1;
            playerController.xp -= playerController.xpMax;
            playerController.xpMax *= playerController.xpScaling;
        }
        xpbar.fillAmount = playerController.xp / playerController.xpMax;
    }
}
