using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class FoundryManager : BaseInteractable
{
    [SerializeField] private GameObject intakePrefab;
    [SerializeField] private GameObject additionSectorPrefab;
    [SerializeField] private GameObject subtractionSectorPrefab;
    [SerializeField] private GameObject multiplicationSectorPrefab;
    [SerializeField] private GameObject orePrefab;
    [SerializeField] private Transform oreTransformRoot;
    [SerializeField] private float intakeWidth;
    [SerializeField] private WindowQuestion windowQuestion;
    [SerializeField] private UnityEvent OnWeaponForged;

    private List<FoundryIntakeManager>[] intakeGroups;
    private List<int> targetPowerLvs;
    private int targetPower;

    private void Awake()
    {
        windowQuestion.GenerateInitialQuestion();
        List<int> enemyStrengths = windowQuestion.GetEnemyStrengths();
        Debug.Log("Window enemies: " + enemyStrengths[0] + ", " + enemyStrengths[1]);

        StartCoroutine(SpawnDelay());
    }

    private IEnumerator SpawnDelay(){
        yield return new WaitForSeconds(0.1f);

        SpawnIntake();
    }

    private void SpawnIntake(){
        targetPower = windowQuestion.GetSolution();
        windowQuestion.GenerateIntakeQuestion(targetPower);
        targetPowerLvs = windowQuestion.GetEnemyStrengths();

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

        List<int>[] valDigits = new List<int>[targetPowerLvs.Count];
        int totalDigits = 0;
        for (int i = 0; i < targetPowerLvs.Count; i++)
        {
            valDigits[i] = new List<int>();
            int temp = targetPowerLvs[i];
            for (int j = 1; j < 10; j++)
            {
                if (temp <= 0) { break; }
                valDigits[i].Add(temp % 10);
                temp /= 10;
            }
            totalDigits += valDigits[i].Count;
        }
        totalDigits += targetPowerLvs.Count - 1;

        intakeGroups = new List<FoundryIntakeManager>[targetPowerLvs.Count];
        int digitCounter = 0;
        for (int i = 0; i < targetPowerLvs.Count; i++)
        {
            intakeGroups[i] = new List<FoundryIntakeManager>();
            for (int j = 0; j < valDigits[i].Count; j++)
            {
                GameObject intake = Instantiate(intakePrefab, transform);
                intake.transform.localPosition = new Vector3(intakeWidth * (digitCounter - totalDigits / 2f + 0.5f), 0f, 0f);
                intakeGroups[i].Add(intake.GetComponent<FoundryIntakeManager>());

                GameObject ore = Instantiate(orePrefab, oreTransformRoot);
                Vector3 orePos = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f)).normalized * 3f;
                orePos.y = 3f;
                ore.transform.localPosition = orePos;
                ore.transform.parent = transform.parent;
                ore.GetComponent<OreManager>().SetPower(valDigits[i][j]);

                digitCounter++;
            }

            if (i + 1 == targetPowerLvs.Count) break;

            GameObject sector;
            switch (windowQuestion.subject)
            {
                case Subject.Addition:
                    sector = Instantiate(additionSectorPrefab, transform);
                    break;
                case Subject.Subtraction:
                    sector = Instantiate(subtractionSectorPrefab, transform);
                    break;
                case Subject.Multiplication:
                    sector = Instantiate(multiplicationSectorPrefab, transform);
                    break;
                default:
                    sector = Instantiate(additionSectorPrefab, transform);
                    break;
            }
            sector.transform.localPosition = new Vector3(intakeWidth * (digitCounter - totalDigits / 2f + 0.5f), 0f, 0f);

            digitCounter++;
        }
    }

    public override void OnInteract()
    {
        int totalAns = 0;
        switch (windowQuestion.subject)
        {
            case Subject.Addition:
                for (int i = 0; i < targetPowerLvs.Count; i++)
                {
                    int ans = 0;
                    foreach (FoundryIntakeManager intake in intakeGroups[i])
                    {
                        ans = ans * 10 + intake.GetPower();
                    }
                    totalAns += ans;
                }
                break;
            case Subject.Subtraction:
                for (int i = 0; i < targetPowerLvs.Count; i++)
                {
                    int ans = 0;
                    foreach (FoundryIntakeManager intake in intakeGroups[i])
                    {
                        ans = ans * 10 + intake.GetPower();
                    }
                    if (i == 0) { totalAns = ans; }
                    else { totalAns += ans; }
                }
                break;
            case Subject.Multiplication:
                totalAns = 1;
                for (int i = 0; i < targetPowerLvs.Count; i++)
                {
                    int ans = 0;
                    foreach (FoundryIntakeManager intake in intakeGroups[i])
                    {
                        ans = ans * 10 + intake.GetPower();
                    }
                    totalAns *= ans;
                }
                break;
        }

        if (windowQuestion.IsCorrect(totalAns))
        {
            Debug.Log("Forge correct weapon");

            OnWeaponForged.Invoke();
        }
        else
        {
            Debug.Log("Forge wrong weapon");
        }
    }
}
