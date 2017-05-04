// Briscola Scoperta
// Author: traillj

public interface Strategy
{
    // Returns a card from compCards
    Card ChooseCard(Card[] compCards, Card[] opponentCards, Card topCard,
        char trumpSuit);

    // Returns a card from compCards, computer is playing second
    Card ChooseCard(Card[] compCards, Card[] opponentCards, Card topCard,
        char trumpSuit, Card playedCard);
}
