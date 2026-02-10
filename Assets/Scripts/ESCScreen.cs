using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ESCScreen : MonoBehaviour
{
    [SerializeField] private GameObject root;
    [SerializeField] private GameObject endrunButtonObj;

    [SerializeField] private Sprite red_endrunButtonSprite;
    [SerializeField] private Sprite red_endrunButtonHiglighterSprite;
    [SerializeField] private Sprite red_endrunButtonPressedSprite;

    private bool settingsOn = false;

    private Button endrunButton;
    private TMP_Text endrunText;
    private Image endrunImage;
    private Sprite defaultSprite;
    private SpriteState defaultState;
    private SpriteState redState;
    

    private bool endrunPress = false;

    private void Start()
    {
        endrunText = endrunButtonObj.GetComponentInChildren<TMP_Text>();
        endrunImage = endrunButtonObj.GetComponent<Image>();
        defaultState = (endrunButtonObj.GetComponent<Button>()).spriteState;
        defaultSprite = endrunImage.sprite;
        endrunButton = endrunButtonObj.GetComponent<Button>();

        redState = defaultState;
        redState.highlightedSprite = red_endrunButtonHiglighterSprite;
        redState.pressedSprite = red_endrunButtonPressedSprite;

        Hide();
    }
    

    public void Hide()
    {
        root.SetActive(false);
        if (endrunPress)
        {
            ResetEndButton();
        }
        if (settingsOn)
        {
            SettingsMenuController.Instance.Hide();
            settingsOn = false;
        }

        GameFlowManager.Instance.ReturnPlaying();
    }

    public void Show()
    {
        if (GameFlowManager.Instance.State != GameState.Playing) return;
        root.SetActive(true);
        GameFlowManager.Instance.EscMenu();
    }

    public void EscKeyPress(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) return;
        if (root.activeSelf) { Hide(); }
        else            { Show(); }
    }

    public void ReturnButton() => Hide();

    public void SettingsButton()
    {
        if(!settingsOn) { SettingsMenuController.Instance.Show(); settingsOn = true; }
        else { SettingsMenuController.Instance.Hide(); settingsOn = false;}
    }

    public void EndRunButton()
    {
        if (endrunPress) { EndButton(); return; }
        endrunPress = true;
        SetRedButton();
        endrunText.text = "END";
    }

    public void EndButton()
    {
        Hide();
        GameFlowManager.Instance.Defeat();
    }

    public void ResetEndButton()
    {

        endrunText.text = "ENDRUN";
        endrunPress = false;
        SetDefaultButton();
    }

    private void SetRedButton()
    {
        endrunImage.sprite = red_endrunButtonSprite;
        endrunButton.spriteState = redState;
    }

    private void SetDefaultButton()
    {
        endrunImage.sprite = defaultSprite;
        endrunButton.spriteState = defaultState;
    }

}
