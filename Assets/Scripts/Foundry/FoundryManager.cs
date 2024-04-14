using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class FoundryManager : BaseInteractable
{
    [SerializeField] private GameObject intakePrefab;
    [SerializeField] private GameObject sectorPrefab;
    [SerializeField] private float intakeWidth;
    [SerializeField] private WindowQuestion windowQuestion;
    [SerializeField] private Subject mathType;
    [SerializeField] private UnityEvent OnWeaponForged;

    private List<FoundryIntakeManager>[] intakeGroups;
    private List<int> targetPowerLvs;

    private void Awake()
    {
        windowQuestion.GenerateQuestion();
        targetPowerLvs = windowQuestion.GetEnemyStrengths();

        string s = "";
        foreach (int i in targetPowerLvs)
        {
            s += i + " ";
        }
        Debug.Log(s);
        //targetPowerLvTotal = 0;

        //switch (mathType)
        //{
        //    case Subject.Addition:
        //        foreach (int i in targetPowerLvs) targetPowerLvTotal += i;
        //        break;
        //    case Subject.Multiplication:
        //        foreach (int i in targetPowerLvs) targetPowerLvTotal += i;
        //        break;
        //}

        int[] valDigitNums = new int[targetPowerLvs.Count];
        int totalDigitNum = 0;
        intakeGroups = new List<FoundryIntakeManager>[targetPowerLvs.Count];
        for (int i = 0; i < targetPowerLvs.Count; i++)
        {
            for (valDigitNums[i] = 1; valDigitNums[i] < 10; valDigitNums[i]++)
            {
                totalDigitNum++;

                if (Mathf.Pow(10, valDigitNums[0]) > targetPowerLvs[i])
                {
                    break;
                }
            }
        }
        totalDigitNum += targetPowerLvs.Count - 1;

        int digitCounter = 0;
        for (int i = 0; i < targetPowerLvs.Count; i++)
        {
            intakeGroups[i] = new List<FoundryIntakeManager>();
            for (int j = 0; j < valDigitNums[i]; j++)
            {
                GameObject intake = Instantiate(intakePrefab, transform);
                intake.transform.localPosition = new Vector3(intakeWidth * (digitCounter - totalDigitNum / 2f + 0.5f), 0f, 0f);
                intakeGroups[i].Add(intake.GetComponent<FoundryIntakeManager>());

                digitCounter++;
            }

            if (i + 1 == targetPowerLvs.Count) break;

            GameObject sector = Instantiate(sectorPrefab, transform);
            sector.transform.localPosition = new Vector3(intakeWidth * (digitCounter - totalDigitNum / 2f + 0.5f), 0f, 0f);

            digitCounter++;
        }
    }

    public override void OnInteract()
    {
        int totalAns = 0;
        for (int i = 1; i < targetPowerLvs.Count; i++)
        {
            int ans = 0;
            foreach (FoundryIntakeManager intake in intakeGroups[i])
            {
                ans = ans * 10 + intake.GetPower();
            }
            totalAns += ans;
        }

        if (windowQuestion.IsCorrect(totalAns))
        {
            Debug.Log("Forge correct weapon");
        }
        else
        {
            Debug.Log("Forge wrong weapon");
        }

        OnWeaponForged.Invoke();
    }
}
