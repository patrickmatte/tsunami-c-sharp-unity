using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideComponent : MonoBehaviour {

	public int index;
	private GameObject _plane;
	public Action<int> deselectCallback;
	public Action<int> selectCallback;
	protected Vector3 gridPosition;
	protected bool isSelected;

	// Use this for initialization
	void Start () {
	}

	public void init() {
		_plane = this.transform.Find ("Plane").gameObject;
		SlidePlaneComponent _planeComponent = _plane.GetComponent<SlidePlaneComponent> ();
		_planeComponent.mouseDownCallBack = planeClickHandler;
	}

	protected void planeClickHandler() {
		if (isSelected) {
			deselectCallback (index);
		} else {
			selectCallback (index);
		}
	}

	public void setTexture(Texture2D value) {
		Renderer theRenderer = _plane.GetComponent<Renderer> ();
		theRenderer.material.mainTexture = value;
		//Color color = renderer.material.color;
		//color.a = 0.1f;
		//renderer.material.color = color;
	}

	public void setPosition(Vector3 value) {
		gameObject.transform.localPosition = value;
	}

	// Update is called once per frame
	void Update () {
		
	}

	public Tween show() {
		Vector3 localPosition = gameObject.transform.localPosition;
		localPosition.z = 0f;

		float startTime = UnityEngine.Random.value * 0.5f;
		Tween tweenZ = new Tween (startTime, 1f, new List<TweenProp> () {
			new TweenProp<Vector3> ( gameObject.transform, "localPosition", gameObject.transform.localPosition, localPosition, Ease.back.Out, Vector3.LerpUnclamped)
		});

		return tweenZ;
	}

	public Tween hide() {
		Vector3 localPosition = gameObject.transform.localPosition;
		localPosition.z = 20f;

		float startTime = UnityEngine.Random.value * 0.5f;
		Tween tweenZ = new Tween (startTime, 1f, new List<TweenProp>() {
			new TweenProp<Vector3> (gameObject.transform, "localPosition", gameObject.transform.localPosition, localPosition, Ease.back.In, Vector3.LerpUnclamped)
		});

		return tweenZ;
	}

	public Promise zoomIn() {
		isSelected = true;

		gridPosition = gameObject.transform.localPosition;

		Vector3 localPosition = gameObject.transform.localPosition;
		localPosition.z = -1f;

		Vector3 localScale = gameObject.transform.localScale;
		localScale.x = 2f;
		localScale.y = 2f;

		Tween tween = new Tween (0f, 1f, new List<TweenProp>() {
			new TweenProp<Vector3>(gameObject.transform, "localPosition", gameObject.transform.localPosition, localPosition, Ease.back.Out, Vector3.LerpUnclamped),
			new TweenProp<Vector3>(gameObject.transform, "localScale", gameObject.transform.localScale, localScale, Ease.back.Out, Vector3.LerpUnclamped)
		});

		tween.Start();

		return Promise.Resolve();
	}

	public Promise zoomOut() {
		isSelected = false;

		Vector3 localScale = gameObject.transform.localScale;
		localScale.x = 1f;
		localScale.y = 1f;

		Tween tween = new Tween (0f, 1f, new List<TweenProp> () {
			new TweenProp<Vector3>(gameObject.transform, "localPosition", gameObject.transform.localPosition, gridPosition, Ease.back.Out, Vector3.LerpUnclamped),
			new TweenProp<Vector3>(gameObject.transform, "localScale", gameObject.transform.localScale, localScale, Ease.back.Out, Vector3.LerpUnclamped),
		});

		tween.Start();

		return Promise.Resolve();
	}

}
