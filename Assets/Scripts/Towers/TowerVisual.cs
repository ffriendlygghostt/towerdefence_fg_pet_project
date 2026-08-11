using System.Runtime.CompilerServices;
using UnityEngine;

public class TowerVisual : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Transform[] unitSlots;

    [SerializeField] private bool useMask;

    private Tower tower;
    private TowerRange range;

    private Unit[] units;

    public void Initialize(Tower owner, TowerRange range)
    {
        tower = owner;
        this.range = range;
    }

    private void Awake()
    {
        units = new Unit[unitSlots.Length];

        if (animator == null)
            animator = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        UpdateSpeedAnimator(SpeedGameManager.Instance.SpeedMultiplier);
    }

    public Transform GetUnitSlot(int index)
    {
        return unitSlots[index];
    }

    private void OnEnable()
    {
        SpeedGameManager.Instance.OnSpeedGameChangedMultiplie += UpdateSpeedAnimator;
    }
    private void OnDisable()
    {
        SpeedGameManager.Instance.OnSpeedGameChangedMultiplie -= UpdateSpeedAnimator;
    }
    private void UpdateSpeedAnimator(float speed)
    {
        animator.speed = speed;
    }

    public void AddUnit(int indexSlot, GameObject unitPrefab, UnitLevelData data)
    {
        Unit unit = Instantiate(
            unitPrefab,
            unitSlots[indexSlot])
            .GetComponent<Unit>();

        unit.Initialize(data, tower, range);

        if (useMask)
            unit.SetMaskInteraction(SpriteMaskInteraction.VisibleInsideMask);

        unit.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        units[indexSlot] = unit;
    }
    public void RemoveUnit(int indexSlot)
    {
        if (units[indexSlot] == null)
            return;

        Destroy(units[indexSlot].gameObject);
        units[indexSlot] = null;
    }
    public void UpgradeUnit(int indexSlot, GameObject newPrefab, UnitLevelData data)
    {
        RemoveUnit(indexSlot);
        AddUnit(indexSlot, newPrefab, data);
    }
}
