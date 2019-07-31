using UnityEngine;

public class CheckPointController : MonoBehaviour {

	private Transform cpTransform;

	public Transform spawnPoint;

	void Start () {
		this.cpTransform = GetComponent<Transform>();
		this.spawnPoint = GameObject.FindWithTag("SpawnPoint").transform;
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.CompareTag("Player")) {
			this.spawnPoint.position = this.cpTransform.position;
		}
	}

}
