using System.Runtime.CompilerServices;
using UnityEngine;

public class TowerVisual : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Transform[] unitSlots;

    [SerializeField] private bool useMask;

    private UnitVisual[] units;

    private void Awake()
    {
        units = new UnitVisual[unitSlots.Length];

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



    public void AddUnit(int indexSlot, GameObject unitPrefab)
    {
        UnitVisual unit = Instantiate(
            unitPrefab,
            unitSlots[indexSlot])
            .GetComponent<UnitVisual>();

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
    public void UpgradeUnit(int indexSlot, GameObject newPrefab)
    {
        RemoveUnit(indexSlot);
        AddUnit(indexSlot, newPrefab);
    }
}
