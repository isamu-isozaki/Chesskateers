using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ButtonKiller : MonoBehaviour
{
    public List<GameObject> cards = new List<GameObject>();
    
    void Start()
    {
        foreach(GameObject Card in GameObject.FindGameObjectsWithTag("Card"))
        {
            cards.Add(Card);
        }
    }

    public void KillCard()
    {
        if (cards.Count > 0)
        {
            foreach(GameObject Card in cards)
            {
                Card.SetActive(false);
            }
        }
    }
}
