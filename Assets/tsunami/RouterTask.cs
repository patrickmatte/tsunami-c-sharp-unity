using System;
using System.Collections.Generic;
using System.Reflection;

public class RouterTask {

	public List<IBranch> branches = new List<IBranch>();
	public Router router = null;
	public string name;
	public bool preload;
	public Action<object> onComplete;

	private IBranch branch;

	public RouterTask(string name, bool preload) {
		this.name = name;
		this.preload = preload;
	}

	public void start(object val = null) {
		//this.preloader = null;
		//this.assets = [];
		if (branches.Count > 0) {
			/*
			if (this.preload) {
				for (let i = 0; i < this.branches.length; i++) {
					this.assets.push(new AssetList());
				}
				this.assetList = new AssetList(this.assets.slice());
				this.preloader = this.router.preloader;
				if (this.preloader) {
					this.isPreloading = true;
					this.checkProgress();
					let promise = this.preloader.show();
					if (promise) {
						promise.then((obj) => {
							console.log("Preloader show promise done");
							this.startNextBranch();
						});
					} else {
						this.startNextBranch();
					}
				} else {
					this.startNextBranch();
				}
			} else {
				this.startNextBranch();
			}
			*/
			startNextBranch();
		} else {
			allComplete();
		}
	}

	/*
	void checkProgress() {
		if (this.assetList) {
			this.preloader.progress = this.assetList.progress;
		}
		if (this.isPreloading) {
			this.animationFrame = requestAnimationFrame(this.checkProgressBind);
		}
	}
	*/

	public void startNextBranch() {
		branch = branches[0];
		branches.RemoveAt(0);

		Type type = typeof(Branch);

		MethodInfo method = type.GetMethod(name);
		if (method != null) {
			//let assetList = this.assets.shift();
			object[] _stringMethodParams = new object[] { };
			Promise promise = (Promise)method.Invoke(branch, _stringMethodParams);
			if (promise != null) {
				promise.Then(branchComplete);
			} else {
				branchComplete();
			}
		} else {
			branchComplete();
		}
	}

	protected void branchComplete(object value = null) {
		if (branches.Count > 0) {
			startNextBranch();
		} else {
			/*
			if (this.preloader) {
				this.isPreloading = false;
				let promise = this.preloader.hide();
				if (promise) {
					promise.then(this.allComplete.bind(this));
				} else {
					this.allComplete();
				}
			} else {
				this.allComplete();
			}
			*/
			this.allComplete ();
		}
	}

	public void allComplete() {
		//this.assets = null;
		//this.assetList = null;
		branches = null;
		onComplete(null);
	}

}
