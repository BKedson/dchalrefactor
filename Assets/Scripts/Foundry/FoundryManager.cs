using UnityEngine;
using UnityEngine.Events;

public enum MathType
{
    Addition, Subtraction, Multiplication, Division
}

public class FoundryManager : BaseInteractable
{
    [SerializeField] private GameObject intakePrefab;
    [SerializeField] private GameObject sectorPrefab;
    [SerializeField] private float intakeWidth;
    [SerializeField] private MathType mathType;
    [SerializeField] private int targetPowerLv;
    [SerializeField] private UnityEvent OnWeaponForged;

    private FoundryIntakeManager[] intakeGroup1;
    private FoundryIntakeManager[] intakeGroup2;
    private int expectedVal1 = 0;
    private int expectedVal2 = 0;

    public bool weaponForged { get; private set; } = false;

    private void Awake()
    {
        switch (mathType)
        {
            case MathType.Addition:
                expectedVal1 = Random.Range(1, targetPowerLv);
                expectedVal2 = targetPowerLv - expectedVal1;
                break;
            case MathType.Multiplication:
                for (int i = (int)Mathf.Ceil(Mathf.Sqrt(targetPowerLv)); i < targetPowerLv; i++)
                {
                    float j = (float)targetPowerLv / i;
                    if (j == (int)j)
                    {
                        expectedVal1 = i;
                        expectedVal2 = (int)j;
                    }
                }
                break;
        }
        Debug.Log(expectedVal1 + ", " + expectedVal2);

        int val1DigitNum, val2DigitNum = 0;
        for (val1DigitNum = 1; val1DigitNum < 10; val1DigitNum++)
        {
            if (Mathf.Pow(10, val1DigitNum) > expectedVal1)
            {
                break;
            }
        }
        for (val2DigitNum = 1; val2DigitNum < 10; val2DigitNum++)
        {
            if (Mathf.Pow(10, val2DigitNum) > expectedVal2)
            {
                break;
            }
        }
        Debug.Log(val1DigitNum + ", " + val2DigitNum);
        intakeGroup1 = new FoundryIntakeManager[val1DigitNum];
        intakeGroup2 = new FoundryIntakeManager[val2DigitNum];

        int totalDigit = val1DigitNum + 1 + val2DigitNum;
        GameObject obj;
        for (int i = 0; i < val1DigitNum; i++)
        {
            obj = Instantiate(intakePrefab, transform);
            obj.transform.localPosition = new Vector3(intakeWidth * (i - totalDigit / 2), 0f, 0f);
            intakeGroup1[i] = obj.GetComponent<FoundryIntakeManager>();
        }
        obj = Instantiate(sectorPrefab, transform);
        obj.transform.localPosition = new Vector3(intakeWidth * (val1DigitNum - totalDigit / 2), 0f, 0f);
        for (int i = val1DigitNum + 1; i < totalDigit; i++)
        {
            obj = Instantiate(intakePrefab, transform);
            obj.transform.localPosition = new Vector3(intakeWidth * (i - totalDigit / 2), 0f, 0f);
            intakeGroup2[i - val1DigitNum - 1] = obj.GetComponent<FoundryIntakeManager>();
        }
    }

    public override void OnInteract()
    {
        int num1 = 0;
        foreach (FoundryIntakeManager intake in intakeGroup1)
        {
            num1 = num1 * 10 + intake.GetPower();
        }
        int num2 = 0;
        foreach (FoundryIntakeManager intake in intakeGroup1)
        {
            num2 = num2 * 10 + intake.GetPower();
        }

        int totalP = 0;
        switch (mathType)
        {
            case MathType.Addition:
                totalP = num1 + num2;
                break;
        }

        if (totalP == targetPowerLv)
        {
            Debug.Log("Forge correct weapon");
        }
        else if (totalP < targetPowerLv)
        {
            Debug.Log("Forge underpowered weapon");
        }
        else
        {
            Debug.Log("Forge overpowered weapon");
        }

        OnWeaponForged.Invoke();
    }
}
