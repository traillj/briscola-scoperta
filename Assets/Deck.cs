// Briscola Scoperta
// Author: traillj

using UnityEngine;

public class Deck : MonoBehaviour
{
    
    private GameObject[] cards;
    
    // Shuffles the cards by sorting randomly and
    // reassigning the layer order of each card.
	void Start()
    {
        cards = GameObject.FindGameObjectsWithTag("Card");
        System.Array.Sort(cards, RandomSort);
        for (int i = 0; i < cards.Length; i++)
        {
            cards[0].GetComponent<Renderer>().sortingOrder = i;
        }
    }
    
    private int RandomSort(GameObject a, GameObject b)
    {
        return Random.Range(-1, 2);
    }
}
