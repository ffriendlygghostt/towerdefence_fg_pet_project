using System;
using Unity.Android.Gradle;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public enum GameState 
{
    Menu,
    Loading,
    Playing,
    ArtifactChoice,
    Defeat,
    Prestart,
    EscMenu
}


public class GameFlowManager : Manager<GameFlowManager>
{
    public GameState State { get; private set; } = GameState.Menu;

    private bool restartLocked = false;

    private void Start()
    {
        BaseManager.Instance.OnBaseDestroyed += Defeat;
    }

    private void OnDisable()
    {
        BaseManager.Instance.OnBaseDestroyed -= Defeat;
    }

    protected override void Awake()
    {
        base.Awake();

    }

    public void StartRun()
    {
        GameManager.Instance.AddFloor();
        LoadStage();
    }

    public void OnStageCompleted()
    { 
        State = GameState.ArtifactChoice;
        SpeedGameManager.Instance.Pause();
        HudManager.Instance.ShowStageCompletedCanvas();
    }

    public void ContinueAfterArtifact()
    {
        GameManager.Instance.AddFloor();
        LoadStage();
    }

    private void LoadStage()
    {
        if (State == GameState.Loading) return;
        State = GameState.Loading;

        SceneLoader.Instance.LoadScene(
            LevelManager.Instance.GetRandomLevel(),
            () =>
            {
                restartLocked = false;

                DifficultyManager.Instance.SetFloor(GameManager.Instance.FloorThisRun);
                WalletManager.Instance.ResetWallet();
                BaseManager.Instance.ResetBase();
                HudManager.Instance.Reset();
                HudManager.Instance.Show();
                State = GameState.Prestart;
            },
            () => { HudManager.Instance.ShowPrestartButton(); }
        );
    }

    public void StartPlaying()
    {
        if (State != GameState.Prestart) return;

        State = GameState.Playing;
        HudManager.Instance.HidePrestart();
        GameManager.Instance.StartGame();
        SpeedGameManager.Instance.Resume();
    }

    public void Defeat()
    {
        State = GameState.Defeat;

        SpeedGameManager.Instance.Pause();
        HudManager.Instance.ShowDefeatScreen();
        GameManager.Instance.EndTimer();
    }

    public void RestartGame()
    {
        if (restartLocked) return;
        if (State != GameState.Defeat) return;

        restartLocked = true;
        GameManager.Instance.EndRun();
        ArtefactManager.Instance.ResetInventory();
        LoadStage();
    }

    public void LeaveGame()
    {
        if (State == GameState.Loading) return;
        //HudManager.Instance.HideDefeatScreen();
        GameManager.Instance.EndRun();
        ArtefactManager.Instance.ResetInventory();
        MenuGame();
    }

    public void MenuGame()
    {
        State = GameState.Loading;

        SpeedGameManager.Instance.Pause();
        
        SceneLoader.Instance.LoadScene("MainMenu",
            () => 
            { 
                HudManager.Instance.Hide();
                State = GameState.Menu;
            }
        );
    }

    public void EscMenu()
    {
        State = GameState.EscMenu;
        SpeedGameManager.Instance.Pause();
    }

    public void ReturnPlaying()
    {
        State = GameState.Playing;
        SpeedGameManager.Instance.SetPastSpeed();
    }
}


