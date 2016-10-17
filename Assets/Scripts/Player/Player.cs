using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UniRx.Triggers;

public class Player : MonoBehaviour {

	public GameObject cubes;
	public Field field;
	private TouchManager touch;
	private ReactiveProperty<int> nextFieldNum = new ReactiveProperty<int>();
	private ReactiveProperty<Cube> checkCube = new ReactiveProperty<Cube>();
	private int nowfieldNum = 4;
	private int[,] checkPoint = new int[,] {
		{0, -3},
		{1, -2},
		{2, -1},
		{2, 3},
		{5, 6},
		{8, 9},
		{8, 11},
		{7, 10},
		{6, 9},
		{6, 5},
		{3, 2},
		{0, -1}
	};

	private int[,] checkRange = new int[,] {
		{2, 3},
		{3, 2},
		{5, 6},
		{6, 5},
		{8, 9}
	};

	private int[,] answerPoint = new int[,] {
		{2, 2},
		{3, 2},
		{4, 2},
		{0, 0},
		{3, 0},
		{6, 0},
		{4, 3},
		{3, 3},
		{2, 3},
		{6, 1},
		{3, 1},
		{0, 1}
	};

	private ListType[] answerListType = new ListType[4] {
		ListType.X,
		ListType.X,
		ListType.Z,
		ListType.Z
	};

	private Vector3[] answerVector = new Vector3[4] {
		Vector3.right,
		Vector3.left,
		Vector3.forward,
		Vector3.back
	};

	// Use this for initialization
	void Start () {
		touch = GetComponent<TouchManager>();
		checkCube.Value = cubes.GetComponent<ParentCube>().centers[0].GetComponent<Cube>();
		var controll = false;
		nextFieldNum.Value = nowfieldNum;
		transform.position = field.objects[nowfieldNum].transform.position;
		
		nextFieldNum
			.Select(_ => _ >= 0 && _ <= 8)
			.Subscribe(result => {
				for(int i = 0; i<checkPoint.GetLength(0); i++) {
					if(nowfieldNum == checkPoint[i, 0] && nextFieldNum.Value == checkPoint[i, 1]) {
						checkCube.Value = cubes.GetComponent<ParentCube>().centers[answerPoint[i, 0]].gameObject.GetComponent<Cube>();
						checkCube.Value.Rot(answerListType[answerPoint[i, 1]], answerVector[answerPoint[i, 1]]);
					}
				}

				result = CheckRangePoint(nowfieldNum, nextFieldNum.Value, result);
				if(result) {
					nowfieldNum = nextFieldNum.Value;
					StartCoroutine(MoveTarget(field.objects[nextFieldNum.Value].transform.position));
				} else {
					nextFieldNum.Value = nowfieldNum;
				}
			})
			.AddTo(this);

		this.UpdateAsObservable()
			.Where(_ => (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow) || 
			(touch.FlickProcess() != FlickVector.NULL && touch.FlickProcess() == FlickVector.UP)) && controll)
			.Subscribe(_ => nextFieldNum.Value += 1)
			.AddTo(this);

		this.UpdateAsObservable()
			.Where(_ => (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow) || 
			(touch.FlickProcess() != FlickVector.NULL && touch.FlickProcess() == FlickVector.LEFT)) && controll)
			.Subscribe(_ => nextFieldNum.Value -= 3)
			.AddTo(this);

		this.UpdateAsObservable()
			.Where(_ => (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow) || 
			(touch.FlickProcess() != FlickVector.NULL && touch.FlickProcess() == FlickVector.DOWN)) && controll)
			.Subscribe(_ => nextFieldNum.Value -= 1)
			.AddTo(this);
			
		this.UpdateAsObservable()
			.Where(_ => (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow) || 
			(touch.FlickProcess() != FlickVector.NULL && touch.FlickProcess() == FlickVector.RIGHT)) && controll)
			.Subscribe(_ => nextFieldNum.Value += 3)
			.AddTo(this);

		checkCube.
			Subscribe(_ => _.nowRotation
							.Subscribe(result => controll = !result)
							.AddTo(this))
			.AddTo(this);
	}
	
	private bool CheckRangePoint(int nowPoint, int nextPoint, bool result) {
		if(!result) return false;
 		for(int i = 0; i<checkRange.GetLength(0); i++) {
			if(nowPoint == checkRange[i, 0] && nextPoint == checkRange[i, 1]) {
				return false;
			}
		}
		return true;
	}

	private System.Collections.IEnumerator MoveTarget(Vector3 target) {
		float time = 0;
		float moveFarame = 3;
		Vector3 keepPos = transform.position;
		while(true) {
			transform.position = Vector3.Lerp(keepPos, target, time/moveFarame);
			time += 1;
			yield return new WaitForSeconds(0.01f);
			if(time >= moveFarame) {
				transform.position = target;
				break;
			}
		}
	}
}
