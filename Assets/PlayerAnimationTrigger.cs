using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationTrigger : MonoBehaviour
{
    [Tooltip("A reference to the player's animator")]
    [SerializeField] Animator _player;

    [Tooltip("The trigger parameter in the player's animator")]
    [SerializeField] string _playerTrigger;

    [Tooltip("Toggle to on if you want the trigger to be called with the box collider")]
    [SerializeField] bool _useTrigger = true;

    [Tooltip("Toggle to on if you want the trigger to be called on start")]
    [SerializeField] bool _onStart = true;

    bool _hasBeenPlayed = false;

    // Start is called before the first frame update
    void Start()
    {
        if (_onStart)
            ActivateAnimation();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!_useTrigger) return;

        if(other.CompareTag("Player"))
        {
            ActivateAnimation();
        }
    }

    void ActivateAnimation()
    {
        if (_hasBeenPlayed) return;

        _hasBeenPlayed = true;
        _player.SetTrigger(_playerTrigger);
    }
}
