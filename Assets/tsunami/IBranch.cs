using System;

public interface IBranch {

	Router router {
		get;
		set;
	}

	IBranch parent {
		get;
		set;
	}

	IBranch root {
		get;
		set;
	}

	string slug {
		get;
		set;
	}

	string path
	{
		get;
		set;
	}

	string defaultChild
	{
		get;
		set;
	}

	Promise load();

	Promise show ();

	Promise hide ();

	IBranch getBranch (string slug);

}
