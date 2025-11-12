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

    [SerializeField] private GameObject damageTextPrefab;
    [SerializeField] private Canvas worldCanvas;

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
    public void ShowDamage(float damage, ElementType element, Vector3 worldPosition)
    {
        // Spawn in world space under the world canvas
        GameObject dmgObj = Instantiate(damageTextPrefab, worldPosition, Quaternion.identity, worldCanvas.transform);

        DamageText dmgText = dmgObj.GetComponent<DamageText>();
        dmgText.SetText(damage, element);
    }
}
