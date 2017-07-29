// Briscola Scoperta
// Author: traillj

using UnityEngine;

// Factory for Card objects.
public class CardFactory
{
    // Adds card scripts to the input card game objects.
    // Uses Points to determine the points of the cards during initialisation.
    public static void AddCardScripts(GameObject[] cards, Points pointsRef)
    {
        for (int i = 0; i < cards.Length; i++)
        {
            AddCardScript(cards[i], pointsRef);
        }
    }

    // Adds a card script to the input card game object.
    // Uses Points to determine the points of the card during initialisation.
    public static Card AddCardScript(GameObject card, Points pointsRef)
    {
        string cardName = card.name;
        int cardPoints = pointsRef.ToPoints(cardName);

        Card cardScript = card.AddComponent<Card>();
        cardScript.InitInfo(cardPoints, cardName[0], cardName[1]);
        return cardScript;
    }
}
