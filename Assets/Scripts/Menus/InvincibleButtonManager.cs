using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvincibleButtonManager : MonoBehaviour
{
    [SerializeField] private GameObject invinciblityActiveButton;
    [SerializeField] private GameManagerProxy gameManager;

    // Start is called before the first frame update
    void Start()
    {
        if(gameManager.IsInvincible()){
            invinciblityActiveButton.SetActive(true);
        }else{
            invinciblityActiveButton.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
