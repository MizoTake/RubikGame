using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ParentCube : SingletonMonoBehaviour<ParentCube> {

	public static float DISTANCE = 1.5f;
	public static int ONE_SIZE = 2;
	private const int MAX_OBJECTS = 27;
	private const int CENTER_OBJECTS = 7;
	private GameObject[] objects = new GameObject[MAX_OBJECTS];
	public GameObject[] centers = new GameObject[CENTER_OBJECTS];
	public Material mat;

	// Use this for initialization
	void Start () {
		var count = 0;
		var centerCount = 0;
		for(int x = -1; x<ONE_SIZE; x++) {
			for(int y = -1; y<ONE_SIZE; y++) {
				for(int z = -1; z<ONE_SIZE; z++) {
					var instanceVector = new Vector3(x * DISTANCE, y * DISTANCE, z * DISTANCE);
					var obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
					obj.transform.position = instanceVector;
					obj.AddComponent<Cube>();
					obj.name = "Cube " + count;
					obj.GetComponent<Cube>().index = count;
					obj.GetComponent<MeshRenderer>().material = mat;
					if((x == 0 && y == 0 && z != 0) || (x == 0 && y == 0 && z == 0) ||
						(x != 0 && y == 0 && z == 0) || (x == 0 && y != 0 && z == 0)) {
						obj.GetComponent<Cube>().center = true;
						centers[centerCount] = obj;
						centerCount += 1;
					}
					obj.transform.parent = gameObject.transform;
					objects[count] = obj;
					count += 1;
				}
			}
		}

		var list = objects.ToList();
		var centerList = centers.ToList();
		centerList.ForEach(_ => {
			var cube = _.GetComponent<Cube>();
			var x = list.GetRange(cube.index - 4, 9);
			x.Remove(_);
			cube.aroundX = x;
			if(cube.index - 10 > 0 && cube.index + 10 < MAX_OBJECTS) {
				var y = list.GetRange(cube.index - 10, 3);
				y.AddRange(list.GetRange(cube.index - 1, 3));
				y.AddRange(list.GetRange(cube.index + 8, 3));
				y.Remove(_);
				cube.aroundY = y;

				int[] numZ = {cube.index - 12, cube.index - 9, cube.index - 6};
				var z = new List<GameObject>();
				for(int i = 0; i<3; i++) {
					for(int j = 0; j<3; j++) {
						if(numZ[j] > MAX_OBJECTS) continue;
						z.Add(list[numZ[j]]);
						numZ[j] += 9;
					}
				}
				cube.aroundZ = z;
			}
		});
	}

	public GameObject InitPositionCube() {
		var finish = false;
		var randomIndex = 0;
		do{
			randomIndex = Random.Range(0, MAX_OBJECTS);
			if(randomIndex != 13) {
				finish = true;
			}
		}while(!finish); 		
		return objects[randomIndex];
	}
	
}
