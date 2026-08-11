using UnityEngine;

public class GhostTowersManager : Manager<GhostTowersManager>
{
    public Sprite[] ghostSprites;
    
    private void Start()
    {

    }

    public Sprite GetRandomGhostSprite()
    {
        return ghostSprites[Random.Range(0, ghostSprites.Length)];
    }
}
