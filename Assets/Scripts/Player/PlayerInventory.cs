using System;
using TMPro;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory _instance;

    [SerializeField] private Vector3 checkCenterOffset;
    [SerializeField] private float checkRadius;
    [SerializeField] private LayerMask whatIsCollectable;

    [SerializeField] private TMP_Text clueNumDisplay;

    private int clueNum;

    private void Start()
    {
        if (_instance == null)
        {
            _instance = this;
        }
    }

    private void Update()
    {
        foreach (Collider col in Physics.OverlapSphere(transform.position + checkCenterOffset, checkRadius, whatIsCollectable))
        {
            clueNum++;

            Destroy(col.gameObject);
        }
    }

    private void LateUpdate()
    {
        clueNumDisplay.text = clueNum.ToString("D2");
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;

    //    Gizmos.DrawWireSphere(transform.position + checkCenterOffset, checkRadius);
    //}

    public int GetClueNum() { return clueNum; }

    public void DropAllClue() {clueNum = 0; }
}
