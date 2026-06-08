using UnityEngine;

[CreateAssetMenu(menuName ="Artefacts/Artefact")]
public class ArtefactSO : ScriptableObject
{
    public int IDA;
    public string artefactName;
    [TextArea] public string description;
    public Sprite icon;

    public ArtefactEffectSO effectArtefact;
}


public abstract class ArtefactEffectSO : ScriptableObject
{
    public abstract void Equip();
    public abstract void Dequip();
}

