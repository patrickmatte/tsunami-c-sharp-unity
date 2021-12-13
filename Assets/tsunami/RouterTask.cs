using System;
using System.Collections.Generic;
using System.Reflection;

public class RouterTask {

	public List<Branch> branches = new List<Branch>();
	public Router router = null;
	public string name;
	public bool preload;
	public Action onComplete;

	private Branch branch;

	public RouterTask(string name, bool preload) {
		this.name = name;
		this.preload = preload;
		//this.checkProgressBind = this.checkProgress.bind(this);
	}

	public void start() {
		//this.preloader = null;
		//this.assets = [];
		if (this.branches.Count > 0) {
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
			this.startNextBranch();
		} else {
			this.allComplete();
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
		this.branch = this.branches[0];
		//this.branch.onComplete = branchComplete;
		this.branches.RemoveAt(0);

		Type type = typeof(Branch);

		//get the method from a given type definition
		MethodInfo method = type.GetMethod(this.name);

		//build the parameters array
		object[] _stringMethodParams = new object[] {};

		//invoke the method
		Promise promise = (Promise)method.Invoke(this.branch, _stringMethodParams);
		promise.Then (branchComplete);

		//Action method = this.branch[this.name];
		//method (branchComplete);

		/*
		if (method) {
			method = method.bind(this.branch);
			let assetList = this.assets.shift();
			let promise = method(assetList);
			if (promise) {
				promise.then(this.branchComplete.bind(this));
			} else {
				this.branchComplete();
			}
		} else {
			this.branchComplete();
		}
		*/
	}

	protected void branchComplete(object value) {
		if (this.branches.Count > 0) {
			this.startNextBranch();
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
		this.branches = null;
		this.onComplete();
	}

}
