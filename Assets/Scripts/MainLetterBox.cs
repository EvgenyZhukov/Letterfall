using UnityEngine;
using TMPro;

public class MainLetterBox : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Sprite[] sprites;
    public TMP_Text textMeshProText;

    private int selectedSpriteIndex;
    public char letter;
    public GameObject tail;

    void Start()
    {
        SetRandomMaterial();
        SetRandomLetter();
    }

    private void Update()
    {
        if (gameObject.transform.position.y <= -10)
        {
            Destroy(gameObject);
        }
    }

    void SetRandomMaterial()
    {
        selectedSpriteIndex = Random.Range(0, sprites.Length); // Случайный индекс из массива материалов
        spriteRenderer.sprite = sprites[selectedSpriteIndex];
    }

    void SetRandomLetter()
    {
        // Получаем случайную букву
        letter = LetterRandomizer.GetRandomLetter();
        textMeshProText.text = letter.ToString();
    }
}