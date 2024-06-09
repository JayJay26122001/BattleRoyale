using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Race : MonoBehaviour
{
    [SerializeField] Racer[] contestants;
    public List<Racer> Colocations = new List<Racer>();
    public bool RaceActive = false;
    [SerializeField] UnityEvent raceStart = new UnityEvent(), raceEnd = new UnityEvent();
    void Start()
    {
        StartRace();
    }
    void StartRace()
    {
        RaceActive = true;
        foreach(Racer contestant in contestants)
        {
            contestant.InicialPosition();
            contestant.race = this;
        }
        raceStart.Invoke();
    }
    void Update()
    {
        if (RaceActive)
        {
            UpdateColocations();
        }
    }

    void UpdateColocations()
    {
        Colocations = new List<Racer>();

        for (int i = 0; i < contestants.Length; i++)
        {
            float maior = contestants[0].transform.position.z;
            int indice = 0;
            for (int j = 0; j < contestants.Length; j++)
            {
                if ((contestants[j].transform.position.z > maior || Colocations.Contains(contestants[indice])) && !Colocations.Contains(contestants[j]))
                {
                    maior = contestants[j].transform.position.z;
                    indice = j;
                }
            }
            Colocations.Add(contestants[indice]);
        }
        GameManager.manager.uiController.ColocationsHud(Colocations);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<Racer>() != null)
        {
            RaceActive = false;
            raceEnd.Invoke();
        }
    }
    
    public int GetPlayerPos(Racer player)
    {
        int i;
        for(i = 0; i < Colocations.Count; i++)
        {
            if(player.name == Colocations[i].name)
            {
                break;
            }
        }
        return i;
    }
}
