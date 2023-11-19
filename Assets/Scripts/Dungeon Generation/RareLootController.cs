using UnityEngine;

public class RareLootController : MonoBehaviour
{
    private string question;
    private string answer;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Rare Loot Close");
        UIManager._instance.SetUpQuestion(question, answer, QuestionType.RareLootLock);
    }

    private void OnTriggerExit(Collider other)
    {
        UIManager._instance.CancelAttempt();
    }

    public void SetUpQuestion(string q, string a)
    {
        question = q;
        answer = a;
    }
}
