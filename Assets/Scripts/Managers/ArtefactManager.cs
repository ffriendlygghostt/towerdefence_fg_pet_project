using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ArtefactManager : Manager<ArtefactManager>
{
    [Header("All Artefacts")]
    [SerializeField] private List<ArtefactSO> allAfterfacts;

    private HashSet<ArtefactSO> equippedArtefacts = new();

    public List<ArtefactSO> GetRandomArtefacts(int count)
    {
        List<ArtefactSO> available = new();
        foreach (var artefact in allAfterfacts)
        {
            if (!equippedArtefacts.Contains(artefact))
            {
                available.Add(artefact);
            }
        }

        List<ArtefactSO> result = new();

        while(result.Count < count)
        {
            int index = Random.Range(0, available.Count);
            result.Add(available[index]);
            available.RemoveAt(index);
        }

        return result;
    }

    public void EquipArtefact(ArtefactSO artefact)
    {
        if (!equippedArtefacts.Contains(artefact))
        {
            equippedArtefacts.Add(artefact);
            artefact.effectArtefact.Equip();
        }
    }

    public void DequipArtefact(ArtefactSO artefact)
    {
        if (equippedArtefacts.Contains(artefact))
        {
            equippedArtefacts.Remove(artefact);
            artefact.effectArtefact.Dequip();
        }
    }

    public void ResetInventory()
    {
        foreach (var item in equippedArtefacts.ToList())
        {
            DequipArtefact(item);
        }
        equippedArtefacts.Clear(); 
    }
}
