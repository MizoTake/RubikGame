using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

public class EffectPlayer : MonoBehaviour {

	private int moveVec;
	private float limitTime = 60;
	private ReactiveProperty<bool> check = new ReactiveProperty<bool>(true);

	// Use this for initialization
	void Start () {

		this.UpdateAsObservable()
			.Where(_ => Time.frameCount % limitTime == 0)
			.Subscribe(_ => moveVec = Random.Range(0, 3))
			.AddTo(this);

		this.UpdateAsObservable()
			.Subscribe(_ => {
				switch(moveVec) {
					case 0:
						transform.Rotate((Vector3.up + Vector3.right) * Random.Range(0.1f, 1f));
						break;
					case 1:
						transform.Rotate((Vector3.right + Vector3.forward) * Random.Range(0.1f, 1f));
						break;
					case 2:
						transform.Rotate((Vector3.forward + Vector3.up) * Random.Range(0.1f, 1f));
						break;
				}
			})
			.AddTo(this);

		check.AsObservable()
			.Where(_ => _)
			.Subscribe(_ => {
				int scaleVec = Random.Range(0, 3);
				Vector3 maxScale = Vector3.zero;
				float range = Random.Range(0, 0.3f);
				switch(scaleVec) {
					case 0:
						maxScale = transform.localScale + Vector3.up * range;
						break;
					case 1:
						maxScale = transform.localScale + Vector3.right * range;
						break;
					case 2:
						maxScale = transform.localScale + Vector3.forward * range;
						break;
				}
				//StartCoroutine(MoveTarget(maxScale));
			})
			.AddTo(this);

	}
	
	private IEnumerator MoveTarget(Vector3 target) {
		float time = 0;
		bool half = false;
		check.Value = false;
		Vector3 keepScale = transform.localScale;
		while(true) {
			transform.localScale = Vector3.Lerp(keepScale, target, time/(limitTime/2f));
			if(half) {
				time -= 1;
				if(limitTime <= time/limitTime){
					check.Value = true;
					break;
				}
			} else if(limitTime/2f <= time/(limitTime/2f)) {
				half = true;
			} else if(limitTime/2f >= time/(limitTime/2f) && !half) {
				time += 1f;
				Debug.Log("aaa");
			}
			yield return new WaitForSeconds(0.01f);
		}
		
	}
}
