using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameEnding : MonoBehaviour
{
    public float fadeDuration = 1f;
    public float displayImageDuration = 1f;
	public GameObject timerOverall;
	public GameObject slider;
    public GameObject player;
    public CanvasGroup exitBackgroundImageCanvasGroup;
    public AudioSource exitAudio;
    public CanvasGroup caughtBackgroundImageCanvasGroup;
    public AudioSource caughtAudio;

	public int Duration;
    bool m_IsPlayerAtExit;
    bool m_IsPlayerCaught;
    float m_Timer;
    bool m_HasAudioPlayed;
    
    void OnTriggerEnter (Collider other)
    {
        if (other.gameObject == player)
        {
            m_IsPlayerAtExit = true;
			timerOverall.SetActive(false);
			slider.SetActive(false);
        }
    }

    public void CaughtPlayer ()
    {
        m_IsPlayerCaught = true;
		timerOverall.SetActive(false);
		slider.SetActive(false);
    }
	
	void Start()
	{
		StartCoroutine(UpdateTimer());
		timerOverall.SetActive(true);
		slider.SetActive(true);
	}
	
	private IEnumerator UpdateTimer()
	{
		while(Duration >= 0)
		{
			Duration -= 1;
			yield return new WaitForSeconds(1f);
		}
		if(m_IsPlayerAtExit == false)
		{
			CaughtPlayer();
			EndLevel (caughtBackgroundImageCanvasGroup, true, caughtAudio);
		}
	}

    void Update ()
    {
        if (m_IsPlayerAtExit)
        {
            EndLevel (exitBackgroundImageCanvasGroup, false, exitAudio);
        }
        else if (m_IsPlayerCaught)
        {
            EndLevel (caughtBackgroundImageCanvasGroup, true, caughtAudio);
        }
    }

    void EndLevel (CanvasGroup imageCanvasGroup, bool doRestart, AudioSource audioSource)
    {
        if (!m_HasAudioPlayed)
        {
            audioSource.Play();
            m_HasAudioPlayed = true;
        }
            
        m_Timer += Time.deltaTime;
        imageCanvasGroup.alpha = m_Timer / fadeDuration;

        if (m_Timer > fadeDuration + displayImageDuration)
        {
            if (doRestart)
            {
                SceneManager.LoadScene (0);
            }
            else
            {
                Application.Quit ();
            }
        }
    }
}
