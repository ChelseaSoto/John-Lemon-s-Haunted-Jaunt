using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Timer : MonoBehaviour
{
	public GameObject timerOverall;
	public GameObject slider;
	public Image uiFill;
	public TMP_Text uiText;
	public int Duration;
	private float remainingDuration;

    void Start()
    {
		timerOverall.SetActive(true);
		slider.SetActive(true);
        Begin(Duration);
    }
	
	private void Begin(float Second)
	{
		remainingDuration = Second;
		StartCoroutine(UpdateTimer());
	}
	
	private IEnumerator UpdateTimer()
	{
		while(remainingDuration >= 0)
		{
			uiText.text = $"{remainingDuration}";
			uiFill.fillAmount = Mathf.InverseLerp(0, Duration, remainingDuration);
			remainingDuration -= 1;
			yield return new WaitForSeconds(1f);
		}
		onEnd();
	}
	
	private void onEnd()
	{
		timerOverall.SetActive(false);
		slider.SetActive(false);
		Debug.Log("Times up!");
	}
}
