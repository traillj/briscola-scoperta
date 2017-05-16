// Briscola Scoperta
// Author: traillj

using System;

public class RandomStrategy : Strategy
{
    public Card ChooseCard(Card[] cards, Card[] opponentCards,
        Card topCard, char trumpSuit)
    {
        Random rand = new Random();
        int randNum = rand.Next(0, cards.Length);
        return cards[randNum];
    }

    public Card ChooseCard(Card[] cards, Card[] opponentCards,
        Card topCard, char trumpSuit, Card playedCard)
    {
        return ChooseCard(cards, opponentCards, topCard, trumpSuit);
    }
}
