using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class AIController : MonoBehaviour {

	public List<GameObject> o1List,o2List;
	public List<Vector3> orginList1, orginList2, attackList1, attackList2, defList1, defList2, adefList1, adefList2, aattackList1, aattackList2;
	public GameObject ball, o2,o3,o4,o5,o7,o8,o9,o11, o1Closest,o2Closest;
	public BallContoller ballCont;
	public enum Takım{
		RED,BLUE,NONE
	}
	public enum Durum{
		ACIL_DEFANS,
		ACIL_ATAK,
		SAKIN,
		DEFANS,
		ATAK
	}
	public Takım whoHasTheBall=Takım.RED; 
	
	// Use this for initialization
	void Start () {
		ballCont= ball.GetComponent<BallContoller>();
		o1List=new List<GameObject>(new GameObject[]{o2,o3,o4,o5});
		o2List=new List<GameObject>(new GameObject[]{o7,o8,o9,o11});
		orginList1=new List<Vector3>();
		orginList2=new List<Vector3>();
		for(int i=0;i<o1List.Count;i++){
			orginList1.Add(new Vector3(o1List[i].transform.position.x,o1List[i].transform.position.y));
		}
		for(int i=0;i<o2List.Count;i++){
			orginList2.Add(new Vector3(o2List[i].transform.position.x,o2List[i].transform.position.y));
		}
		for(int i=0;i<o1List.Count;i++){
			attackList1.Add(new Vector3(o1List[i].transform.position.x-4,o1List[i].transform.position.y));
		}
		for(int i=0;i<o2List.Count;i++){
			attackList2.Add(new Vector3(o2List[i].transform.position.x+4,o2List[i].transform.position.y));
		}
		for(int i=0;i<o1List.Count;i++){
			aattackList1.Add(new Vector3(o1List[i].transform.position.x-7,o1List[i].transform.position.y));
		}
		for(int i=0;i<o2List.Count;i++){
			aattackList2.Add(new Vector3(o2List[i].transform.position.x+7,o2List[i].transform.position.y));
		}
		for(int i=0;i<o1List.Count;i++){
			defList1.Add(new Vector3(o1List[i].transform.position.x+2,o1List[i].transform.position.y));
		}
		for(int i=0;i<o2List.Count;i++){
			defList2.Add(new Vector3(o2List[i].transform.position.x-2,o2List[i].transform.position.y));
		}
		for(int i=0;i<o1List.Count;i++){
			adefList1.Add(new Vector3(o1List[i].transform.position.x+1,o1List[i].transform.position.y));
		}
		for(int i=0;i<o2List.Count;i++){
			adefList2.Add(new Vector3(o2List[i].transform.position.x-1,o2List[i].transform.position.y));
		}
	}
	
	// Update is called once per frame
	void Update () {

		var ballPos =ball.transform.position.x;
		if(whoHasTheBall==Takım.RED){ //top kırmızıda mavi defans
			if(ballPos <-5){
				moveTakım(Takım.BLUE,Durum.ACIL_DEFANS);
			}else if(ballPos >-5 && ballPos < -1){
				moveTakım(Takım.BLUE,Durum.DEFANS);
			}else if(ballPos > -1){
				moveTakım(Takım.BLUE,Durum.SAKIN);
			}	
			//KIRMIZI ATAK
			if(ballPos < -2){
				moveTakım(Takım.RED,Durum.ACIL_ATAK);
			}else{
				moveTakım(Takım.RED,Durum.ATAK);
			}		
		}else{
			///MAVI ATAK
			if(ballPos >2){
				moveTakım(Takım.BLUE,Durum.ACIL_ATAK);
			}else{
				moveTakım(Takım.BLUE,Durum.ATAK);
			}
			//KIRMIZI DEFANS
			if(ballPos > 5){
				moveTakım(Takım.RED,Durum.ACIL_DEFANS);
			}else if(ballPos < 5 && ballPos > 1){
				moveTakım(Takım.RED,Durum.DEFANS);
			}else if(ballPos < 1){
				moveTakım(Takım.RED,Durum.SAKIN);
			}	
		}
		//neredeyse durmuş
		moveBallToClosest();
		pressYap();

	}
	void moveTakım(Takım t,Durum d){
		var list_to_use = t==Takım.RED ? o1List : o2List;
		var originList= t==Takım.RED ? orginList1 : orginList2;
		var defList= t==Takım.RED ? defList1 : defList2;
		var adefList= t==Takım.RED ? adefList1 : adefList2;
		var attackList= t==Takım.RED ? attackList1 : attackList2;
		var aattackList= t==Takım.RED ? aattackList1 : aattackList2;

		switch (d)
		{			
			case Durum.ACIL_ATAK: 
				for(int i=0;i<list_to_use.Count;i++){					
					moveOneTo(list_to_use[i],aattackList[i]);
				}
			break;
			case Durum.ACIL_DEFANS: 
				Debug.Log("ACIL DEFANS");
				for(int i=0;i<list_to_use.Count;i++){					
					moveOneTo(list_to_use[i],ball.transform.position);
				}
			break;
			case Durum.SAKIN: 
				Debug.Log("SAKİN");
				for(int i=0;i<list_to_use.Count;i++){					
					moveOneTo(list_to_use[i],originList[i]);
				}
			break;
			case Durum.DEFANS: 
				Debug.Log("DEFANS");
				for(int i=0;i<list_to_use.Count;i++){					
					moveOneTo(list_to_use[i],defList[i]);
				}
			break;
			case Durum.ATAK: 
				for(int i=0;i<list_to_use.Count;i++){					
					moveOneTo(list_to_use[i],attackList[i]);
				}
			break;
		}
	}
	void moveOneTo(GameObject g,Vector3 v){
		Vector3 force =(v-g.transform.position);
		if(force.sqrMagnitude > 1){ 
			force.Normalize();
			force*=3;
			g.GetComponent<Rigidbody2D>().velocity=force;
		}else{
			g.GetComponent<Rigidbody2D>().velocity=force*3;
		}
		
	}
	void pressYap(){
		if(ball.transform.position.x < 14 && ball.transform.position.x > -14){
			moveOneTo(o1Closest,ball.transform.position+new Vector3(-2,0,0));
			moveOneTo(o2Closest,ball.transform.position+new Vector3(2,0,0));
		}else{
			moveTakım(Takım.BLUE,Durum.SAKIN);
			moveTakım(Takım.RED,Durum.SAKIN);
		}		
	}

	void moveBallToClosest(){		 
		 Vector3 position = ball.transform.position;
		 o1Closest=getClosest(o1List);
		 o2Closest=getClosest(o2List);
		 var distance1=(o1Closest.transform.position-position).magnitude;
		 var distance2=(o2Closest.transform.position-position).magnitude;
		 if(ballCont.getVel().magnitude > 3 ) return;
		 if( distance1 < distance2 && distance1 < 2.1f){			 
			 	ball.transform.position=new Vector3(o1Closest.transform.position.x-1.3f,o1Closest.transform.position.y,-0.1f) ;
			 	whoHasTheBall=Takım.RED; 
			 	o1Closest.GetComponent<Rigidbody2D>().velocity=new Vector3(0,0,0);				
		 }else if(distance2 < 2.1f){		
				 ball.transform.position=new Vector3(o2Closest.transform.position.x+1.3f,o2Closest.transform.position.y,-0.1f) ;
				 whoHasTheBall=Takım.BLUE; 
			 	 o2Closest.GetComponent<Rigidbody2D>().velocity=new Vector3(0,0,0);	
		 }else{
			 //whoHasTheBall=Takım.NONE;			 
		 }
	}
	GameObject getClosest(List<GameObject> list){
		Vector3 position = ball.transform.position;
		var distance=Mathf.Infinity;
		var selectedOne=list[0];
		foreach(GameObject t in list){
			 var _dist =(t.transform.position-position).sqrMagnitude;
			 if(_dist<distance){
				 distance=_dist;
				 selectedOne=t;
			 }
		 }
		 return selectedOne;
	}
}
