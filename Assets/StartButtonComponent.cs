using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartButtonComponent: MonoBehaviour {

	protected Promise promise;
	protected Button _buttonComponent;
	protected Text _textComponent;
	protected object positionRef;
	protected object canvasGroupRef;

	// Use this for initialization
	void Start () {
		gameObject.SetActive(false);

		GameObject button = transform.Find ("Button").gameObject;
		_buttonComponent = button.GetComponent<Button>();

		GameObject text = button.transform.Find ("Text").gameObject;

		_textComponent = text.GetComponent<Text>();
	}

	public Button buttonComponent {
		get {
			return _buttonComponent;
		}
	}

	public Text textComponent {
		get {
			return _textComponent;
		}
	}

	public Promise show ()
	{
		gameObject.SetActive (true);

		CanvasGroup canvasGroup = gameObject.GetComponent<CanvasGroup> ();

		Vector3 localPositionStart = gameObject.transform.localPosition;
		localPositionStart.z = 20f;

		Vector3 localPositionEnd = gameObject.transform.localPosition;
		localPositionEnd.z = 0f;

		Tween tween = new Tween (0f, 1f, new List<TweenProp> () {
			new TweenProp<Vector3>(gameObject.transform, "localPosition", localPositionStart, localPositionEnd, Ease.back.Out, Vector3.LerpUnclamped),
			new TweenProp<float>(canvasGroup, "alpha", 0f, 1f, Ease.cubic.Out, Mathf.LerpUnclamped)
		});

		return tween.Start();
	}

	public Promise hide ()
	{
		CanvasGroup canvasGroup = gameObject.GetComponent<CanvasGroup> ();

		Vector3 localPositionStart = gameObject.transform.localPosition;
		localPositionStart.z = 0f;

		Vector3 localPositionEnd = gameObject.transform.localPosition;
		localPositionEnd.z = 20f;

		Tween tween = new Tween (0f, 1f, new List<TweenProp> () {
			new TweenProp<Vector3>(gameObject.transform, "localPosition", localPositionStart, localPositionEnd, Ease.back.In, Vector3.LerpUnclamped),
			new TweenProp<float>(canvasGroup, "alpha", 1f, 0f, Ease.cubic.In, Mathf.LerpUnclamped)
		});

		return tween.Start().Then(hideCompleteHandler);
	}

	protected void hideCompleteHandler(object value) {
		gameObject.SetActive (false);
	}

	void Update () {
		
	}

}
