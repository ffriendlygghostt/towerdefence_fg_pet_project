using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private Canvas canvas;

    public void Init(float maxHP)
    {
        slider.maxValue = maxHP;
        slider.value = maxHP;
        Show();
    }

    public void UpdateHealth(float currentHP)
    {
        slider.value = currentHP;
    }

    public void Show()
    {
        canvas.enabled = true;
    }

    public void Hide()
    {
        canvas.enabled = false;
    }
}
