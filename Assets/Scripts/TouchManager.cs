using UnityEngine;
using System.Collections;

public class TouchManager : MonoBehaviour {

	Vector3 startPos = Vector3.zero;
	Vector3 endPos = Vector3.zero;

	public FlickVector FlickProcess() {
		FlickVector flick = FlickVector.NULL;
		

		if(Input.GetMouseButtonDown(0)){
			startPos = Input.mousePosition;
		}

		if(Input.GetMouseButtonUp(0)){
			endPos = Input.mousePosition;

			flick = FlickDirection(startPos, endPos);
		}
		return flick;
	}

	public FlickVector FlickDirection(Vector3 start, Vector3 end) {
		FlickVector result = FlickVector.NULL;
		float directionX = end.x - start.x;
		float directionY = end.y - start.y;
		Debug.Log(end.x + " : " + start.x);Debug.Log(end.y + " : " + start.y);

		if(Mathf.Abs(directionX) > Mathf.Abs(directionY)) {
			if(directionX > 30) {
				result = FlickVector.RIGHT;
			} else if(directionX < -30) {
				result = FlickVector.LEFT;
			}
		} else if(Mathf.Abs(directionX) < Mathf.Abs(directionY)) {
			if(directionY > 30) {
				result = FlickVector.UP;
			} else if(directionY < -30) {
				result = FlickVector.DOWN;
			}
		}

		Debug.Log(result + " : " + directionX + "  " + directionY);
		return result;
	}

}

public enum FlickVector {
	NULL,
	UP,
	DOWN,
	LEFT,
	RIGHT
}