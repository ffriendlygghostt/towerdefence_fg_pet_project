using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;

public class WaveHud : MonoBehaviour
{
    [SerializeField] private Transform container;
    [SerializeField] private Image prefabIcon;

    [SerializeField] private Color activeColor = Color.white;
    [SerializeField] private Color inactiveColor = new Color(1f, 1f, 1f, 0.3f);


    private List<Image> icons = new();

    private void Awake()
    {
        Clear();
    }

    public void Init(int totalWaves)
    {
        Clear();

        for(int i = 0; i < totalWaves; i++)
        {
            var icon = Instantiate(prefabIcon, container);
            icon.color = inactiveColor;
            icons.Add(icon);
        }
    }

    public void SetCurrentWave(int waveIndex)
    {
        for (int i = 0; i < icons.Count; i++)
        {
            icons[i].color = i < waveIndex
                ? activeColor
                : inactiveColor;
        }
    }

    private void Clear()
    {
        foreach (var icon in icons)
        {
            Destroy(icon);
        }
        icons.Clear();
    }
}
