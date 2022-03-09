using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StateManagerDisplay : MonoBehaviour
{
    TextMeshProUGUI display;
    public bool isEnemy;
    EnemyStateManager enemyManager;
    PlayerStateManager playerManager;

    void Start()
    {
        display = this.GetComponent<TextMeshProUGUI>();
        if(isEnemy)
            enemyManager = GetComponentInParent<EnemyStateManager>();
        else
            playerManager = GetComponentInParent<PlayerStateManager>();
    }

    void Update()
    {
        if (isEnemy)
            display.text = enemyManager.GetStateName();
        else { }
            //display.text = playerManager.GetStateName();

        // Debug.Log(playerManager.GetStateName());
    }
}
