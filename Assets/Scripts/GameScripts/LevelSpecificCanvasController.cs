using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class LevelSpecificCanvasController : MonoBehaviour
{
    [SerializeField] PlayerController PlayerController;
    [SerializeField] Transform HeartIcons;
    int HealthCount=3;
    [SerializeField] TextMeshProUGUI BulletCountText,LevelInfoText;
    [SerializeField] Animator LevelEndAnimator,TutorialAnimator;

    LevelManager LevelManager;
    [SerializeField] GameObject TutorialContainer,SwipeTutorial,TapTutorial;
    bool IsFirstLevel = false;
    private void OnEnable()
    {
        EventManager.StartListening(GameConstants.GameEvents.PICKUP_HEALTH, IncreaseHealth);
        EventManager.StartListening(GameConstants.GameEvents.HITTED_TO_OBSTACLE, DecreaseHealth);
        EventManager.StartListening(GameConstants.LEVEL_EVENTS.LEVEL_STARTED,DeActivateSwipeTutorial);
        EventManager.StartListening(GameConstants.LEVEL_EVENTS.LEVEL_FAILED, LevelFailedFeedBack);
        EventManager.StartListening(GameConstants.LEVEL_EVENTS.LEVEL_SUCCESSED,LevelSuccessedFeedBack);
        EventManager.StartListening(GameConstants.GameEvents.PLAYER_ATTACK_STARTED,ActivateTapTutorial);
        EventManager.StartListening(GameConstants.GameEvents.PLAYER_ATTACK_ENDED,DeActivateTapTutorial);

    }

    private void OnDisable()
    {
        EventManager.StopListening(GameConstants.GameEvents.PICKUP_HEALTH, IncreaseHealth);
        EventManager.StopListening(GameConstants.GameEvents.HITTED_TO_OBSTACLE, DecreaseHealth);
        EventManager.StopListening(GameConstants.LEVEL_EVENTS.LEVEL_STARTED, DeActivateSwipeTutorial);
        EventManager.StopListening(GameConstants.LEVEL_EVENTS.LEVEL_FAILED, LevelFailedFeedBack);
        EventManager.StopListening(GameConstants.LEVEL_EVENTS.LEVEL_SUCCESSED, LevelSuccessedFeedBack);
        EventManager.StopListening(GameConstants.GameEvents.PLAYER_ATTACK_STARTED, ActivateTapTutorial);
        EventManager.StopListening(GameConstants.GameEvents.PLAYER_ATTACK_ENDED, DeActivateTapTutorial);

    }


    void IncreaseHealth(EventParam param)
    {
        if (HealthCount < 3)
        {
            HealthCount++;
            HeartIcons.GetChild(HealthCount - 1).gameObject.SetActive(true);
        }
    }

    void DecreaseHealth(EventParam param)
    {
        if (HealthCount > 0)
        {
            HealthCount--;
            HeartIcons.GetChild(HealthCount).gameObject.SetActive(false);
        }
    }

    void DeActivateSwipeTutorial(EventParam param)
    {
        if (IsFirstLevel)
        {
            SwipeTutorial.SetActive(false);
        }
    }

    void ActivateTapTutorial(EventParam param)
    {
        if (IsFirstLevel)
        {
            TutorialAnimator.SetTrigger("TapTutorial");
            TapTutorial.SetActive(true);
        }
    }
    void DeActivateTapTutorial(EventParam param)
    {
        if (IsFirstLevel)
        {
            TapTutorial.SetActive(false);
        }
    }

    void LevelFailedFeedBack(EventParam param)
    {
        LevelEndAnimator.SetTrigger("Fail");
    }
    void LevelSuccessedFeedBack(EventParam param)
    {
        LevelEndAnimator.SetTrigger("Success");
    }

    void Start()
    {
        LevelManager = LevelManager.Instance;
        CheckLevelIsFirstLevel();
        LevelInfoText.text = "LEVEL " + (LevelManager.GetCurrenLevelIndex() + 1).ToString();
    }

    // Update is called once per frame
    void Update()
    {
        BulletCountText.text = PlayerController.GetBulletCount().ToString();
    }

    void CheckLevelIsFirstLevel()
    {
        if(LevelManager.GetCurrenLevelIndex() == 0)
        {
            TutorialContainer.SetActive(true);
            IsFirstLevel = true;
        }
        
    }
}
