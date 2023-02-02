using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class BulletHolder : MonoBehaviour
{
    public int BulletCount;
    public TextMeshProUGUI BulletCountText;
    Collider SelfCollider;
    void Start()
    {
        BulletCountText.text ="+ " + BulletCount.ToString();
        SelfCollider = GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            SelfCollider.enabled = false;
            EventParam event_param = new EventParam();
            event_param.BulletCount = BulletCount;
            EventManager.TriggerEvent(GameConstants.GameEvents.PICKUP_AMMO, event_param);
        }
    }
}
