using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="Towers/Tower Catalog")]
public class TowerCatalog : ScriptableObject
{
    public List<TowerDefinition> towers;
}
