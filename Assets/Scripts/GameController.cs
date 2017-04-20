using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class GameController : MonoBehaviour {
	float scale = 0.5f;
	public GameObject lr;
	//[HideInInspector]
	public static GameObject lr_temp;
	public static  List<TextAsset> Lvls = new List<TextAsset>();
	public static List<Vector2> lrMass = new List<Vector2>();
	public static bool isPlay;
	static int score; 

	Text lastScore,bestScore,scoretext;
	public static Animator animMenu;
	//public int lvl;
	
	void Awake()
	{
		animMenu = GameObject.Find("Canvas").GetComponent<Animator>();
		scoretext = GameObject.Find("ScoreText").GetComponent<Text>();
		lastScore = GameObject.Find("LastScore").GetComponent<Text>();
		bestScore = GameObject.Find("BestScore").GetComponent<Text>();
		bestScore.text = "Best Score\n"+PlayerPrefs.GetInt("BestScore");
		CreateLvlsMass();
	} 
	public static void CreateLvlsMass()
	{		
		for (int i = 0; i < Resources.LoadAll("",typeof(UnityEngine.Object)).Where(el => el.name.Contains("Level_")).Count(); i++)
		{
			int a = i+1;
			Lvls.Add(Resources.Load("Level_"+a)as TextAsset);
		}
	}

	public void Play()
	{
		score = 0;
		animMenu.Play("MenuAnimStart");		
		GenerationLr();
		scoretext.text = "Score:"+score;
		isPlay = true;		
	}
	public void GameOver()
	{					
		animMenu.Play("MenuAnimFinish");		
		isPlay = false;
		Destroy(RecognizerController.cursor_temp);
		lr_temp.GetComponent<LineRenderer>().positionCount = 0;

		Camera.main.GetComponent<RecognizerController>().temps.Clear();
		GameObject.Find("TextButtonPlay").GetComponent<Text>().text = "Play more";
		lastScore.text = "Last Score\n"+score;
		if(score > PlayerPrefs.GetInt("BestScore"))
		{
			PlayerPrefs.SetInt("BestScore",score);
		}
		bestScore.text = "Best Score\n"+PlayerPrefs.GetInt("BestScore");
	}
	public void NextFigure()
	{
		score = score + 1;
		GameObject.Find("ScoreText").GetComponent<Text>().text = "Score:"+score; 
		GenerationLr();
	}

	void GenerationLr()
	{
		if(lr_temp != null)
		{
			lr_temp.GetComponent<Animator>().Play("Lr_animEnd");
			Destroy(lr_temp,1.2f);
		}
		lrMass.Clear();
		lr_temp = Instantiate(lr);
		//lr_temp = 
		if(Lvls.Count == 0)
			CreateLvlsMass();
			
		int lvl = Random.Range(0,Lvls.Count-1);
		//print(lvl);	
		ParseTextAsset(lvl,lr_temp.GetComponent<LineRenderer>());
		Lvls.RemoveAt(lvl);
	}
	
	void ParseTextAsset(int lvl, LineRenderer lr_Temp )
	{	
		char vector = ';';
		char xy = ',';
        string s = Lvls[lvl].text.TrimEnd(new char[] { vector });
        string[] substrings = s.Split (vector);

        for (int i = 0; i < substrings.Length-1; i++) 
		{		
			string[] a = substrings[i].Split (xy);
			float x = float.Parse( a [0]);
			float y = float.Parse( a [1]);
			lr_Temp.positionCount += 1;
			lr_Temp.SetPosition(lr_Temp.positionCount-1,new Vector3(x*scale,y*scale,0));
			lrMass.Add(lr_Temp.GetPosition(lr_Temp.positionCount-1));
		}
		float w = 0.2f* scale;
		lr_Temp.startWidth = w;
		lr_Temp.endWidth = w;
		
	}

}
