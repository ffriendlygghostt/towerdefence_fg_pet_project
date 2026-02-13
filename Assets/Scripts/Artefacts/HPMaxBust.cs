using UnityEngine;

[CreateAssetMenu(menuName = "Artefacts/HPMaxBust")]
public class HPMaxBust : ArtefactEffectSO
{
    public int hpAmount = 40;

    public override void Dequip()
    {
        Debug.LogError("Artefact1 Dequip!");
    }

    public override void Equip()
    {
        Debug.LogError("Artefact1 Equip!");
    }
}
