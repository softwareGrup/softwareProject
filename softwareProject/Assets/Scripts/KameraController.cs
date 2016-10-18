using UnityEngine;
using System.Collections;

public class KameraController : MonoBehaviour {

	public GameObject ball;
	// Use this for initialization
	void Start () {
		Screen.orientation = ScreenOrientation.LandscapeLeft;
	}
	
	// Update is called once per frame
	void Update () {
		//this.transform.position= new Vector3(ball.transform.position.x,ball.transform.position.y,-15f);
	}
}
