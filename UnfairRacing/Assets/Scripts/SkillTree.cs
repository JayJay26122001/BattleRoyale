using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillTree : MonoBehaviour
{
    public Dictionary<int, Skill> tree = new Dictionary<int, Skill>();
    public List<Skill> skills = new List<Skill>();
    public int points;
    void Awake()
    {
        for(int i = 0; i < skills.Count; i++)
        {
            tree.Add(i, skills[i]);
        }
        tree[0].Unlocked = true;
        tree[0].Activate();
    }

    public void ActivateSkill(int index)
    {
        if(points >= tree[index].price)
        {
            points -= tree[index].price;
            tree[index].Activate();
        }
    }
}
