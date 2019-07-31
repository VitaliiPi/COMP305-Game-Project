using UnityEngine;



public class BulletController : MonoBehaviour {

	public float speed;
	private float direction;

	void Start () {
		Destroy (gameObject, 1f); 
	}
	
	void Update () {
			this.transform.Translate (Vector3.right * Time.deltaTime * this.speed);
	}

	private void OnCollisionEnter2D(Collision2D other) {
		if (other.gameObject.CompareTag("Wall") || other.gameObject.CompareTag("Platform")) {
			Destroy(gameObject);
		}
	}

	public void DestroyBullet(){
		Destroy (gameObject);
	}
}
