using UnityEngine;

public class EntityGlasses : MonoBehaviour
{
    [SerializeField] private bool equipped;

    public bool IsEquipped => equipped;

    public void SetEquipped(bool value)
    {
        equipped = value;

        EntityBoss[] bosses = FindObjectsByType<EntityBoss>(
            FindObjectsSortMode.None
        );

        foreach (EntityBoss boss in bosses)
        {
            boss.SetGlassesState(equipped);
        }
    }

    private void Start()
    {
        SetEquipped(equipped);
    }
}