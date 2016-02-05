using UnityEngine;
using System.Collections;

public class BulletController : MonoBehaviour {

	public float bulletSpeed;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
	
		//transform.localPosition += transform.up * Time.deltaTime * this.bulletSpeed;

	}

	void OnCollisionEnter2D(Collision2D coll) {
		Debug.Log ("Treffer");
		//if (coll.gameObject.tag == "Enemy")
			//coll.gameObject.SendMessage("ApplyDamage", 10);

	}

}
