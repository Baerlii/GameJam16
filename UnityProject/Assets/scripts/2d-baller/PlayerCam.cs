﻿using UnityEngine;
using System.Collections;

public class PlayerCam : MonoBehaviour {

	private GameObject player;
	public float camDistance;

	// Use this for initialization
	void Start () {
		player = GameObject.Find("Player");
	}

	void FixedUpdate(){
		transform.position = new Vector3(this.player.transform.position.x, this.player.transform.position.y, this.player.transform.position.z-camDistance);
	}
}