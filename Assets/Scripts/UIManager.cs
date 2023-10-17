using TMPro;
using UnityEngine;

public enum QuestionType
{
    DoorLock, RareLootLock
}

public class UIManager : MonoBehaviour
{
    public static UIManager _instance;

    [SerializeField] private bool skipQuestions;

    [SerializeField] private GameObject questionUIRoot;
    [SerializeField] private TMP_Text questionDisplayer;
    [SerializeField] private TMP_InputField answerInput;

    [SerializeField] private GameObject DungeonExitComfirmationUI;

    private string correctAnswer = "";
    private QuestionType currQType;

    private void Start()
    {
        if (_instance == null)
        {
            _instance = this;
        }

        answerInput.onSubmit.AddListener(OnSubmitAnswer);
        questionUIRoot.SetActive(false);
    }

    public void SetUpQuestion(string question, string correctAns, QuestionType qType)
    {
        currQType = qType;
        if (currQType == QuestionType.DoorLock && skipQuestions)
        {
            DungeonGenerator._instance.OnCorrectAnswer();

            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            questionUIRoot.SetActive(true);
            questionDisplayer.text = question;
            questionDisplayer.fontSize = (DungeonGenerator._instance.GetOperation() == MathType.Fraction) ? 80f : 130f;
            answerInput.text = "";
            correctAnswer = correctAns;

            Cursor.lockState = CursorLockMode.Confined;

            answerInput.Select();
        }
    }

    public void CancelAttempt()
    {
        questionUIRoot.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
    }

    public void OnSubmitAnswer(string answer)
    {
        if (answer != "" && answer == correctAnswer)
        {
            switch (currQType)
            {
                case QuestionType.DoorLock:
                    DungeonGenerator._instance.OnCorrectAnswer();
                    break;
                case QuestionType.RareLootLock:
                    break;
            }

            questionUIRoot.SetActive(false);

            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    public void ActivateDungeonExitComfirmation(bool active)
    {
        DungeonExitComfirmationUI.SetActive(active);

        if (active)
        {
            Cursor.lockState = CursorLockMode.Confined;

            PlayerMovement._instance.EnableInput(false);
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;

            PlayerMovement._instance.EnableInput(true);
        }
    }
}
