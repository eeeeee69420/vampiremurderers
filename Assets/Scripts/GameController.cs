using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public List<GameObject> Players = new();
    public GameObject hitScreen;
    public Color hitScreenColor;
    void Start()
    {
        hitScreen = GameObject.Find("HitScreen");

    }

    void Update()
    {
    }
    public void hitScreenAnim()
    {
        hitScreen.GetComponent<Animator>().Play("HitScreen");
    }
}
