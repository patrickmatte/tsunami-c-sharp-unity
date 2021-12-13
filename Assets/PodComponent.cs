using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PodComponent : MonoBehaviour {

	protected GameObject _startButton;
	protected StartButtonComponent _startButtonComponent;
	protected GalleryComponent _galleryComponent;

	void Start () {;
		GameObject startButton = this.transform.Find ("StartButton").gameObject;
		_startButtonComponent = startButton.GetComponent<StartButtonComponent>();

		GameObject gallery = this.transform.Find ("Gallery").gameObject;
		_galleryComponent = gallery.GetComponent<GalleryComponent>();
	}

	public StartButtonComponent startButtonComponent {
		get {
			return _startButtonComponent;
		}
	}

	public GalleryComponent galleryComponent {
		get {
			return _galleryComponent;
		}
	}

	// Update is called once per frame
	void Update () {

	}

}
