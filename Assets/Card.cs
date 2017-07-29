// Briscola Scoperta
// Author: traillj

using UnityEngine;

// Script for moving and storing information about a card.
public class Card : MonoBehaviour
{
    // Points the card is worth
    private int points;
    // The rank or face card of the card
    private char symbol;
    // Suit of the card
    private char suit;

    // The position to place the card chosen in the trick
    private const float yPlayPos = -0.7f;

    // True if the card has been moved
    private bool moved = false;
    // True if clicking the card will cause it to move
    private bool clickable = false;

    // Initialises information about the card.
    public void InitInfo(int points, char symbol, char suit)
    {
        this.points = points;
        this.symbol = symbol;
        this.suit = suit;
    }

    // Moves the card to the play position instantly.
    void OnMouseUp()
    {
        if (clickable)
        {
            moved = true;
            transform.position = new Vector3(transform.position.x, yPlayPos);
        }
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

    // Get the points of the card.
    public int GetPoints()
    {
        return points;
    }

    // Get the symbol of the card.
    public char GetSymbol()
    {
        return symbol;
    }

    // Get the suit of the card.
    public char GetSuit()
    {
        return suit;
    }

    // Test whether the card has moved.
    public bool HasMoved()
    {
        return moved;
    }

    // Set whether the card has moved.
    public void SetMoved(bool moved)
    {
        this.moved = moved;
    }

    // Set whether the card has moved.
    // Setting this to true allows the card to be moved.
    public void SetClickable(bool clickable)
    {
        this.clickable = clickable;
    }
}
