using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour {

	private const float ENEMY_ROTATION_SPEED = 360.0f;
	private const float ENEMY_MOVEMENT_SPEED = 6.0f;
	private const float ENEMY_MAX_HEALTH = 100.0f;
	private const float ENEMY_HEALTH_REG = 0.01f;
	private float enemyHealth = 100.0f;
	private const float BULLET_SPEED = 10.0f;
	private const float WEAPON_COOLDOWN = 3.0f;

	private bool ON_COOLDOWN = false;
	private bool SHOW_HEALTH = false;

	private GameObject player;
	private GameObject enemyHealthBar;
	private GameObject enemyGreenHealh;
	public GameObject bullet;


	void Start(){
		player = GameObject.Find ("Player");
		enemyHealthBar = GameObject.Find ("Enemy/healthbar");
		enemyGreenHealh = GameObject.Find ("Enemy/healthbar/foreground");
	}

	// Update is called once per frame
	void FixedUpdate () {

		if (enemyHealth < ENEMY_MAX_HEALTH) {
			enemyHealth += ENEMY_HEALTH_REG;
			enemyGreenHealh.transform.localScale = new Vector3(enemyHealth / 100.0f, enemyGreenHealh.transform.localScale.y, enemyGreenHealh.transform.localScale.z);
		}

		LookAtPlayer ();

		if (SHOW_HEALTH) {
			enemyHealthBar.SetActive (true);
		} else {
			enemyHealthBar.SetActive (false);
		}

		if (!ON_COOLDOWN) {
			GameObject bulletInstance = Instantiate(bullet, transform.position, Quaternion.Euler(new Vector3(0, 0, 0))) as GameObject;
			bulletInstance.GetComponent<Rigidbody2D>().velocity = transform.up * BULLET_SPEED;
			Physics2D.IgnoreCollision(bulletInstance.GetComponent<Collider2D>(), GetComponent<Collider2D>());
			StartCoroutine ("Wait");
		}
	}

	void LookAtPlayer (){

		Vector3 targetDistance = (player.transform.position - transform.position);
		float angle = (Mathf.Atan2(targetDistance.y, targetDistance.x) * Mathf.Rad2Deg)-90.0f;
		Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);
		transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, ENEMY_ROTATION_SPEED * Time.deltaTime);

	}

	void TakeDamage(float damage){
		enemyHealth -= damage;
		if (enemyHealth > 0) {
			StartCoroutine ("showHealthBar");
			enemyGreenHealh.transform.localScale = new Vector3(enemyHealth / 100.0f, enemyGreenHealh.transform.localScale.y, enemyGreenHealh.transform.localScale.z);
		} else {
			Destroy (gameObject);
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