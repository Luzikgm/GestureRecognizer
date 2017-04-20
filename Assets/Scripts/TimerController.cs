using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerController : MonoBehaviour {

	public static Image timer {get;set;}
	public float timer_count;
	public static float timer_countS;
	public static bool ResTimer;

	 
	void Awake()
	{
		timer_countS = timer_count;
		timer = GetComponent<Image>();
	}
	void Update()
	{
		if(GameController.isPlay)
		{
			if(ResTimer)
			{
				if(timer.fillAmount > 0)
					timer.fillAmount -= Time.deltaTime * 3;
				else
					ResTimer = false;
			}
			else
			{
				if(timer.fillAmount < 1)
				timer.fillAmount += Time.deltaTime / timer_countS;
				else				
				{	
					ResTimer = true;				
					TimerController.timer_countS = timer_count;
					Camera.main.GetComponent<RecognizerController>().LrHelper.positionCount = 0;
					Camera.main.GetComponent<GameController>().GameOver();
				}
			}
		}
		
	}
}
