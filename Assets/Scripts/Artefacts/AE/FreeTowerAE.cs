using UnityEngine;

[CreateAssetMenu(menuName = "Artefacts/FreeTowerAE")]
public class FreeTowerAE : ArtefactEffectSO
{
    public override void Dequip()
    {
        Debug.LogError("Artefact 2 Dequip");
    }

    public override void Equip()
    {
        Debug.LogError("Artefact 2 Dequip");
    }
}
