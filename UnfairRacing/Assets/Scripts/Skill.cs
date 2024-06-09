using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class Skill : MonoBehaviour
{
    [SerializeField] UnityEvent action;
    [SerializeField] string skillName;
    public List<Skill> children = new List<Skill>();
    bool active = false, unlocked = false;
    public int price;
    public UnityEvent Event
    {
        get { return action; }
    }
    public string Name
    {
        get { return skillName; }
    }
    public bool Active
    {
        get { return active; }
    }
    public bool Unlocked
    {
        get { return unlocked; }
        set { unlocked = value; }
    }

    public void Activate()
    {
        active = true;
        action.Invoke();
        foreach(Skill child in children)
        {
            child.unlocked = true;
        }
    }

}
