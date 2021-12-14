using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseBranch : Branch {

	public Pod pod {
		get {
			return (Pod)_root;
		}
	}

}

public class Pod:BaseBranch {

	public App app;
	public PodComponent podComponent;

	public Pod(App app, PodComponent podComponent) {
		this.app = app;
		this.podComponent = podComponent;

		router = new Router (this);
        router.AddEventListener(Router.COMPLETE, routerComplete);
		//router.AddEventListener (Router.CHANGE, routerChange);

		Branches.Add("home", new HomeBranch());
		Branches.Add("gallery", new GalleryBranch());

		defaultChild = "home";
	}

	protected void routerChange(Event e) {
		Debug.Log ("router CHANGE " + router.location);
	}

	protected void routerComplete(Event e) {
		Debug.Log ("router COMPLETE " + router.location);
	}

}

public class HomeBranch:BaseBranch {

	public HomeBranch() {
	}
		
    public override Promise show() {
		setButtonText ();
		pod.podComponent.startButtonComponent.buttonComponent.onClick.AddListener (startClickHandler);
		return pod.podComponent.startButtonComponent.show ();
	}

    public override Promise hide() {
		pod.podComponent.startButtonComponent.buttonComponent.onClick.RemoveListener (startClickHandler);
		return pod.podComponent.startButtonComponent.hide ();
	}

	protected virtual void setButtonText() {
		pod.podComponent.startButtonComponent.gameObject.transform.localPosition = new Vector3 (0, 0, 0);
		pod.podComponent.startButtonComponent.textComponent.text = "OPEN";
	}

	protected virtual void startClickHandler() {
		router.location = "gallery";
	}

}

public class GalleryBranch:HomeBranch {

	public GalleryBranch()
    {
		//Branches.Add("*", new SlideBranch());
    }

	protected override void startClickHandler() {
		router.location = "";
	}

	protected override void setButtonText() {
		pod.podComponent.startButtonComponent.gameObject.transform.localPosition = new Vector3 (0, 8, 0);
		pod.podComponent.startButtonComponent.textComponent.text = "CLOSE";
	}

	public override Promise load() {
		pod.podComponent.galleryComponent.selectSlideCallback = this.slideSelectHandler;
		pod.podComponent.galleryComponent.deselectSlideCallback = this.slideDeselectHandler;

		List<Promise> list = new List<Promise> ();
		foreach(ImageData imageData in pod.app.gameData.images ) {
			string path = System.IO.Path.Combine(Application.streamingAssetsPath, imageData.url);
			//Debug.Log ("ImageData.url = " + path);
			list.Add(UnityUtils.LoadImageFromDisk (path));
		}

		return Promise.All(list).Then(imagesLoaded);
	}

	public void imagesLoaded(List<object> list) {
		List<Texture2D> textures = new List<Texture2D>();
		foreach(object tex in list) {
			textures.Add (tex as Texture2D);
		}
		pod.podComponent.galleryComponent.setTextures (textures);
	}

	protected void slideSelectHandler(int index) {
		router.location = path + "/slide-" + index;
	}

	protected void slideDeselectHandler(int index) {
		router.location = path;
	}

	public override Promise show() {
		base.show();
		return pod.podComponent.galleryComponent.show();
	}

	public override Promise hide() {
		base.hide();
		return pod.podComponent.galleryComponent.hide();
	}

    public override IBranch getBranch(string slug)
    {
        return new SlideBranch();
    }

}

public class SlideBranch:BaseBranch {

	public override Promise show() {
		string[] slugArray = slug.Split('-');
		int index = Int32.Parse (slugArray[1]);
		SlideComponent slide = pod.podComponent.galleryComponent.slides [index];
		return slide.zoomIn();
	}

	public override Promise hide() {
		string[] slugArray = slug.Split('-');
		int index = Int32.Parse (slugArray[1]);
		SlideComponent slide = pod.podComponent.galleryComponent.slides [index];
		return slide.zoomOut();
	}

}
