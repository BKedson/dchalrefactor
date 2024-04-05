using UnityEngine;

public class FoundryIntakeManager : MonoBehaviour
{
    [SerializeField] private GameObject cursor;

    private GameObject insertedOre;
    //private int orePower;  // Save once to avoid frequent GetComponent

    // Start is called before the first frame update
    void Start()
    {
        transform.Find("Canvas").GetComponent<Canvas>().worldCamera = Camera.main;

        cursor.SetActive(false);
    }

    private void Update()
    {
        if (insertedOre)
        {
            insertedOre.transform.position = Vector3.Lerp(insertedOre.transform.position, transform.position, 0.05f);
            insertedOre.transform.rotation = Quaternion.Lerp(insertedOre.transform.rotation, Quaternion.identity, 0.05f);
        }
    }

    public void Select()
    {
        cursor.SetActive(true);
    }
    
    public void Unselect()
    {
        cursor.SetActive(false);
    }

    public bool Insert(GameObject ore)
    {
        if (insertedOre)
        {
            return false;
        }

        insertedOre = ore;

        cursor.SetActive(false);

        return true;
    }

    public GameObject Eject()
    {
        GameObject obj = insertedOre;
        insertedOre = null;
        return obj;
    }

    public int GetPower()
    {
        return (insertedOre ? insertedOre.GetComponent<OreManager>().orePower : 0);
    }
}
