using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private Vector3 meleeRangeOffset;
    [SerializeField] private Vector3 meleeRangeDiemnsions;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    //private void OnDrawGizmosSelected()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawWireCube(transform.position + meleeRangeOffset, meleeRangeDiemnsions);
    //}
}
