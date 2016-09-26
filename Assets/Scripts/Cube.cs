using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UniRx;

public class Cube : MonoBehaviour {

	public int index { get; set; }
	public bool center { get; set; }
	public ReactiveProperty<bool> nowRotation = new ReactiveProperty<bool>(false);
	public List<GameObject> aroundX;
	public List<GameObject> aroundY;
	public List<GameObject> aroundZ;
	private const float waitSecons = 0.01f;
	private const float rotframe = 20f;

	/// <summary>
    /// 回転軸と回転方向を決定
    /// </summary>
    /// <param name="type"> X , Y ,Z</param>
    /// <param name="vec"> right, up, forward</param>
	public void Rot(ListType type, Vector3 vec) {
		StartCoroutine(LerpRotate(type, vec));
	}

	public IEnumerator LerpRotate(ListType type, Vector3 vec) {
		var targets = new List<GameObject>();
		var time = 0f;
		var initRot = transform.rotation;
		var addRot = vec * 90;
		var targetRot = new Quaternion(initRot.x + addRot.x, initRot.y + addRot.y, initRot.z + addRot.z, initRot.w);
		nowRotation.Value = true;

		switch(type) {
			case ListType.X :
				if(aroundX.Count <= 0) Debug.LogError("指定軸が間違っています。");
				targets = aroundX;
				break;
			case ListType.Y :
				if(aroundX.Count <= 0) Debug.LogError("指定軸が間違っています。");
				targets = aroundY;
				break;
			case ListType.Z :
				if(aroundX.Count <= 0) Debug.LogError("指定軸が間違っています。");
				targets = aroundZ;
				break;
		}

		targets.ForEach( _ => {
			_.transform.parent = gameObject.transform;
		});

		while(true) {
			transform.rotation = Quaternion.Slerp(initRot, targetRot, time/rotframe);
			time += 1;
			yield return new WaitForSeconds(waitSecons);
			if(time/rotframe >= 1) {
				transform.rotation = Quaternion.identity;
				break;
			}
		}

		targets.ForEach( _ => {
			_.transform.parent = transform.parent;
		});

		nowRotation.Value = false;

		yield return 0;
	}
}

public enum ListType {
		X,
		Y,
		Z
	}
