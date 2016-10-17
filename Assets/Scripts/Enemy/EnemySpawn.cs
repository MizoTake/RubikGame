using UnityEngine;
using System.Collections;

public class EnemySpawn : MonoBehaviour {

	public GameObject spawnObject;
	public GameObject[] spawnPoint;
	private const int WAIT_TIME = 2;

	void Start () {
		StartCoroutine("SpawnTimer");
	}

	private IEnumerator SpawnTimer() {
		while(true) {
			yield return new WaitForSeconds(WAIT_TIME);
			GameObject point = spawnPoint[Random.Range(0, spawnPoint.Length)];
			Instantiate(spawnObject, point.transform.position, point.transform.rotation);
		}
	}
}
