// Briscola Scoperta
// Author: traillj

using UnityEngine;

public class CardFactory
{
    public static void AddCardScripts(GameObject[] cards, Points pointsRef)
    {
        for (int i = 0; i < cards.Length; i++)
        {
            AddCardScript(cards[i], pointsRef);
        }
    }

    public static Card AddCardScript(GameObject card, Points pointsRef)
    {
        string cardName = card.name;
        int cardPoints = pointsRef.ToPoints(cardName);

        Card cardScript = card.AddComponent<Card>();
        cardScript.InitInfo(cardPoints, cardName[0], cardName[1]);
        return cardScript;
    }
}
