using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class Background_Music : MonoBehaviour
{
    public static Background_Music instance;
    public AudioSource audioSource;
    public AudioClip[] audioClips;
    public bool inCombat = false;
    public bool inBossCombat = false;
    private void Awake()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        if(instance == null)
        {
            DontDestroyOnLoad(this.gameObject);
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        audioSource = GetComponent<AudioSource>();
        PlayLevelMusic(sceneName);
        //StartCoroutine(EnemyCombat());
        //StartCoroutine(BossCombat());
    
    }

    public void MenuMusic()
    {   
        audioSource.clip = audioClips[0];
        audioSource.Stop();
        audioSource.Play();
    }
    public void PlayLevelMusic(string sceneName)
    {
        switch(sceneName)
        {
            case "TheOuterVer2":
                audioSource.clip = audioClips[1];
                audioSource.Stop();
                audioSource.Play();
            break;
            case "Inner":
                audioSource.clip = audioClips[2];
                audioSource.Stop();
                audioSource.Play();
            break;
            case "The Center":
                audioSource.clip = audioClips[3];
                audioSource.Stop();
                audioSource.Play();
            break;
            case "Start Menu":
                audioSource.clip = audioClips[0];
                audioSource.Stop();
                audioSource.Play();
            break;
            case "TimTutorialScene":
                audioSource.clip = audioClips[1];
                audioSource.Stop();
                audioSource.Play();
            break;
        }
    }
    public void OuterMusic()
    {
        audioSource.clip = audioClips[1];
        audioSource.Stop();
        audioSource.Play();
    }
    public void InnerMusic()
    {
        audioSource.clip = audioClips[2];
        audioSource.Stop();
        audioSource.Play();
    }
    public void CenterMusic()
    {
        audioSource.clip = audioClips[3];
        audioSource.Stop();
        audioSource.Play();
    }
    public void EnemyMusic()
    {
        if(audioSource.clip != audioClips[4])
        {
        audioSource.clip = audioClips[4];
        audioSource.Stop();
        audioSource.Play();
        }
    }
    public void BossMusic()
    {
        audioSource.clip = audioClips[5];
        audioSource.Stop();
        audioSource.Play();
    }

    /*IEnumerator EnemyCombat()
    {
        yield return new WaitUntil(() => inCombat);
        EnemyMusic();
        StartCoroutine(LevelMusic());
    }
    IEnumerator BossCombat()
    {
        yield return new WaitUntil(() => inBossCombat);
        BossMusic();
        StartCoroutine(BossLevelMusic());
    }
    IEnumerator LevelMusic()
    {
        yield return new WaitUntil(() => !inCombat);
        string sceneName = SceneManager.GetActiveScene().name;
        PlayLevelMusic(sceneName);
        StartCoroutine(EnemyCombat());
    }
    IEnumerator BossLevelMusic()
    {
        yield return new WaitUntil(() => !inBossCombat);
        string sceneName = SceneManager.GetActiveScene().name;
        PlayLevelMusic(sceneName);
        StartCoroutine(BossCombat());
    }*/
}
