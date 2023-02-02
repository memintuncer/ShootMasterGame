using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public Transform Destination;
    [SerializeField] float BulletSpeed;
    GameObject TargetEnemy;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Destination != null)
        {
            transform.LookAt(Destination);

            if (Destination.tag == "Player")
            {
                
                transform.position = Vector3.MoveTowards(transform.position, Destination.position, BulletSpeed * Time.deltaTime);
            }
            else
            {
                transform.Translate(transform.forward * BulletSpeed * Time.deltaTime);
            }

        }
    }

    public void SetDestination(Transform destination)
    {
        Destination = destination;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "ChosenTarget" && other.gameObject == TargetEnemy)
        {
            
            EnemyContoller enemyContoller = other.GetComponent<EnemyContoller>();
            enemyContoller.Death(transform);
            Destroy(Destination.gameObject);
            Destroy(gameObject);
        }

        if(other.tag == "Player" && other.gameObject == TargetEnemy)
        {
            Destroy(gameObject);
            EventManager.TriggerEvent(GameConstants.GameEvents.PLAYER_IS_DEAD, new EventParam());
        }
    }

    public void SetEnemyTarget(GameObject target_enemy)
    {
        TargetEnemy = target_enemy;
    }
}
