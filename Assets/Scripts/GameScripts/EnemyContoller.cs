using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyContoller : MonoBehaviour
{
    protected enum EnemyStates
    {
        Idle,
        Attacking,
        Hitted,
        Dead
    }

    protected EnemyStates EnemyState = EnemyStates.Idle;
    [SerializeField] protected Animator EnemyAnimator;
    protected Transform PlayerControllerTransform;
    protected Rigidbody[] ragdollRBs;
    protected Collider[] ragdollColliders;
    public bool isAttackToPlayer,isDead;
    protected GameObject ParentEnemyHolder;

    public virtual void AttackToPlayer()
    {
        transform.LookAt(PlayerControllerTransform.position);
    }

   

    public void SetPlayerTransform(Transform player_transform)
    {
        PlayerControllerTransform = player_transform;
    }

    private void Update()
    {
        if (!isDead)
        {
            if (isAttackToPlayer)
            {
                AttackToPlayer();
            }
        }
        
        
    }


    protected void SetEnemyStates()
    {

    }

    public void Death(Transform bullet_transform)
    {
        isAttackToPlayer = false;
        ToggleRagdoll(true);
    }

    protected void ToggleRagdoll(bool state)
    {
        EnemyAnimator.enabled = !state;
        foreach (Rigidbody rb in ragdollRBs)
        {
            rb.mass = 1;
            rb.isKinematic = !state;
        }
       
        foreach (Collider col in ragdollColliders)
            if (GetComponent<Collider>() != col)
            {
                col.enabled = state;
                col.isTrigger = !state;
                if (isDead && col.gameObject.tag == "EnemyAttack")
                    col.tag = "Bullet";
            }
    }


   protected void SetStartConditions()
    {
        ragdollRBs = EnemyAnimator.GetComponentsInChildren<Rigidbody>();
        ragdollColliders = EnemyAnimator.GetComponentsInChildren<Collider>();
        ToggleRagdoll(false);
        ParentEnemyHolder = transform.parent.gameObject;
        

        
    }


}
