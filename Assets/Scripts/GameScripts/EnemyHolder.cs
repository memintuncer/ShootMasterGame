using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHolder : MonoBehaviour
{
    [SerializeField] EnemyContoller[] CurrentEnemies ;
    public PlayerController playerController;
    Collider selfCollider;
    bool x = false;
    void Start()
    {
        CurrentEnemies = transform.GetComponentsInChildren<EnemyContoller>();
        selfCollider = GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player" && !x)
        {
            x = true;
            EventParam new_param  = new EventParam();
            new_param.CurrentEnemyCount = transform.childCount;
            new_param.EnemyHolderObject = gameObject;
            EventManager.TriggerEvent(GameConstants.GameEvents.PLAYER_ATTACK_STARTED, new_param);
            for(int i = 0; i < CurrentEnemies.Length; i++)
            {
                CurrentEnemies[i].SetPlayerTransform(other.transform);
                CurrentEnemies[i].isAttackToPlayer = true;
                Destroy(selfCollider);
                //CurrentEnemies[i].AttackToPlayer(other.transform);
            }
        }
    }
}
