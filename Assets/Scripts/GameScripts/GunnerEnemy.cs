using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunnerEnemy : EnemyContoller
{
    [SerializeField] GameObject BulletPrefab;
    bool CanAttack,HasFiredToGun,WillAttack;
    [SerializeField] private Transform BulletSpawnPos;
    float AttackTimer = 1f;
    [SerializeField] GameObject[] Weapons;
    private void OnEnable()
    {
        EventManager.StartListening(GameConstants.GameEvents.PLAYER_ATTACK_ENDED, FireToPlayer);
        EventManager.StartListening(GameConstants.GameEvents.PLAYER_ATTACK_STARTED, ReturnIdle);
    }

    private void OnDisable()
    {
        EventManager.StopListening(GameConstants.GameEvents.PLAYER_ATTACK_ENDED, FireToPlayer);
        EventManager.StopListening(GameConstants.GameEvents.PLAYER_ATTACK_STARTED, ReturnIdle);
    }

    void FireToPlayer(EventParam param)
    {
        if (WillAttack)
        {
            EnemyAnimator.SetTrigger("FireTheGun");
            Weapons[0].SetActive(false);
            Weapons[1].SetActive(true);

            StartCoroutine(CheckForCanAttack());
        }
       
    }
     void ReturnIdle(EventParam param)
    {
        if (ParentEnemyHolder == param.EnemyHolderObject)
        {
            WillAttack = true;
        }
        
    }

    IEnumerator CheckForCanAttack()
    {
        yield return new WaitForSeconds(0.5f/Time.timeScale);
        CanAttack = true;
    }
    
    void Start()
    {
        base.SetStartConditions();
    }

    // Update is called once per frame
    void Update()
    {
        if (CanAttack && !HasFiredToGun && isAttackToPlayer)
        {
            
            AttackToPlayer();
            
        }
    }


    public override void AttackToPlayer()
    {
       
        
        HasFiredToGun = true;

        
        
        GameObject bullet_gameobject = Instantiate(BulletPrefab, BulletSpawnPos.position, Quaternion.identity);
        bullet_gameobject.transform.localRotation = Quaternion.Euler(Vector3.zero);
        BulletController new_bullet_controller = bullet_gameobject.GetComponent<BulletController>();
        new_bullet_controller.SetDestination(PlayerControllerTransform);
        new_bullet_controller.SetEnemyTarget(PlayerControllerTransform.gameObject);
        
       
        


    }
}
