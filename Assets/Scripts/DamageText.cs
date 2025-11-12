using UnityEngine;
using TMPro;

public class DamageText : MonoBehaviour
{
    public TextMeshProUGUI textMesh;
    public float moveSpeed = 1f;
    public float fadeDuration = 1f;

    private Color startColor;
    private float timer;

    void Start()
    {
        startColor = textMesh.color;
    }

    void Update()
    {
        transform.position += moveSpeed * Time.deltaTime * Vector3.up;

        timer += Time.deltaTime;
        float alpha = Mathf.Lerp(startColor.a, 0, timer / fadeDuration);
        textMesh.color = new Color(startColor.r, startColor.g, startColor.b, alpha);

        if (alpha <= 0.01f)
            Destroy(gameObject);
    }

    public void SetText(string text, ElementType element)
    {
        textMesh.text = text;
        startColo
    }
}