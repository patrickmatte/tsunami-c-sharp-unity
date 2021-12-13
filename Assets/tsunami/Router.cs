﻿using System;
using System.Collections.Generic;

public class Router : EventDispatcher {

	public static string CHANGE = "change";
	public static string COMPLETE = "complete";

	public List<IBranch> branches;
	public IBranch root;
	public Dictionary<string, RouterTransition> Transitions = new Dictionary<string, RouterTransition>();

	protected string _location = null;
	protected bool _inTransition;
	protected string _interruptLocation = null;
	protected string _nextLocation;

	public Router(IBranch root):base() {
		this.root = root;
		branches = new List<IBranch>();

		RouterTransition show = new RouterTransition(this, "show", _showComplete);
		show.tasks.Add( new RouterTask("load", true) );
		show.tasks.Add( new RouterTask("show", false) );

		RouterTransition hide = new RouterTransition(this, "hide", _hideComplete);
		hide.tasks.Add( new RouterTask("hide", false) );

		Transitions.Add("show", show);
		Transitions.Add("hide", hide);
	}

	public string location {
		get {
			return _location;
		}

		set {
			if (_inTransition) {
				_interruptLocation = value;
			} else {
				_interruptLocation = null;
				string path = value;
				if (path != location) {
					_inTransition = true;
					_location = path;
					DispatchEvent(new Event (Router.CHANGE));
					_nextLocation = "root";
					if (path != "") {
						_nextLocation += "/" + path;
					}
					_startTransitions();
				}
			}
		}
	}

	private List<string> sliceString(List<string> list, int start, int end) {
		end = Math.Min (list.Count, end);
		List<string> copy = new List<string>();
		for(int i = start; i < end; i++) {
			string str = list[i];
			copy.Add(str);
		}
		return copy;
	}
		
	public void _startTransitions() {
		List<string> currentLocationArray = new List<string> ();
		foreach (IBranch branch in branches) {
			currentLocationArray.Add (branch.slug);
		}

		string[] nextLocationStringArray = _nextLocation.Split('/');

		List<string> nextLocationArray = new List<string> ();
		foreach(string slug in nextLocationStringArray) {
			nextLocationArray.Add (slug);
		}

		int breakIndex = -1;
		for (int i = 0; i < currentLocationArray.Count; i++) {
			List<string> currentLocationIds = sliceString(currentLocationArray, 0, i + 1);
			string branchId = string.Join("_", currentLocationIds.ToArray());

			List<string> nextLocationIds = sliceString(nextLocationArray, 0, i + 1);
			string nextBranchId = string.Join("_", nextLocationIds.ToArray());

			if (branchId == nextBranchId) {
				breakIndex = i;
			}
		}
			
		int startIndex = breakIndex + 1;
		int length = branches.Count - startIndex;
		Transitions["hide"].branches = branches.Splice (startIndex, length);
		Transitions["hide"].branches.Reverse ();

		IBranch parent = null;
		if (branches.Count > 0) {
			parent = branches[branches.Count - 1];
		}

		List<IBranch> newBranches = new List<IBranch>();
		for (int i = breakIndex + 1; i < nextLocationArray.Count; i++) {
			string slug = nextLocationArray[i];
			IBranch branch = root;
			if (parent != null) {
				branch = parent.getBranch (slug);
			}
			branch.slug = slug;
			branch.router = this;
			branch.root = root;
			string path = "";
			if (parent != null) {
				branch.parent = parent;
				if (parent.slug == "root") {
					path = slug;
				} else {
					path = parent.path + "/" + slug;
				}
			}
			branch.path = path;
			newBranches.Add(branch);
			parent = branch;
		}

		Transitions["show"].branches = newBranches;

		Transitions["hide"].start();
	}

	public void _hideComplete() {
		if (_interruptLocation != null) {
			_inTransition = false;
			location = _interruptLocation;
		} else {
			foreach (IBranch branch in Transitions["show"].branches) {
				branches.Add(branch);
			}
			Transitions["show"].start();
		}
	}

	public void _showComplete() {
		_inTransition = false;
		DispatchEvent(new Event(Router.COMPLETE));
		if (_interruptLocation != null) {
			location = _interruptLocation;
		}
	}

	public string toString() {
		return "[Router" + " location=" + location + "]";
	}

}
