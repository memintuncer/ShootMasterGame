using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMelee : EnemyContoller
{

    [SerializeField] float MovementSpeed;
    float StartSpeed;
    float DistanceToPlayer;
    bool isKillThePlayer=false;

    private void OnEnable()
    {
        EventManager.StartListening(GameConstants.GameEvents.PLAYER_ATTACK_ENDED, SpeedUp);
        EventManager.StartListening(GameConstants.GameEvents.PLAYER_ATTACK_STARTED, SlowDown);
    }

    private void OnDisable()
    {
        EventManager.StopListening(GameConstants.GameEvents.PLAYER_ATTACK_ENDED, SpeedUp);
        EventManager.StopListening(GameConstants.GameEvents.PLAYER_ATTACK_STARTED, SlowDown);
    }
    
    void SpeedUp(EventParam param)
    {
        MovementSpeed *= 3;
    }
    void SlowDown(EventParam param)
    {
        MovementSpeed = StartSpeed;
    }
    void Start()
    {
        base.SetStartConditions();
        StartSpeed = MovementSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (isAttackToPlayer && !isKillThePlayer)
        {
            AttackToPlayer(); 
            DistanceToPlayer = Vector3.Distance(transform.position, PlayerControllerTransform.position);
            if (DistanceToPlayer <= 0.3f)
            {
                KillThePlayer();
            }
        }


    }


    void KillThePlayer()
    {
        MovementSpeed = 0;
        EnemyAnimator.SetTrigger("Attack");
        isKillThePlayer = true;
        EventManager.TriggerEvent(GameConstants.GameEvents.PLAYER_IS_DEAD, new EventParam());

    }

    public override void AttackToPlayer()
    {
        base.AttackToPlayer();
        EnemyAnimator.SetTrigger("Run");
        
        transform.Translate(-transform.forward * MovementSpeed * Time.deltaTime);
        

    }

    
}
