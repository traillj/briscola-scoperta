// Briscola Scoperta
// Author: traillj

public interface Strategy
{
    // Returns a card from cards
    Card ChooseCard(Card[] cards, Card[] opponentCards, Card topCard,
        char trumpSuit);

    // Returns a card from compCards, computer is playing second
    Card ChooseCard(Card[] cards, Card[] opponentCards, Card topCard,
        char trumpSuit, Card playedCard);
}
