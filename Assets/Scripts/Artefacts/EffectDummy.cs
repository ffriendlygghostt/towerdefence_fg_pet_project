using UnityEngine;

public class EffectDummy : MonoBehaviour, IArtefactEffect
{
    public void Equip()
    {
        Debug.LogError("Artefact Equip!");
    }
    public void Unequip()
    {
        Debug.LogError("Artefact Dequip!");
    }
}
