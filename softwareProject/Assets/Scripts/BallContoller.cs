using UnityEngine;
using System.Collections;
using System;

public class BallContoller : MonoBehaviour {

 	public Rigidbody2D rb ;
	public Vector3 startPos;
	public Vector3 direction, endPos;
	private DateTime dt1,dt2;
	private double touchPow=0;

	public GameObject progress;
	public GameObject ok;

	public bool touching=false;
	// Use this for initialization
	void Start () {
		rb=  GetComponent<Rigidbody2D>();
	}
	
	public Vector3 getVel(){ 
		return rb.velocity;
	}

	// Update is called once per frame
	void Update () {
		
		if(Input.GetMouseButtonDown(0)){
			startPos= Input.mousePosition;
			dt1=DateTime.Now;
			touching=true;
		}
		if(Input.GetMouseButtonUp(0)){
			//mouse'u bıraktı, direction'ı ikisi arasındaki farktan hesapla		
			dt2=DateTime.Now;	
			direction= Input.mousePosition-startPos;
			onTouchEnd();
			touching=false;
		}
		resizeProgress();
		resizeOk();
	}
	void moveBallToClosest(){

	}

	void resizeProgress(){
		if(touching){
			//touchPow=Convert.ToSingle((dt2-dt1).TotalMilliseconds)*1f;
			progress.transform.localScale+=new Vector3(0.1F, 0, 0);
			
		}else{
			progress.transform.localScale=new Vector3(0f,1f,0f);
		}
		progress.transform.position=new Vector3(this.transform.position.x,this.transform.position.y-1);
	}
	void resizeOk(){
		if(touching){
			ok.active=true;	
			direction= Input.mousePosition-startPos;		
         	float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
         	ok.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
		}else{
			ok.active=false;
		}
		ok.transform.position=transform.position;
	}
	//oyuncu mouse'u bırakırsa
	private void onTouchEnd(){
			touchPow=Convert.ToSingle((dt2-dt1).TotalMilliseconds)*1f;
			direction.Normalize();	
			Push(direction.x*5*(float)touchPow,direction.y*5*(float)touchPow);
	}
	
	public void Push(float xF,float yF){
		Vector3 v3Force =new Vector3(xF,yF,0);
 		rb.AddForce(v3Force * Time.deltaTime);
	}
}
