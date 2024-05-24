using UnityEngine;

// This class handles behaviors of an intake when selected/deselected/inserted with an ore
public class FoundryIntakeManager : MonoBehaviour
{
    // UI object to highlight this intake
    [SerializeField] private GameObject cursor;

    // UI objects to provide player feedback
    [SerializeField] private GameObject incorrect;
    [SerializeField] private GameObject correct;

    private GameObject insertedOre;  // A reference to the currently inserted ore, if any
    //private int orePower;  // Save once to avoid frequent GetComponent

    private TextboxBehavior tutorial;

    // Start is called before the first frame update
    void Start()
    {
        // Setup the canvas of cursor
        transform.Find("Canvas").GetComponent<Canvas>().worldCamera = Camera.main;

        cursor.SetActive(false);
    }

    private void Update()
    {
        // Keep the inserted ore in its position
        if (insertedOre)
        {
            insertedOre.transform.position = Vector3.Lerp(insertedOre.transform.position, transform.position, 0.1f);
            // insertedOre.transform.position = Vector3.Lerp(pos, transform.position, 0.05f);
            insertedOre.transform.rotation = Quaternion.Lerp(insertedOre.transform.rotation, Quaternion.identity, 0.05f);
        }
    }

    // Highlight when aimed at
    public void Select()
    {
        cursor.SetActive(true);
    }

    // Cancel highlight when no longer aimed at
    public void Unselect()
    {
        cursor.SetActive(false);
    }

    public void IncorrectFeedback(){
        incorrect.SetActive(true);
    }

    public void CorrectFeedback(){
        correct.SetActive(true);
    }

    public void ClearFeedback(){
        correct.SetActive(false);
        incorrect.SetActive(false);
    }

    public bool Insert(GameObject ore)
    {
        if (insertedOre)
        {
            return false;
        }

        insertedOre = ore;

        cursor.SetActive(false);

        tutorial.OrePlaced();

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

    public void SetTutorial(TextboxBehavior tut){
        tutorial = tut;
    }
}
