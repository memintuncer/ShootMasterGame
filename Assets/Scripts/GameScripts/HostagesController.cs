using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HostagesController : MonoBehaviour
{
    // Start is called before the first frame update
    public Animator[] HostagesAnimators;

    private void Start()
    {
        HostagesAnimators = GetComponentsInChildren<Animator>();
    }
    private void OnEnable()
    {
        EventManager.StartListening(GameConstants.LEVEL_EVENTS.LEVEL_SUCCESSED, HostagesFreed);
    }

    private void OnDisable()
    {
        EventManager.StopListening(GameConstants.LEVEL_EVENTS.LEVEL_SUCCESSED, HostagesFreed);
    }

    void HostagesFreed(EventParam param)
    {
        foreach(Animator hostage_animator in HostagesAnimators)
        {
            hostage_animator.SetTrigger("Dance");
        }
    }
}
