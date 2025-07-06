using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static GameController Instance { get; private set; }

    public List<GameObject> Players = new();
    public List<Image> Tools = new();
    public GameObject hitScreen;
    public Color hitScreenColor;

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

    public void hitScreenAnim()
    {
        if (hitScreen != null)
            hitScreen.GetComponent<Animator>().Play("HitScreen");
        else
            Debug.LogWarning("HitScreen object not assigned!");
    }
}
