using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory _instance;

    [SerializeField] private Vector3 checkCenterOffset;
    [SerializeField] private float checkRadius;
    [SerializeField] private LayerMask whatIsCollectable;

    private int clueNum;
    private int money;
    private bool[] items = new bool[3];
    private List<GameObject> weapons = new List<GameObject>();

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

            if(col.gameObject.tag == "sword" || col.gameObject.tag == "gun" || col.gameObject.tag == "controller" ){
                AddItem(col.gameObject);
                col.gameObject.SetActive(false);
            }
            else{
                Destroy(col.gameObject);
            }
        }

        
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;

    //    Gizmos.DrawWireSphere(transform.position + checkCenterOffset, checkRadius);
    //}

    public int GetClueNum() { return clueNum; }

    public void DropAllClue() {clueNum = 0; }

    public void makeMoney(int quant){
        money += quant;
    }

    public void spendMoney(int quant){
        money -= quant;
    }
    public void AddItem(GameObject itemtoAdd){
        weapons.Add(itemtoAdd);
        //if the item is a sword
        if(itemtoAdd.tag == "sword"){
            //set the corresponding boolean in the array to true
            Debug.Log("GET");
            items[0] = true;
        }
        //if the item is a gun
        if(itemtoAdd.tag == "gun"){
            //set the corresponding boolean in the array to true
            items[1] = true;
        }
        //if the item is a controller
        if(itemtoAdd.tag == "controller"){
            //set the corresponding boolean in the array to true
            items[2] = true;
        }

    }
    public bool checkIfCollected(int target){
        //check if the boolean is true at the weapon location
        return items[target-1];
    }
}
