using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;


public class Portal : MonoBehaviour
{
    public string nextSceneName; // Name of the scene to load
    public SceneTransitionController sceneTransition;
    public ParticleSystem portalParticle; // Reference to the Particle System

    public GameObject ObjectiveTrackerObject;

     
    [SerializeField]
    private CinemachineVirtualCamera MainCam;
    [SerializeField]
    private CinemachineVirtualCamera PortalCam;
    [SerializeField]
    private CinemachineVirtualCamera AimCam;
    private bool MainCamera = false;
    private bool LevelIsComplete = false;

    private void Awake()
    {
        // Disable the Portal object at the start
      //  gameObject.SetActive(false);

        // Ensure the particle system is not playing on awake
        if (portalParticle != null)
        {
            portalParticle.Stop();
        }
    }

    private void OnEnable()
    {
       // AndDestroy();
    }
    private void Update()
    {
        LevelCompleteCheck();

    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !sceneTransition.IsFading())
        {
            
            SaveSystemTest saveSystem = FindObjectOfType<SaveSystemTest>();
            if (saveSystem != null)
            {
                //saveSystem.SaveGame();
                //saveSystem.LoadGame();
            }

            
            if (portalParticle != null)
            {
                portalParticle.Play();
            }

            Background_Music.instance.PauseMusic();
            sceneTransition.StartCoroutine("FadeIn", nextSceneName);
            MainCamera = true;
            Destroy(other.gameObject);
            PortalCamPriority();
        }
        else
        {
            Debug.Log("Level is not complete yet!");
            
        }
    }

    /*public void AndDestroy()
    {
        // Find and destroy all objects tagged as "Enemy"
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            Destroy(enemy);
        }
    }*/

    public void PortalCamPriority()
    {
            if (MainCamera)
        {

            MainCam.Priority = 0;
            PortalCam.Priority = 3;
            AimCam.Priority = 0;
        }
        else{

            MainCam.Priority = 10;
            PortalCam.Priority = 0;
            AimCam.Priority = 10;
        }
        MainCamera = !MainCamera;
    }

    private void LevelCompleteCheck()
    {
        if (ObjectiveTrackerObject != null)
        {
            ObjectiveTracker objectiveTracker = ObjectiveTrackerObject.GetComponent<ObjectiveTracker>();
            if (objectiveTracker != null)
            {
                LevelIsComplete = objectiveTracker.levelComplete;
            }
        }
    }
}