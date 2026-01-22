using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.HID;
using UnityEngine.UI;

public class SpeedController : MonoBehaviour
{
    public Button pauseButton;
    public Button normalButton;
    public Button _2xButton;
    public Button _4xButton;

    private Button ActiveButton;

    private Color defaultColor;
    private Color inactiveColor = new Color(255, 255, 255, 0.3f);

    private void Start()
    {
        SpeedGameManager.Instance.OnSpeedGameChanged += OnSpeedChanged;
        defaultColor = pauseButton.image.color;
        ActiveButton = pauseButton;
        ButtonOff();
    }

    private void OnDestroy()
    {
        SpeedGameManager.Instance.OnSpeedGameChanged -= OnSpeedChanged;
    }

    private void ButtonOff()
    {
        pauseButton.image.color = inactiveColor;
        normalButton.image.color = inactiveColor;
        _2xButton.image.color = inactiveColor;
        _4xButton.image.color = inactiveColor;
        ActiveButton.image.color = defaultColor;
    }

    public void OnPause()
    {
        if (GameFlowManager.Instance.State != GameState.Playing)
            return;

        SpeedGameManager.Instance.Pause();
    }
    public void OnNormal()
    {
        if (GameFlowManager.Instance.State != GameState.Playing)
            return;

        SpeedGameManager.Instance.Resume();
    }

    public void On2x()
    {
        if (GameFlowManager.Instance.State != GameState.Playing)
            return;

        SpeedGameManager.Instance.Speed2x();
    }

    public void On4x()
    {
        if (GameFlowManager.Instance.State != GameState.Playing)
            return;

        SpeedGameManager.Instance.Speed4x();
    }

    public void KeyPause(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) return;
        OnPause();
    }

    public void KeyNormal(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) return;
        OnNormal();
    }

    public void Key2x(InputAction.CallbackContext ctx)
    {
        if(ctx.performed) return;
        On2x();
    }

    public void Key4x(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) return;
        On4x();
    }

    private void OnSpeedChanged(GameSpeed gameSpeed)
    {
        ActiveButton = gameSpeed switch
        {
            GameSpeed.Pause => pauseButton,
            GameSpeed.X1 => normalButton,
            GameSpeed.X2 => _2xButton,
            GameSpeed.X4 => _4xButton,
            _ => ActiveButton
        };

        ButtonOff();
    }
}
