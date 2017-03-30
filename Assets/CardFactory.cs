// Briscola Scoperta
// Author: traillj

using UnityEngine;

public class CardFactory
{
    public static Card AddCardScript(GameObject card, Points pointsRef)
    {
        string cardName = card.name;
        int cardPoints = pointsRef.ToPoints(cardName);

        Card cardScript = card.AddComponent<Card>();
        cardScript.InitInfo(cardPoints, cardName[0], cardName[1]);
        return cardScript;
    }
}
