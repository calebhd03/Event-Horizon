using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Background_Music : MonoBehaviour
{
    public static Background_Music instance;
    public AudioSource audioSource;
    public AudioClip[] audioClips;
    public float fadeDuration = 2f; // Duration of fade in/out in seconds

    private int enemiesSeeingPlayer = 0;

    private void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(this.gameObject);
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        audioSource = GetComponent<AudioSource>();

        // Play music for the initial scene
        PlayLevelMusic(SceneManager.GetActiveScene().name);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void MenuMusic()
    {
        StartCoroutine(FadeMusic(audioClips[0]));
    }
    public void EnemyMusic()
    {
        if(audioSource.clip != audioClips[4])
        {
            StartCoroutine(FadeMusic(audioClips[4]));
        }
    }
    public void BossMusic()
    {
        StartCoroutine(FadeMusic(audioClips[5]));
    }

    public void PlayLevelMusic(string sceneName)
    {
        AudioClip clipToPlay = null;

        switch (sceneName)
        {
            case "TheOuterVer2":
                clipToPlay = audioClips[1];
                break;
            case "Inner":
                clipToPlay = audioClips[2];
                break;
            case "The Center":
                clipToPlay = audioClips[3];
                break;
            case "Start Menu":
                clipToPlay = audioClips[0];
                break;
            case "TimTutorialScene":
                clipToPlay = audioClips[1];
                break;
        }

        if (clipToPlay != null)
        {
            StartCoroutine(FadeMusic(clipToPlay));
        }
    }
    //these are for the dev buttons
    public void OuterMusic()
    {
        StartCoroutine(FadeMusic(audioClips[1]));
    }
    public void InnerMusic()
    {
        StartCoroutine(FadeMusic(audioClips[2]));
    }
    public void CenterMusic()
    {
        StartCoroutine(FadeMusic(audioClips[3]));
    }

    IEnumerator FadeMusic(AudioClip newClip)
    {
        // Fade out current music
        float startVolume = audioSource.volume;
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            audioSource.volume = Mathf.Lerp(startVolume, 0f, t / fadeDuration);
            yield return null;
        }
        audioSource.volume = 0f;

        // Change clip and start fading in new music
        audioSource.clip = newClip;
        audioSource.Play();

        // Fade in new music
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            audioSource.volume = Mathf.Lerp(0f, startVolume, t / fadeDuration);
            yield return null;
        }
        audioSource.volume = startVolume;
    }

    public void IncrementSeeingPlayerCount()
    {
        enemiesSeeingPlayer++;
        if (enemiesSeeingPlayer == 1) // First enemy to see the player
        {
            // Implement if needed
        }
    }

    public void DecrementSeeingPlayerCount()
    {
        enemiesSeeingPlayer--;
        if (enemiesSeeingPlayer == 0) // No enemies seeing the player
        {
            string sceneName = SceneManager.GetActiveScene().name;
            PlayLevelMusic(sceneName);
        }
    }

    private void OnDestroy()
    {
        // Unregister the method from the sceneLoaded event to prevent memory leaks
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Reset the count of enemies seeing the player when a new scene is loaded
        enemiesSeeingPlayer = 0;

        // Start playing music for the new scene
        PlayLevelMusic(scene.name);
    }
    public void PauseMusic()
    {
        audioSource.Pause();
    }
    public void ResumeMusic()
    {
        audioSource.Play();
    }
}