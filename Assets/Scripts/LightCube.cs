using UnityEngine;
using System.Collections;

public class LightCube : MonoBehaviour {

	// Use this for initialization
	void Start () {
		transform.position = ParentCube.Instance.InitPositionCube().transform.position;
		StartCoroutine(TransScale(Vector3.zero, Vector3.one));
	}

	private IEnumerator TransScale(Vector3 start, Vector3 target) {
		float time = 0;
		float moveFrame = 10;
		while(true) {
			transform.localScale = Vector3.Lerp(start, target, time/moveFrame);
			time += 1;
			yield return new WaitForSeconds(0.01f);
			if(time >= moveFrame) {
				transform.localScale = target;
				break;
			}
		}
	}
}
