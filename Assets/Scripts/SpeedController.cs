using UnityEngine;
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
    private Color inactiveColor = new Color(255, 255, 255, 0.10f);

    private void Start()
    {
        defaultColor = pauseButton.image.color;
        ActiveButton = normalButton;
        ButtonOff();
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
        SpeedGameManager.Instance.Pause();
        ActiveButton = pauseButton;
        ButtonOff();
    }
    public void OnNormal()
    {
        SpeedGameManager.Instance.Resume();
        ActiveButton = normalButton;
        ButtonOff();
    }

    public void On2x()
    {
        SpeedGameManager.Instance.Speed2x();
        ActiveButton = _2xButton;
        ButtonOff();
    }

    public void On4x()
    {
        SpeedGameManager.Instance.Speed4x();
        ActiveButton = _4xButton;
        ButtonOff();
    }
}
