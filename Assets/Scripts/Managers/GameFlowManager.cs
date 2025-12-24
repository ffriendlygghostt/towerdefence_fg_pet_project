using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState 
{
    Menu,
    Loading,
    Playing,
    ArtifactChoice,
    Defeat
}


public class GameFlowManager : Manager<GameFlowManager>
{
    public GameState State { get; private set; }

    private int currentFloor = 0;

    public void StartRun()
    {
        currentFloor = 1;
        LoadStage();
    }

    public void OnStageCompleted()
    {
        State = GameState.ArtifactChoice;
        //ArtifactUI.Instance.Show();
    }

    public void ContinueAfterArtifact()
    {
        currentFloor++;
        LoadStage();
    }

    private void LoadStage()
    {
        State = GameState.Loading;
        FadeManager.FadeOutThen(() =>
        {
            DifficultyManager.Instance.SetFloor(currentFloor);
            SceneLoader.Instance.LoadScene(LevelManager.Instance.GetRandomLevel());
        });
    }

    public void Defeat()
    {
        State = GameState.Defeat;
        //TO DO: UI DEFEAT
    }
}
