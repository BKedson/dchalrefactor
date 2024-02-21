using UnityEngine;

public class FoundryManager : BaseInteractable
{
    [SerializeField] private FoundryIntakeManager intake1;
    [SerializeField] private FoundryIntakeManager intake2;
    [SerializeField] private FoundryIntakeManager intake3;
    [SerializeField] private int targetPowerLv;

    public bool weaponForged { get; private set; } = false;

    public override void OnInteract()
    {
        int totalP = intake1.GetPower() * 100 + intake2.GetPower() * 10 + intake3.GetPower();
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

        weaponForged = true;
    }
}
