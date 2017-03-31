// Briscola Scoperta
// Author: traillj

using System;

public class RandomStrategy : Strategy
{
    public Card ChooseCard(Card[] compCards, Card[] opponentCards,
        Card topCard, char trumpSuit)
    {
        Random rand = new Random();
        int randNum = rand.Next(0, compCards.Length);
        return compCards[randNum];
    }
}
