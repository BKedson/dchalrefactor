using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UpdateEnemyCount : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        //update the UI element to the current score
        this.gameObject.GetComponent<TMP_Text>().text = PointsAndScoreController.Instance.enemyPoints.ToString();
    }
}
