using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private enum PlayerStates
    {
        Idle,
        Moving,
        ChooseTargets,
        Shoot,
        Death,
        ReachedFinish,
        Finish,
        NoEffect
    }

    PlayerStates PlayerState = PlayerStates.Idle;
    [SerializeField] float ForwardSpeed, HorizontalSpeed,HorizontalInput;
    InputManager InputManager;
    [SerializeField] Animator PlayerAnimator;
    bool isChooseTargets = false,isShooting = false,CanMove = false,isDead;
    public float AttackTimer = 5;
    List<BulletController> CurrentBullets = new List<BulletController>();
    [SerializeField] Transform BulletStartPos;
    List<Transform> TargetDestinations = new List<Transform>();
    List<GameObject> Bullets = new List<GameObject>();

    [SerializeField] GameObject CrossHairPrefab,BulletPrefab;
    int BulletCount = 0, CurrentChoosedEnemy =0, CurrentEnemiesCount =0, PlayerHealth=3;
    LevelManager LevelManager;
    [SerializeField] Transform PlayerModel;
    public int  GetBulletCount()
    {
        return BulletCount;
    }

    public void SetBulletCount(int new_bullets)
    {
        BulletCount += new_bullets;
    }

    private void OnEnable()
    {
        EventManager.StartListening(GameConstants.GameEvents.PLAYER_ATTACK_STARTED, SetIsAttackBool);
        EventManager.StartListening(GameConstants.GameEvents.PLAYER_IS_DEAD, PlayerIsDead);
        EventManager.StartListening(GameConstants.GameEvents.PICKUP_HEALTH, IncreaseHealth);
        EventManager.StartListening(GameConstants.GameEvents.HITTED_TO_OBSTACLE, DecreaseHealth);
        EventManager.StartListening(GameConstants.GameEvents.PICKUP_AMMO, PickUpAmmo);
        EventManager.StartListening(GameConstants.GameEvents.PLAYER_IS_SHOOTED, PickUpAmmo);
    }

    private void OnDisable()
    {
        EventManager.StopListening(GameConstants.GameEvents.PLAYER_ATTACK_STARTED, SetIsAttackBool);
        EventManager.StopListening(GameConstants.GameEvents.PLAYER_IS_DEAD, PlayerIsDead);
        EventManager.StopListening(GameConstants.GameEvents.PICKUP_HEALTH, IncreaseHealth);
        EventManager.StopListening(GameConstants.GameEvents.HITTED_TO_OBSTACLE, DecreaseHealth);
        EventManager.StopListening(GameConstants.GameEvents.PICKUP_AMMO, PickUpAmmo);
        EventManager.StopListening(GameConstants.GameEvents.PLAYER_IS_SHOOTED, PickUpAmmo);
    }


    void IncreaseHealth(EventParam param)
    {
        if (PlayerHealth < 3)
        {
            PlayerHealth++;
            
        }
    }

    void DecreaseHealth(EventParam param)
    {
        if (PlayerHealth > 0)
        {
            PlayerHealth--;
            if(PlayerHealth == 0)
            {
                EventManager.TriggerEvent(GameConstants.GameEvents.PLAYER_IS_DEAD, new EventParam());
            }
        }
    }

    void PickUpAmmo(EventParam param)
    {
        BulletCount += param.BulletCount;
    }


    public void SetCurrentEnemyCount(int current_enemy_count)
    {
        CurrentEnemiesCount = current_enemy_count;
    }
    void SetIsAttackBool(EventParam param)
    {
        PlayerAnimator.SetTrigger("Aim");
        
        isChooseTargets = true;
        
        Debug.Log(param.CurrentEnemyCount);
        Debug.Log(CurrentEnemiesCount);
        CurrentEnemiesCount = param.CurrentEnemyCount;
    }
    void PlayerIsDead(EventParam param)
    {
        if (!isDead)
        {
            PlayerState = PlayerStates.Death;
            isDead = true;
        }
        
       
    }

    void Start()
    {
        InputManager = InputManager.Instance;
        LevelManager = LevelManager.Instance;
    }

    
    void Update()
    {
        
        PlayerStateController();
    }

    void PlayerStateController()
    {
        switch (PlayerState)
        {
            case PlayerStates.Idle:
                if(InputManager.isPointerClick || InputManager.isDrag || CanMove)
                {
                    PlayerState = PlayerStates.Moving;
                    EventManager.TriggerEvent(GameConstants.LEVEL_EVENTS.LEVEL_STARTED, new EventParam());
                    PlayerAnimator.SetTrigger("Run");
                }
                break;
            case PlayerStates.Moving:
                PlayerMovement();
                if (isChooseTargets)
                {
                    PlayerState = PlayerStates.ChooseTargets;
                }
                if (PlayerHealth <= 0)
                {
                    PlayerState = PlayerStates.Death;
                }
                break;
            case PlayerStates.ChooseTargets:
                ChooseEnemyTargets();
                if (isShooting)
                {
                    PlayerState = PlayerStates.Shoot;
                }
                break;
            case PlayerStates.Shoot:
                if (isShooting)
                {
                    PlayerState = PlayerStates.Idle;
                    StartCoroutine(ShootTargets());
                }
                break;
            case PlayerStates.ReachedFinish:

                StartCoroutine(ReachedFinish());
                PlayerState = PlayerStates.Finish;
                break;
            case PlayerStates.Finish:
                break;
            case PlayerStates.Death:
                PlayerAnimator.SetTrigger("Death");
                EventManager.TriggerEvent(GameConstants.LEVEL_EVENTS.LEVEL_FAILED, new EventParam());
                PlayerState= PlayerStates.NoEffect;
                break;
            case PlayerStates.NoEffect:
                break;
        }
    }

    IEnumerator ReachedFinish()
    {
       
        PlayerModel.rotation = Quaternion.Euler(0, 180, 0);
        yield return new WaitForSeconds(0.2f);
        PlayerAnimator.SetTrigger("Dance");
        yield return new WaitForSeconds(1.8f);
        EventManager.TriggerEvent(GameConstants.LEVEL_EVENTS.LEVEL_SUCCESSED, new EventParam());

    }

    IEnumerator ShootTargets()
    {
        PlayerAnimator.SetTrigger("Aim");
        CanMove = false;
        isShooting = false;

        for(int i = 0; i < Bullets.Count; i++)
        {
            Bullets[i].SetActive(true);
            yield return new WaitForSeconds(0.03f / Time.timeScale);
            BulletCount--;
        }
        Time.timeScale = 1;
        yield return new WaitForSeconds(1.5f);
        if (CurrentChoosedEnemy >= CurrentEnemiesCount)
        {
            PlayerAnimator.SetTrigger("Run");
            CanMove = true;
            isChooseTargets = false;
            CurrentEnemiesCount = 0;
            CurrentChoosedEnemy = 0;

        }

        else
        {
            PlayerAnimator.SetTrigger("Fear");
        }
        Bullets.Clear();
        AttackTimer = 5;
    }

    void PlayerMovement()
    {
        
        if (InputManager.isDrag)
        {
            if (HorizontalInput != InputManager.deltaPos.x)
            {
                HorizontalInput = InputManager.deltaPos.x;
                HorizontalMovement();
            }
        }

        if (!InputManager.isDrag)
            HorizontalInput = 0;
        
        transform.Translate((transform.forward * ForwardSpeed) * Time.deltaTime);
        transform.localPosition = new Vector3(Mathf.Clamp(transform.localPosition.x, -1.2f, 1.2f), transform.localPosition.y, transform.localPosition.z);
    }

    
    void HorizontalMovement()
    {
       
        transform.Translate(Vector3.right * HorizontalSpeed * HorizontalInput * Time.deltaTime);
        
    }


    void ChooseEnemyTargets()
    {
        CheckRaycastHit();
       
        Time.timeScale = 0.25f;
        AttackTimer -= Time.deltaTime/Time.timeScale;
        if (AttackTimer <= 0)
        {
            
            isChooseTargets = false;
            EventManager.TriggerEvent(GameConstants.GameEvents.PLAYER_ATTACK_ENDED, new EventParam());
            isShooting = true;
        }
        

    }


    void CheckRaycastHit()
    {
        if (InputManager.isPointerClick)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 250f))
            {
                
                if (hit.collider.tag =="Enemy" && BulletCount>0)
                {
                    
                    GameObject crosshair_object = Instantiate(CrossHairPrefab, hit.point- new Vector3(0,0,0.1f), Quaternion.identity);
                    crosshair_object.transform.parent = hit.collider.transform;
                    GameObject bullet_object = Instantiate(BulletPrefab, BulletStartPos.position, Quaternion.identity);
                    
                    BulletController new_bullet_controller = bullet_object.GetComponent<BulletController>();
                    new_bullet_controller.SetDestination(crosshair_object.transform);
                    new_bullet_controller.SetEnemyTarget(hit.collider.gameObject);
                    bullet_object.SetActive(false);
                    bullet_object.transform.parent = transform.parent;
                    Bullets.Add(bullet_object);
                    CurrentChoosedEnemy++;
                    hit.collider.tag = "ChosenTarget";
                    

                }
            }
        }
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "FinishArea")
        {
            PlayerState = PlayerStates.ReachedFinish;
            Destroy(other.gameObject);
        } 
        if (other.tag == "Obstacle")
        {
            
            Destroy(other.gameObject);
            PlayerAnimator.Play("HitToObstacle");
           
            EventManager.TriggerEvent(GameConstants.GameEvents.HITTED_TO_OBSTACLE, new EventParam());
        }

        
    }
}
