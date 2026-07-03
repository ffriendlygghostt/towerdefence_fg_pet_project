using UnityEngine;

public class TowerRange : MonoBehaviour
{
    private SpriteRenderer spriteRend;


    private void Awake()
    {
        spriteRend = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        Debug.Assert(spriteRend != null);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void SetRadius(float radius)
    {
        transform.localScale = Vector3.one * radius;
    }
}
