// Briscola Scoperta
// Author: traillj

using UnityEngine;

public class Card : MonoBehaviour
{
    private int points;
    private char symbol;
    private char suit;

    // The position to place the card chosen in the trick
    private const float yPlayPos = -0.7f;

    private bool moved = false;

    public void InitInfo(int points, char symbol, char suit)
    {
        this.points = points;
        this.symbol = symbol;
        this.suit = suit;
    }

    // Moves the card to the play position instantly.
    void OnMouseUp()
    {
        moved = true;
        transform.position = new Vector3(transform.position.x, yPlayPos);
    }

    // Moves the card to the play position instantly.
    public void Move(bool fromTop)
    {
        moved = true;
        float newY = yPlayPos;
        if (fromTop)
        {
            newY *= -1;
        }
        transform.position = new Vector3(transform.position.x, newY);
    }

    public void DisableTouch()
    {
        Destroy(this.GetComponent<BoxCollider2D>());
    }

    public int GetPoints()
    {
        return points;
    }

    public char GetSymbol()
    {
        return symbol;
    }

    public char GetSuit()
    {
        return suit;
    }

    public bool HasMoved()
    {
        return moved;
    }
}
