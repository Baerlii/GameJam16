using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	private const float PLAYER_ROTATION_SPEED = 250.0f;
	private const float PLAYER_MOVEMENT_SPEED = 5.0f;
	private const float PLAYER_MAX_HEALTH = 100.0f;
	private const float PLAYER_HEALTH_REG = 0.05f;
	private float playerHealth = 100.0f;
	private const float BULLET_SPEED = 17.0f;
	private const float WEAPON_COOLDOWN = 0.3f;

	private bool ON_COOLDOWN = false;
	private bool SHOW_HEALTH = false;

	private GameObject playerGreenHealh;
	private GameObject playerHealthBar;
	public GameObject bullet;

	void Start(){
		playerHealthBar = GameObject.Find ("Player/healthbar");
		playerGreenHealh = GameObject.Find ("Player/healthbar/foreground");
	}

	// Update is called once per frame
	void FixedUpdate () {

		if (playerHealth < PLAYER_MAX_HEALTH) {
			playerHealth += PLAYER_HEALTH_REG;
			playerGreenHealh.transform.localScale = new Vector3(playerHealth / 100.0f, playerGreenHealh.transform.localScale.y, playerGreenHealh.transform.localScale.z);
		}

		if (SHOW_HEALTH) {
			playerHealthBar.SetActive (true);
		} else {
			playerHealthBar.SetActive (false);
		}

		if (Input.GetKey (KeyCode.RightArrow)) {
			transform.Rotate (-Vector3.forward * Time.deltaTime * PLAYER_ROTATION_SPEED);
		}else if(Input.GetKey (KeyCode.LeftArrow)) {
			transform.Rotate (Vector3.forward * Time.deltaTime * PLAYER_ROTATION_SPEED);
		}

		if(Input.GetKey (KeyCode.UpArrow)) {
			transform.localPosition += transform.up * Time.deltaTime * PLAYER_MOVEMENT_SPEED;
		}else if(Input.GetKey (KeyCode.DownArrow)) {
			transform.localPosition -= transform.up * Time.deltaTime * PLAYER_MOVEMENT_SPEED;
		}

		if (Input.GetKey (KeyCode.Space) && !ON_COOLDOWN) {
			GameObject bulletInstance = Instantiate(bullet, transform.position, Quaternion.Euler(new Vector3(0, 0, 0))) as GameObject;
			bulletInstance.GetComponent<Rigidbody2D>().velocity = transform.up * BULLET_SPEED;
			Physics2D.IgnoreCollision(bulletInstance.GetComponent<Collider2D>(), GetComponent<Collider2D>());
			StartCoroutine ("Wait");
		}
	}

	void TakeDamage(float damage){
		playerHealth -= damage;
		if (playerHealth >= 0) {
			StartCoroutine ("showHealthBar");
			playerGreenHealh.transform.localScale = new Vector3(playerHealth / 100.0f, playerGreenHealh.transform.localScale.y, playerGreenHealh.transform.localScale.z);
		} else {
			Debug.Log ("Player dead!");
		}
	}

	IEnumerator showHealthBar(){
		SHOW_HEALTH = true;
		yield return new WaitForSeconds(WEAPON_COOLDOWN * 6.5f);
		SHOW_HEALTH = false;
	}

	IEnumerator Wait() {
		ON_COOLDOWN = true;
		yield return new WaitForSeconds(WEAPON_COOLDOWN);
		ON_COOLDOWN = false;
	}
}
