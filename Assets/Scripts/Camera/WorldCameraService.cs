using System;
using UnityEngine;

public class WorldCameraService : Manager<WorldCameraService>
{
    private Camera activeCam;
    public Action<Camera> UpdateActiveCam;
    public void SetActiveCam(Camera active)
    {
        activeCam = active;
        UpdateActiveCam(activeCam);
    }
    public Camera GetActiveCam()
    {
        return activeCam;
    }
}
