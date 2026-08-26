using UnityEngine;

[CreateAssetMenu(menuName ="Artefacts/ClickKillAE")]
public class ClickKillAE : ArtefactEffectSO
{
    public override void Dequip()
    {
        Debug.LogError("Artefact 3 Dequip");
    }

    public override void Equip()
    {
        Debug.LogError("Artefact 3 Equip");
    }
}
