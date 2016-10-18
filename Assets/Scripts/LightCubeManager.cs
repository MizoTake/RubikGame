using UnityEngine;
using System.Collections;

public class LightCubeManager : SingletonMonoBehaviour<LightCubeManager> {

	public GameObject[] lightCubes;

	// Use this for initialization
	void Start () {
		for(int i = 0; i<lightCubes.Length; i++) {
			var target = ParentCube.Instance.InitPositionCube().transform;
			for(int j = 0; j<i; j++) {
				if(target == lightCubes[j].transform.parent) {
					j = 0;
					target = ParentCube.Instance.InitPositionCube().transform;
					continue;
				}
			}
			lightCubes[i].transform.position = target.position;
			lightCubes[i].transform.parent = target;
			lightCubes[i].GetComponent<LightCube>().Index = target.GetComponent<Cube>().index;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
