using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PrestartUI : MonoBehaviour
{
    [Header("UI Ref")]
    [SerializeField] private Button prestartButton;
    [SerializeField] private GameObject prestartText;

    public event Action OnPressed;
    private bool inputEnabled = false;

    private void Awake()
    {
        prestartButton.onClick.AddListener(HandlePress);
        Hide();
    }

    public void Show()
    {
        prestartText.SetActive(true);
        prestartButton.interactable = true;
        inputEnabled = true;
    }

    public void Hide()
    {
        prestartText.SetActive(false);
        prestartButton.interactable = false;
        inputEnabled = false;
    }

    public void HandlePress()
    {
        if (!inputEnabled) return;
        inputEnabled = false;
        prestartButton.interactable = false;

        OnPressed?.Invoke();
    }

    public void OnAnyKey(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) return;
        HandlePress();
    }
}
