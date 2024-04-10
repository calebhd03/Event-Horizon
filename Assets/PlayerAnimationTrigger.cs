using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationTrigger : MonoBehaviour
{
    Animator _player;

    [Tooltip("The trigger parameter in the player's and this animator")]
    [SerializeField] string _trigger;

    [Tooltip("Disables player input while the animator on this object is playing")]
    [SerializeField] bool _disablePlayerInput;

    [Tooltip("Toggle to on if you want the trigger to be called with the box collider")]
    [SerializeField] bool _useTrigger = true;

    [Tooltip("Toggle to on if you want the trigger to be called on start")]
    [SerializeField] bool _onStart = true;

    bool _hasBeenPlayed = false;
    CinemachineVirtualCamera _virtualCamera;
    Animator _animator;

    // Start is called before the first frame update
    void Start()
    {
        _player = StarterAssetsInputs.Instance.GetComponent<Animator>();


        _virtualCamera = GetComponentInChildren<CinemachineVirtualCamera>();
        _virtualCamera.gameObject.SetActive(false);

        _animator = GetComponent<Animator>();
        _animator.enabled = false;

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

        _animator.enabled = true;
        _animator.SetTrigger(_trigger);

        _player.SetTrigger(_trigger);

        _virtualCamera.gameObject.SetActive(true);

        if(_disablePlayerInput) disableInput();
            
    }

    void disableInput()
    {
        StarterAssetsInputs.Instance.playerInput.currentActionMap.Disable();
    }
    void enableInput()
    {
        StarterAssetsInputs.Instance.playerInput.currentActionMap.Enable();
    }

    // The end of the animation set on this animation
    public void AnimationEnd()
    {
        if (_disablePlayerInput) enableInput();

        _animator.ResetTrigger(_trigger);
        _player.ResetTrigger(_trigger);

        _animator.enabled = false;

        _virtualCamera.gameObject.SetActive(false);
    }
}
