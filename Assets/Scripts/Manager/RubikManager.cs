using UnityEngine;
using System.Collections;

public class RubikManager : MonoBehaviour {

	public Material cubeShader;

	private const int ONE_SIDE = 3;
	private const float ONE_SIDE_DISTANCE = 1.5f;
	private GameObject[,,] rubikCube = new GameObject[3,3,3];

	// Use this for initialization
	void Start () {
		var parentObj = new GameObject("RubikCube");

		for(int i = 0; i<ONE_SIDE; i++)
		{
			for(int j = 0; j<ONE_SIDE; j++)
			{
				for(int k = 0; k<ONE_SIDE; k++)
				{
					Debug.Log(i + " " + j + " " + k);
					rubikCube[i, j, k] = GameObject.CreatePrimitive(PrimitiveType.Cube);
					rubikCube[i, j, k].transform.position = new Vector3(i - 1, j - 1, k - 1) * ONE_SIDE_DISTANCE;
					rubikCube[i, j, k].GetComponent<MeshRenderer>().material = cubeShader;
					rubikCube[i, j, k].transform.parent = parentObj.transform;
				}
			}
		}
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
