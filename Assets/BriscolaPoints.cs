// Briscola Scoperta
// Author: traillj

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
}
