using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public float playerRotationSpeed;
	public float playerMovementSpeed;

	public GameObject bullet;
	public float bulletSpeed;
	public float weaponCooldown;

	private bool ON_COOLDOWN = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (Input.GetKey (KeyCode.RightArrow)) {
			transform.Rotate (-Vector3.forward * Time.deltaTime * this.playerRotationSpeed);
		}else if(Input.GetKey (KeyCode.LeftArrow)) {
			transform.Rotate (Vector3.forward * Time.deltaTime * this.playerRotationSpeed);
		}

		if(Input.GetKey (KeyCode.UpArrow)) {
			transform.localPosition += transform.up * Time.deltaTime * this.playerMovementSpeed;
		}else if(Input.GetKey (KeyCode.DownArrow)) {
			transform.localPosition -= transform.up * Time.deltaTime * this.playerMovementSpeed;
		}

		if (Input.GetKey (KeyCode.Space) && !ON_COOLDOWN) {
			GameObject bulletInstance = Instantiate(this.bullet, transform.position, Quaternion.Euler(new Vector3(0, 0, 0))) as GameObject;
			bulletInstance.GetComponent<Rigidbody2D>().velocity = transform.up * this.bulletSpeed;
			Physics2D.IgnoreCollision(bulletInstance.GetComponent<Collider2D>(), GetComponent<Collider2D>());
			StartCoroutine ("Wait");
		}
	}

	IEnumerator Wait() {
		ON_COOLDOWN = true;
		yield return new WaitForSeconds(weaponCooldown);
		ON_COOLDOWN = false;
	}
}
