using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Transform Destination,NormalPosition, AttackPosition;
    public float CameraSpeed;

    private void OnEnable()
    {
        EventManager.StartListening(GameConstants.GameEvents.PLAYER_ATTACK_STARTED, AttackPos);
        EventManager.StartListening(GameConstants.GameEvents.PLAYER_ATTACK_ENDED, NormalPos);
    }

    private void OnDisable()
    {
        EventManager.StopListening(GameConstants.GameEvents.PLAYER_ATTACK_STARTED, AttackPos);
        EventManager.StopListening(GameConstants.GameEvents.PLAYER_ATTACK_ENDED, NormalPos);
    }

    void AttackPos(EventParam param)
    {
        
        Destination = AttackPosition;
    } 
    void NormalPos(EventParam param)
    {
        Destination = NormalPosition;
    }
    void Start()
    {
        Destination = NormalPosition;
    }

    
    void Update()
    {
        
        transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, Destination.position.y, Destination.position.z), CameraSpeed * Time.deltaTime);
        transform.rotation = Destination.rotation;
    }
}
