using System;
using UnityEngine;

public class NextFloorScreen : MonoBehaviour
{
    public Action OnEndRunWinMenu;
    [SerializeField] private GameObject contentItems;

    private void Awake() =>
        Hide();

    public void Show()
    {
        gameObject.SetActive(true);
        LoadNewItems();
    }

    public void Hide() =>
        gameObject.SetActive(false);

    public void ContinueButton() =>
        GameFlowManager.Instance.ContinueAfterArtifact();
    public void EndRunButton() =>
        GameFlowManager.Instance.LeaveGame();

    public void LoadNewItems()
    {

    }

}
