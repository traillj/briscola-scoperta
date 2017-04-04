// Briscola Scoperta
// Author: traillj

public interface Points
{
    // Card format examples:
    // "5C", "AD", etc.
    int ToPoints(string card);

    // If card1 wins points, a positive number is returned.
    // If card2 wins points, a negative number is returned.
    // Assumes only one player can win points in the trick,
    // or only the difference is important.
    int PointsWon(string card1, string card2, char trumpSuit);

    // Returns the winning card.
    string GetWinningCard(string card1, string card2, char trumpSuit);
}
