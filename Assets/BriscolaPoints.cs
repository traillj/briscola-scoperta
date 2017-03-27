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
        // TO DO
        return 0;
    }
}
