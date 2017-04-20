using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class RecognizerController : MonoBehaviour {

	[HideInInspector]
	public GameObject cursor;
	private bool isClockwise,isnTrueFigure;
	public List<Vector2> temps = new List<Vector2>();
	private Vector2 temp;
	public static  GameObject cursor_temp;
	private int State;
	public float errorRange;
	public LineRenderer LrHelper;
	void Update () 
	{
		if(GameController.isPlay)
		{		
			if(Input.GetMouseButtonDown(0))
			{
				temps.Add(Camera.main.ScreenToWorldPoint(Input.mousePosition));
				temp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
				cursor_temp = Instantiate(cursor,new Vector2(temp.x,temp.y),Quaternion.identity);
				State = (int)SwipeState.waiting;
			}
			if(Input.GetMouseButton(0))
			{
				Vector2 a = Camera.main.ScreenToWorldPoint(Input.mousePosition);
				//arrow.transform.position = new Vector2(temp.x, temp.y);
				cursor_temp.transform.position = new Vector3(a.x, a.y,0);

				float abs_x = Mathf.Abs(temp.x - a.x);
				float abs_y = Mathf.Abs(temp.y- a.y);

				if(abs_x >= errorRange && abs_y < errorRange/2)//horizontal
				{								
					if(abs_y < errorRange/3)
					{
						if(State == (int)SwipeState.waiting)
							State = (int)SwipeState.horizontal;
						else
						if(State != (int)SwipeState.horizontal)
						{							
							if(Mathf.Abs(temps[temps.Count-1].x - temp.x) > errorRange*2 || Mathf.Abs(temps[temps.Count-1].y - temp.y) > errorRange*2 )
								temps.Add(temp);
							State = (int)SwipeState.horizontal;																			
						}					
					}
					temp = Camera.main.ScreenToWorldPoint(Input.mousePosition);							
				}
				else
				if(abs_x < errorRange/2&& abs_y >= errorRange)//vertical
				{					
					if(abs_x < errorRange/3)
					{	
						if(State == (int)SwipeState.waiting)
							State = (int)SwipeState.vertical;
						else
						if(State != (int)SwipeState.vertical)
						{	
							if(Mathf.Abs(temps[temps.Count-1].x - temp.x) > errorRange*2  || Mathf.Abs(temps[temps.Count-1].y - temp.y) > errorRange*2 )		
								temps.Add(temp);
							State = (int)SwipeState.vertical;
						}					
					}
					temp = Camera.main.ScreenToWorldPoint(Input.mousePosition);					
				}

				if(abs_x >= errorRange/2 && abs_y >= errorRange/2)//angle	
				{
					//print("angle");
					if(a.y - temp.y  > 0)//angle_up
					{
						if(a.x - temp.x > 0)
						{	
								//print("angle_up_right");					
							if(State == (int)SwipeState.waiting)
								State = (int)SwipeState.angle_up_right;
							else
							if(State != (int)SwipeState.angle_up_right)
							{			
								if(Mathf.Abs(temps[temps.Count-1].x - temp.x) > errorRange*2  || Mathf.Abs(temps[temps.Count-1].y - temp.y) > errorRange*2 )						
									temps.Add(temp);
								State = (int)SwipeState.angle_up_right;
							}	
							//temp = Camera.main.ScreenToWorldPoint(Input.mousePosition);				
						}
						else
						{
							//print("angle_up_left");
							if(State == (int)SwipeState.waiting)
								State = (int)SwipeState.angle_up_left;
							else
							if(State != (int)SwipeState.angle_up_left)
							{	
								if(Mathf.Abs(temps[temps.Count-1].x - temp.x) > errorRange*2 || Mathf.Abs(temps[temps.Count-1].y - temp.y) > errorRange*2 )									
									temps.Add(temp);
								State = (int)SwipeState.angle_up_left;
							}
							//temp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
						}
						temp = Camera.main.ScreenToWorldPoint(Input.mousePosition);						
					}
					else // angel_down
					{
						if(a.x - temp.x > 0)
						{
								//print("angle_down_right");

							if(State == (int)SwipeState.waiting)
								State = (int)SwipeState.angle_down_right;
							else
							if(State != (int)SwipeState.angle_down_right)
							{		
								if(Mathf.Abs(temps[temps.Count-1].x - temp.x) > errorRange*2 || Mathf.Abs(temps[temps.Count-1].y - temp.y) > errorRange*2)										
									temps.Add(temp);
								State = (int)SwipeState.angle_down_right;	
							}	
							//temp = Camera.main.ScreenToWorldPoint(Input.mousePosition);					
						}
						else
						{
								//print("angle_down_left");
								
							if(State == (int)SwipeState.waiting)
								State = (int)SwipeState.angle_down_left;
							else
							if(State != (int)SwipeState.angle_down_left)
							{	
								if(Mathf.Abs(temps[temps.Count-1].x - temp.x) > errorRange*2 || Mathf.Abs(temps[temps.Count-1].y - temp.y) > errorRange*2)										
									temps.Add(temp);
								State = (int)SwipeState.angle_down_left;
							}	
							//temp = Camera.main.ScreenToWorldPoint(Input.mousePosition);						
						}
						temp = Camera.main.ScreenToWorldPoint(Input.mousePosition);	
					}			
				}
			}
			if(Input.GetMouseButtonUp(0) && temps.Count!=0)
			{
				temps.Add(Camera.main.ScreenToWorldPoint(Input.mousePosition));

				if(GameController.lrMass[0] == GameController.lrMass[GameController.lrMass.Count-1])
				{
					Vector2 a = temps[0] - temps[temps.Count-1];
					if(Mathf.Abs(a.x) < errorRange && Mathf.Abs(a.y) < errorRange)
						GestureRecognizer();
					else
						{isnTrueFigure = true;print("Game Over : firs != last");
						}
				}
				else
					GestureRecognizer();	

				if(!isnTrueFigure)
				{
					LrHelper.positionCount = 0;
					TimerController.timer_countS = TimerController.timer_countS -1;
					TimerController.ResTimer = true;
					temps.Clear();
					Camera.main.GetComponent<GameController>().NextFigure();
					//GameController.NextFigure();	
				}			
				else
				{
					LineRenderer lr_temp = GameController.lr_temp.GetComponent<LineRenderer>();
					LrHelper.positionCount =  lr_temp.positionCount;
					for (int i = 0; i < lr_temp.positionCount; i++)
					{
						LrHelper.SetPosition(i,new Vector2(lr_temp.GetPosition(i).x*1.5f,lr_temp.GetPosition(i).y*1.5f));
					}
					temps.Clear();
					GameController.lr_temp.GetComponent<Animator>().Play("Lr_animFalseRes");
					isnTrueFigure = false;
				}
				Destroy(cursor_temp);	
			}
		}
	}
	//public Vector2[] ss;
	void GestureRecognizer()
	{
		Vector2[] recMass = new Vector2[GameController.lrMass.Count];
		//lrMass.RemoveAt(lrMass.Count-1);
		if(recMass.Length == temps.Count)	
		{	
			if(GameController.lrMass.Count > 2)
			{
				if(GameController.lrMass[0] != GameController.lrMass[GameController.lrMass.Count-1])
				{
					Vector2 a = (temps[1]-temps[0]).normalized;	
					if(a.y >= 0.8f || a.y <= -0.8f)
					{
						Vector2 b = (temps[2]-temps[1]).normalized;
						if(b.x >= 0)
							isClockwise = true;	
						else
							isClockwise = false;
					}
					else
					{
						if(a.x > 0)
							isClockwise = true;	
						else
							isClockwise = false;
					}	
				}
				else
				{
					Vector2 a = (temps[1]-temps[0]).normalized;		
					if(a.y >= 0.8f)
					{											
						Vector2 b = (temps[2]-temps[1]).normalized;
						if(b.x >= 0)
							isClockwise = true;	
						else
							isClockwise = false;							
					}
					else
					if(a.y <= -0.8f)
					{				
						Vector2 b = (temps[2]-temps[1]).normalized;
						if(b.x >= 0)
							isClockwise = false;	
						else
							isClockwise = true;
					}
					else
					{
						Vector2 b = (temps[2]-temps[1]).normalized;
						if(a.x >= 0)
						{						
							if(b.y <= 0)
							{
								isClockwise = true;
							}	
							else
								isClockwise = false;
						}
						else
						{
							if(b.y >= 0)
							{
								isClockwise = true;
							}	
							else
								isClockwise = false;
						}
					}
				}	
			}
			else
			{
				Vector2 a = (temps[1]-temps[0]).normalized;
				if(a.x >= 0 )
					isClockwise = true;	
				else				
					isClockwise = false;	
			}

		//	print(isClockwise);
			if(isClockwise)
			{						
				if(GameController.lrMass[0] == GameController.lrMass[GameController.lrMass.Count-1])
				{																							
					for (int i = 0; i < recMass.Length-1; i++)
					{						
						Vector2 dirVector = (temps[i+1]-temps[i]).normalized;					
						for (int j = 0; j < GameController.lrMass.Count-1; j++)
						{							
							Vector2 a = (GameController.lrMass[j+1]-GameController.lrMass[j]).normalized;
							//print("i="+i+ " der="+dirVector+" a="+a+ " x="+(a.x - dirVector.x)+" y="+(a.y - dirVector.y));								
							if((a.x - dirVector.x <= 0.25f && a.x - dirVector.x >= -0.25f) &&  (a.y - dirVector.y <= 0.25f && a.y - dirVector.y >= -0.25f))
							{		
								isnTrueFigure = false;											
								if(i==0)
								{																		
									recMass[0] = GameController.lrMass[j];
									recMass[recMass.Length-1] = recMass[0];	
									//lrMass.RemoveAt(j);								
									break;
								}
								else
								{
									recMass[i] = GameController.lrMass[j];
									break;
								}								
							}
							else
							isnTrueFigure = true;
						}
						if(isnTrueFigure)
						{
							print("angle error right");
							break;
						}
					}
				}
				else
				{					
					for (int i = 0; i < recMass.Length; i++)
					{
						recMass[i] = GameController.lrMass[i];
					}

					for (int i = 0; i < recMass.Length-1; i++)
					{											
						Vector2 dirVector = (temps[i+1]-temps[i]).normalized;					
						for (int j = 0; j < recMass.Length-1; j++)
						{						
							Vector2 a = (recMass[j+1]-recMass[j]).normalized;	
							//print("i="+i+ " der="+dirVector+" a="+a+ " x="+(a.x - dirVector.x)+" y="+(a.y - dirVector.y));							
							if((a.x - dirVector.x <= 0.25f && a.x - dirVector.x >= -0.25f) &&  (a.y - dirVector.y <= 0.25f && a.y - dirVector.y >= -0.25f))
							{
								isnTrueFigure = false;
								break;
							}
							else
							{
								isnTrueFigure = true;	
							}
						}
						if(isnTrueFigure)
						{
							print("angle error right");
							break;
						}
					}										
				}			
			}
			else
			{
				if(GameController.lrMass[0] == GameController.lrMass[GameController.lrMass.Count-1])
				{																					
					for (int i = 0 ; i < temps.Count-1; i++)
					{																		
						Vector2 dirVector = (temps[i+1]-temps[i]).normalized;					
						for (int j = GameController.lrMass.Count-1; j > 0; j--)
						{														
							Vector2 a = (GameController.lrMass[j-1]-GameController.lrMass[j]).normalized;	
							//print("i="+i+ " der="+dirVector+" a="+a+ " x="+(a.x - dirVector.x)+" y="+(a.y - dirVector.y));				
							if(a.x - dirVector.x <= 0.25f && a.x - dirVector.x >= -0.25f && a.y - dirVector.y <= 0.25f && a.y - dirVector.y >= -0.25f)
							{		
								isnTrueFigure = false;																		
								if(i==0)
								{																		
									recMass[0] = GameController.lrMass[j];
									recMass[recMass.Length-1] = recMass[0];									
									break;
								}
								else
								{
									recMass[i] = GameController.lrMass[j];
									break;
								}
							}
							else
							isnTrueFigure = true;
						}
						if(isnTrueFigure)
						{
							print("angle error left");
							break;
						}
					}
				}
				else
				{
					for (int i = 0; i < recMass.Length; i++)
					{
						recMass[i] = GameController.lrMass[(GameController.lrMass.Count-1)-i];
						//print(recMass[i]);
					}

					for (int i = 0; i < recMass.Length-1; i++)
					{						
						Vector2 dirVector = (temps[i+1]-temps[i]).normalized;					
						for (int j = 0; j < recMass.Length-1; j++)
						{						
							Vector2 a = (recMass[j+1]-recMass[j]).normalized;	
							//print("i="+i+ " der="+dirVector+" a="+a+ " x="+(a.x - dirVector.x)+" y="+(a.y - dirVector.y));							
							if((a.x - dirVector.x <= 0.25f && a.x - dirVector.x >= -0.25f) &&  (a.y - dirVector.y <= 0.25f && a.y - dirVector.y >= -0.25f))
							{
								isnTrueFigure = false;
								break;
							}
							else
							isnTrueFigure = true;
						}
						if(isnTrueFigure)
						{
							print("angle error left");
							break;
						}
					}	
				}	
			}
			
/*			for (int i = 0; i < recMass.Length; i++)
			{				
				ss[i] = recMass[i];
			}*/

			if(temps.Count > 2 && !isnTrueFigure) // Scale
			{	
				for (int i = 0; i < recMass.Length-2; i++)
				{							
					//float b = Vector2.Distance(recMass[i],recMass[i+1])/Vector2.Distance(recMass[i+1],recMass[i+2]);
					//float a = Vector2.Distance(temps[i],temps[i+1])/Vector2.Distance(temps[i+1],temps[i+2]);

					float a = Vector2.Distance(temps[i+1],temps[i]) / Vector2.Distance(temps[i+2],temps[i+1]);
					float b = Vector2.Distance(recMass[i+1],recMass[i]) / Vector2.Distance(recMass[i+2],recMass[i+1]) ;					
				
				//print("i="+i+" a="+a+" b="+b+"  a-b="+ (Mathf.Abs(a) - Mathf.Abs(b)));
					if(Mathf.Abs(Mathf.Abs(a) - Mathf.Abs(b)) < 0.85f)
					{
						isnTrueFigure = false;
						//break;
					}
					else
					{
						isnTrueFigure = true;
						break;
					}
				}
				if(isnTrueFigure)
				{
					print("Game Over : scale");
				}
			}
		}
		else
		{
			isnTrueFigure = true;print("Game Over : dods !=");
		}
	}
}

enum SwipeState
{
	waiting,
	horizontal,
	vertical,
	angle_up_right,
	angle_up_left,
	angle_down_right,
	angle_down_left
}
