// Briscola Scoperta
// Author: traillj

// Strategy for choosing a card in Briscola Scoperta.
public interface Strategy
{
    // Returns a card from cards, playing the first card of the trick.
    Card ChooseCard(Card[] cards, Card[] opponentCards, Card topCard,
        char trumpSuit);

    // Returns a card from cards, playing the second card of the trick.
    Card ChooseCard(Card[] cards, Card[] opponentCards, Card topCard,
        char trumpSuit, Card playedCard);
}
