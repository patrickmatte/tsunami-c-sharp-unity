using System.Collections.Generic;

public class Branch:EventDispatcher, IBranch {

	protected Router _router = null;
	protected IBranch _root = null;
	protected IBranch _parent = null;
	protected string _slug = null;
	protected string _path = null;
	protected string _defaultChild = null;
	public Dictionary<string, IBranch> Branches;

	public Branch(Dictionary<string, IBranch> branches = null) : base()
	{
		Branches = branches ?? new Dictionary<string, IBranch>();
	}

	public virtual IBranch root {
		get {
			return _root;
		}

		set {
			_root = value as Branch;
		}
	}

	public virtual IBranch parent {
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

	public virtual string defaultChild
	{
		get
		{
			return _defaultChild;
		}

		set
		{
			_defaultChild = value;
		}
	}

	public virtual IBranch getBranch(string slug) {
		IBranch branch = Branches[slug];
		if (branch == null)
		{
			branch = Branches["*"];
		}
		if (branch == null)
		{
			branch = new Branch() as IBranch;
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
