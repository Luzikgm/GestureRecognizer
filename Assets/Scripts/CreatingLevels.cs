using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;

public class CreatingLevels : MonoBehaviour {

	 public List<Vector3> temp = new List<Vector3>();
	 private LineRenderer lr;

	void Awake()
	{
		lr = GameObject.Find("LineRenderer").GetComponent<LineRenderer>();
	}

	void Update()
	{
		 if (Input.GetMouseButtonDown(0))
            {
                RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
                if (hit.collider != null)
                {					
					if(temp.Count > 0)
					{
						if(temp[temp.Count-1] != hit.collider.gameObject.transform.localPosition)
						{ 
							temp.Add(hit.collider.gameObject.transform.localPosition);							
						}
					}
					else
					{
						temp.Add(hit.collider.gameObject.transform.localPosition);
					}
					lr.positionCount += 1;
					lr.SetPosition(lr.positionCount-1,temp[temp.Count-1]);
				}
			}
	}
	void OnGUI()
	{
		if(GUI.Button(new Rect(10,150,100,100), "save"))
		{
		  
		   bool save = true;
		   Vector2 c = (temp[1]-temp[0]).normalized;	
			if(c.y == 1)
			{
				Vector2 d = (temp[2]-temp[1]).normalized;
				if(d.x > 0)
					save = true;	
				else
					save = false;
			}
			else
			if(c.y == -1)
			{
				save = false;				
			}
			else
			{
				if(c.x > 0)
					save = true;	
				else
					save = false;
			}	
		
			if(save)
			{
				int a = Resources.LoadAll("",typeof(UnityEngine.Object)).Where(el => el.name.Contains("Level_")).Count();
				int b = a+1;
				StreamWriter sw = new StreamWriter("Assets/Resources/Level_"+ b +".txt"); // Создаем файл
				for (int i = 0; i < temp.Count; i++)
				{
					sw.WriteLine(temp[i].x+","+temp[i].y+";");// Пишем координаты
				}
				Debug.Log("Save");
				sw.Close(); // Закрываем(сохраняем)
				temp.Clear();
				lr.positionCount = 0;
		#if UNITY_EDITOR
				UnityEditor.AssetDatabase.Refresh ();
		#endif
			}
			else
			{
				temp.Clear();
				lr.positionCount = 0;
				print("Save Failed");
			}
		}

		if(GUI.Button(new Rect(10,10,100,50), "ReadMe"))
		{
			Application.OpenURL("https://docs.google.com/document/d/1eW6t0k-HeL8DkIaGssMWzPrmHYsolxVNlZcGdDBvAnQ/edit");
		}
	}
}
