using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerUIController : MonoBehaviour
{
    [SerializeField] GamePlayPage _pageGamePlay;


    // Start is called before the first frame update
    void Start()
    {
        PlayerCharacter playerCharacter = GetComponent<PlayerCharacter>();
        playerCharacter.actionTakeHit += OnTakeHit;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void OnTakeHit(int health)
    {
        _pageGamePlay.ShowHealth(health);

        AudioController.SharedInstance.PlayEffect(AudioController.SharedInstance.DamageSound);
    }
}
