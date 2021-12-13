using System.Collections.Generic;

public class Branch {

	protected Router _router;
	protected Branch _root;
	protected Branch _parent;
	protected string _slug;
	protected string _path;
	public Dictionary<string, Branch> Branches;

	public Branch(Dictionary<string, Branch> branches = null) {
		Branches = branches ?? new Dictionary<string, Branch>();
	}

	public virtual Branch root {
		get {
			return _root;
		}

		set {
			_root = value;
		}
	}

	public virtual Branch parent {
		get {
			return _parent;
		}

		set {
			_parent = value;
		}
	}

	public virtual Router router {
		get {
			return _router;
		}

		set {
			_router = value;
		}
	}

	public virtual string path {
		get {
			return _path;
		}

		set {
			_path = value;
		}
	}

	public virtual string slug {
		get {
			return _slug;
		}

		set {
			_slug = value;
		}
	}

	public virtual Branch getBranch(string slug) {
		Branch branch = Branches[slug];
		if (branch == null)
		{
			branch = Branches["*"];
		}
		if (branch == null)
		{
			branch = new Branch();
		}
		return branch;
	}

	public virtual Promise load() {
        return Promise.Resolve ();
	}

    public virtual Promise show() {
        return Promise.Resolve ();
	}

    public virtual Promise hide() {
        return Promise.Resolve ();
	}

	public override string ToString() {
		return "[Branch" + " slug=" + this.slug + "]";
	}

}
