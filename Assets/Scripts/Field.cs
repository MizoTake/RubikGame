using UnityEngine;
using System.Collections;

public class Field : MonoBehaviour {

	public GameObject[] objects = new GameObject[9];
	private const int INIT_HEIGHT = 3;

	// Use this for initialization
	void Start () {
		var cnt = 0;
		for(int i = -1; i<ParentCube.ONE_SIZE; i++) {
			for(int j = -1; j<ParentCube.ONE_SIZE; j++) {
				var obj = new GameObject("filed" + cnt);
				obj.transform.position = new Vector3(i * ParentCube.DISTANCE, INIT_HEIGHT, j * ParentCube.DISTANCE);
				obj.transform.parent = transform;
				objects[cnt] = obj;
				cnt += 1;
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
