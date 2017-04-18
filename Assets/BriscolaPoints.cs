// Briscola Scoperta
// Author: traillj

using System;


// Assumes card1 was played first.
public class BriscolaPoints : Points
{
    public int ToPoints(string card)
    {
        char symbol = card[0];
        int points = 0;
        if (symbol == 'A')
        {
            points = 11;
        }
        else if (symbol == '3')
        {
            points = 10;
        }
        else if (symbol == 'K')
        {
            points = 4;
        }
        else if (symbol == 'Q')
        {
            points = 3;
        }
        else if (symbol == 'J')
        {
            points = 2;
        }

        return points;
    }

    public int PointsWon(string card1, string card2, char trumpSuit)
    {
        char suit1 = card1[1];
        char suit2 = card2[1];
        int points1 = ToPoints(card1);
        int points2 = ToPoints(card2);
        int totalPoints = points1 + points2;

        // For the card 2 to win, either both cards have the
        // same suit and card 2 has a higher rank, or
        // card 2 is the trump suit and card 1 is not.
        if (suit1 == suit2 && points1 < points2)
        {
            totalPoints *= -1;
        }
        else if (suit2 == trumpSuit && suit1 != trumpSuit)
        {
            totalPoints *= -1;
        }

        return totalPoints;
    }

    public string GetWinningCard(string card1, string card2, char trumpSuit)
    {
        string card = GetWinningSuit(card1, card2, trumpSuit);
        if (!String.Equals(card, ""))
        {
            return card;
        }

        string winningCard = card2;
        int pointsWon = PointsWon(card1, card2, trumpSuit);
        if (pointsWon > 0)
        {
            winningCard = card1;
        }
        else if (pointsWon == 0)
        {
            // Only number cards do not have point values.
            // For zero point cards, higher numbers have higher ranks.
            int rank1 = int.Parse(card1[0].ToString());
            int rank2 = int.Parse(card2[0].ToString());
            if (rank1 > rank2)
            {
                winningCard = card1;
            }
        }

        return winningCard;
    }

    // Returns the card of the winning suit.
    // If the suits are the same, "" is returned.
    private string GetWinningSuit(string card1, string card2, char trumpSuit)
    {
        string winningCard = card1;
        char suit1 = card1[1];
        char suit2 = card2[1];

        if (suit1 == suit2)
        {
            winningCard = "";
        }
        else if (suit2 == trumpSuit)
        {
            winningCard = card2;
        }

        return winningCard;
    }

}
