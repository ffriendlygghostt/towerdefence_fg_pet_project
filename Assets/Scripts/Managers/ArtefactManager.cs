using System.Collections.Generic;
using System.Linq;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class ArtefactManager : Manager<ArtefactManager>
{
    private List<ArtefactSO> allArtefacts = new();
    private List<ArtefactSO> infinityArtefacts = new();
    private HashSet<ArtefactSO> equippedArtefacts = new();

    private bool artefactsClear = false;


    protected override void Awake()
    {
        base.Awake();
        InitializeAllArtefacts();
        InitializeInfinityArtefacts();
        if (allArtefacts.Count < 1)
            Debug.LogWarning("Artefacts not detected!");
    }

    public async void InitializeAllArtefacts()
    {
        var handle = Addressables.LoadAssetsAsync<ArtefactSO>("Artefact", null);
        await handle.Task;
        if (handle.Status == UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationStatus.Succeeded)
        {
            allArtefacts = new List<ArtefactSO>(handle.Result);
        }
    }
    public async void InitializeInfinityArtefacts()
    {
        var handle = Addressables.LoadAssetsAsync<ArtefactSO>("ArtefactINF", null);
        await handle.Task;
        if (handle.Status == UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationStatus.Succeeded)
        {
            infinityArtefacts = new List<ArtefactSO>(handle.Result);
        }
    }


    public List<ArtefactSO> GetRandomArtefacts(int count)
    {
        List<ArtefactSO> available = new();
        foreach (var artefact in allArtefacts)
        {
            if (!equippedArtefacts.Contains(artefact))
            {
                available.Add(artefact);
            }
        }

        List<ArtefactSO> result = new();

        while (result.Count < count)
        {
            if (available.Count < count)
            {
                if (available.Count > 0)
                {
                    result.Add(available[0]);
                    available.RemoveAt(0);
                }
                else
                {
                    if (infinityArtefacts.Count == 0)
                    {
                        Debug.LogWarning("Infinity artefacts pool is empty");
                        break;
                    }

                    int index = Random.Range(0, infinityArtefacts.Count);
                    var art = Instantiate(infinityArtefacts[index]);
                    result.Add(art);
                }
            }
            // FIXME: Creates runtime SO instances that are never released.
            // TODO: Temporary fallback.
            // When all unique artefacts are exhausted, generate runtime copies
            // of infinity artefacts. Consider replacing with repeatable upgrades.
            else
            {
                int index = Random.Range(0, available.Count);
                result.Add(available[index]);
                available.RemoveAt(index);
            }
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
