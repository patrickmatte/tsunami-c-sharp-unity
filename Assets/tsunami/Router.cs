using System;
using System.Collections.Generic;
using UnityEngine;

public class Router : EventDispatcher
{

    public static string CHANGE = "change";
    public static string COMPLETE = "complete";
    public static string INTERRUPT = "interrupt";

    public List<IBranch> branches;
    public IBranch root;
    public Dictionary<string, RouterTransition> Transitions = new Dictionary<string, RouterTransition>();

    protected string _location = null;
    protected bool _inTransition;
    protected string _interruptLocation = null;
    protected string _nextLocation;

    public Router(IBranch root) : base()
    {
        this.root = root;
        branches = new List<IBranch>();

        Transitions.Add("show", new RouterTransition(this, "show"));
        Transitions["show"].tasks.Add(new RouterTask("load", true));
        Transitions["show"].tasks.Add(new RouterTask("show", false));

        Transitions.Add("hide", new RouterTransition(this, "hide"));
        Transitions["hide"].tasks.Add(new RouterTask("hide", false));
    }

    public string location
    {
        get
        {
            return _location;
        }

        set
        {
            if (_inTransition)
            {
                _interruptLocation = value;
            }
            else
            {
                _changeTheLocation(value);
            }
        }
    }

    protected void _changeTheLocation(string value)
    {
        _interruptLocation = null;
        string path = value;
        if (path != location)
        {
            _inTransition = true;
            _location = path;
            DispatchEvent(new Event(Router.CHANGE));
            _nextLocation = "root";
            if (path != "")
            {
                _nextLocation += "/" + path;
            }
            _startTransitions();
        }
        else
        {
            _showComplete();
        }
    }

    private List<string> sliceString(List<string> list, int start, int end)
    {
        end = Math.Min(list.Count, end);
        List<string> copy = new List<string>();
        for (int i = start; i < end; i++)
        {
            string str = list[i];
            copy.Add(str);
        }
        return copy;
    }

    public void _startTransitions()
    {
        string[] nextLocationStringArray = _nextLocation.Split('/');
        List<string> nextLocationArray = new List<string>();
        foreach (string slug in nextLocationStringArray)
        {
            nextLocationArray.Add(slug);
        }

        List<string> currentLocationArray = new List<string>();
        foreach (IBranch branch in branches)
        {
            currentLocationArray.Add(branch.slug);
        }

        int breakIndex = -1;
        for (int i = 0; i < currentLocationArray.Count; i++)
        {
            List<string> currentLocationIds = sliceString(currentLocationArray, 0, i + 1);
            string branchId = string.Join("_", currentLocationIds.ToArray());

            List<string> nextLocationIds = sliceString(nextLocationArray, 0, i + 1);
            string nextBranchId = string.Join("_", nextLocationIds.ToArray());

            if (branchId == nextBranchId)
            {
                breakIndex = i;
            }
        }

        int startIndex = breakIndex + 1;
        int length = branches.Count - startIndex;
        Transitions["hide"].branches = branches.Splice(startIndex, length);
        Transitions["hide"].branches.Reverse();

        Transitions["hide"].start().Then((object val) =>
        {
            _hideComplete();
        });
    }

    protected void checkForDefaultBranches(IBranch parent, List<IBranch> branches)
    {
        if (parent != null)
        {
            if (parent.defaultChild != null)
            {
                string slug = parent.defaultChild;
                IBranch branch = getBranchFromSlug(parent, slug);
                if (branch != null)
                {
                    branches.Add(branch);
                    checkForDefaultBranches(branch, branches);
                }
            }
        }
    }

    protected IBranch getBranchFromSlug(IBranch parent, string slug = null)
    {
        IBranch branch = null;
        if (slug != null)
        {
            if (parent != null)
            {
                branch = parent.getBranch(slug);
                branch.parent = parent;
                if (parent == root)
                {
                    branch.path = slug;
                }
                else
                {
                    branch.path = parent.path + "/" + slug;
                }
            }
            else
            {
                branch = root;
                branch.path = "";
            }
            branch.router = this;
            branch.root = root;
            branch.slug = slug;
        }
        return branch;
    }


    public void _hideComplete()
    {

        if (_interruptLocation != null)
        {
            _inTransition = false;
            DispatchEvent(new Event(INTERRUPT));
            _changeTheLocation(_interruptLocation);
        }
        else
        {
            string[] nextLocationStringArray = _nextLocation.Split('/');
            List<string> nextLocationArray = new List<string>();
            foreach (string slug in nextLocationStringArray)
            {
                nextLocationArray.Add(slug);
            }
            IBranch parent = null;
            if (branches.Count > 0)
            {
                parent = branches[branches.Count - 1];
            }
            List<IBranch> newBranches = new List<IBranch>();
            for (int i = branches.Count; i < nextLocationArray.Count; i++)
            {
                string slug = nextLocationArray[i];
                IBranch branch = getBranchFromSlug(parent, slug);
                newBranches.Add(branch);
                parent = branch;
            }
            checkForDefaultBranches(parent, newBranches);
            Transitions["show"].branches = newBranches;
            foreach (IBranch branch in Transitions["show"].branches)
            {
                this.branches.Add(branch);
            }
            Transitions["show"].start().Then((object val) =>
            {
                this._showComplete();
            });
        }
    }

    public void _showComplete()
    {
        _inTransition = false;
        DispatchEvent(new Event(Router.COMPLETE));
        if (_interruptLocation != null)
        {
            _changeTheLocation(_interruptLocation);
        }
    }

    public string toString()
    {
        return "[Router" + " location=" + location + "]";
    }

}
