using UnityEngine;
using System.Collections;

public class BulletController : MonoBehaviour {

	public float bulletSpeed;
	private float lifetime;

	// Use this for initialization
	void Start () {
		lifetime = Time.time;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
	
		if (lifetime + 5.0f <= Time.time) {
			Destroy (gameObject);
		}

	}

	void OnCollisionEnter2D(Collision2D coll) {
		if (coll.gameObject.tag == "Enemy") {
			coll.gameObject.SendMessage ("TakeDamage", 20);
		}else if (coll.gameObject.tag == "Player") {
			coll.gameObject.SendMessage ("TakeDamage", 10);
		}
		Destroy (gameObject);
	}

}
