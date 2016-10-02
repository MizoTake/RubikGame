using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UniRx;

public class Cube : MonoBehaviour {

	private const float waitSecons = 0.01f;
	private const float rotframe = 10f;
	public int index { get; set; }
	public bool center { get; set; }
	public ReactiveProperty<bool> nowRotation = new ReactiveProperty<bool>(false);
	public List<GameObject> aroundX;
	public List<GameObject> aroundY;
	public List<GameObject> aroundZ;
	private Side[] mySideInfo;
	private Dictionary<Side, Vector3> sideInfoManage = new Dictionary<Side, Vector3>() {
		{Side.up, Vector3.up},
		{Side.down, Vector3.down},
		{Side.left, Vector3.left},
		{Side.right, Vector3.right},
		{Side.forward, Vector3.forward},
		{Side.back, Vector3.back},
	};

	//以下 X Z の２次元配列は逆回転がしたい場合、添字を逆にとれば可能
	//回転した先のCube番号 : X軸が基軸
	private int[,] sendTargetCubeX = {
		{0, 6},
		{1, 3},
		{2, 0},
		{3, 7},
		{4, 4},
		{5, 1},
		{6, 8},
		{7, 5},
		{8, 2}
	};

	//回転した先のCube番号 : Z軸が基軸
	private int[,] sendTargetCubeZ = {
		{0, 18},
		{3, 9},
		{6, 0},
		{9, 21},
		{12, 12},
		{15, 3},
		{18, 24},
		{21, 15},
		{24, 6}
	};

	/// <summary>
    /// 回転軸と回転方向を決定
    /// </summary>
    /// <param name="type"> X , Y ,Z</param>
    /// <param name="vec"> right, up, forward</param>
	public void Rot(ListType type, Vector3 vec) {
		StartCoroutine(LerpRotate(type, vec));
	}

	public void ChangeSideInfo(Side[] sideInfo) {
		//float3を引数にして、Vector3から値を取得して代入？
		//Shaderならこれでいいかも？
		for(int i = 0; i<mySideInfo.Length; i++) {
			//消す
		}

		mySideInfo = sideInfo;
		for(int i = 0; i<mySideInfo.Length; i++) {
			//表示
		}
	}

	private IEnumerator LerpRotate(ListType type, Vector3 vec) {
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
			//実際の回転処理
			transform.rotation = Quaternion.Slerp(initRot, targetRot, time/rotframe);
			time += 1;
			yield return new WaitForSeconds(waitSecons);
			if(time/rotframe >= 1) {
				//初期化処理で回転をなかったことにしている
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

public enum Side {
	up,
	down,
	left,
	right,
	forward,
	back
}
