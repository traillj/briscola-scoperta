// Briscola Scoperta
// Author: traillj

public interface Strategy
{
    // Returns a card from compCards
    Card ChooseCard(Card[] compCards, Card[] opponentCards, Card topCard,
        char trumpSuit);
}
