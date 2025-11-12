using UnityEngine;
using TMPro;

public class DamageText : MonoBehaviour
{
    public TextMeshProUGUI textMesh;

    [Header("Movement")]
    public float initialSpeed = 3f;
    public float deceleration = 2f;     // slows down over time
    public float lifetime = 1.5f;

    private Vector3 velocity;
    private float timer;
    private Color startColor;

    void Start()
    {
        startColor = textMesh.color;

        Vector2 randomDir = Random.insideUnitCircle.normalized;
        randomDir.y = Mathf.Abs(randomDir.y) + 0.2f; // bias upward a bit
        velocity = randomDir * initialSpeed;
    }

    void Update()
    {
        float delta = Time.deltaTime;
        timer += delta;
        transform.position += velocity * delta;
        velocity = Vector3.MoveTowards(velocity, Vector3.zero, deceleration * delta);
        float alpha = Mathf.Lerp(startColor.a, 0f, timer / lifetime);
        textMesh.color = new Color(startColor.r, startColor.g, startColor.b, alpha);

        if (timer >= lifetime)
            Destroy(gameObject);
    }

    public void SetText(float damage, ElementType element)
    {
        textMesh.text = ((int)damage).ToString();
        startColor = element switch
        {
            ElementType.Typeless => Color.white,
            ElementType.Water => new Color(0.3f, 0.6f, 1f),
            ElementType.Fire => new Color(1f, 0.35f, 0.1f),
            ElementType.Grass => new Color(0.2f, 0.8f, 0.2f),
            ElementType.Earth => new Color(0.55f, 0.4f, 0.2f),
            ElementType.Thunder => new Color(1f, 0.9f, 0.2f),
            ElementType.Air => new Color(0.8f, 0.9f, 1f),
            ElementType.Ice => new Color(0.6f, 0.9f, 1f),
            ElementType.Poison => new Color(0.5f, 0.9f, 0.3f),
            ElementType.Light => new Color(1f, 1f, 0.7f),
            ElementType.Dark => new Color(0.5f, 0f, 0.7f),
            _ => Color.white
        };
    }
}