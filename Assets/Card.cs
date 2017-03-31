// Briscola Scoperta
// Author: traillj

using UnityEngine;

public class Card : MonoBehaviour
{
    private int points;
    private char symbol;
    private char suit;

    // The position to place the card chosen in the trick
    private const float yDistFromCentre = -0.7f;

    private Vector3 playPos;
    private float speed = 7.0f;

    public void InitInfo(int points, char symbol, char suit)
    {
        this.points = points;
        this.symbol = symbol;
        this.suit = suit;
    }

    void Start()
    {
        playPos = transform.position;
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position,
            playPos, Time.deltaTime * speed);
    }

    void OnMouseUp()
    {
        playPos = new Vector3(transform.position.x, yDistFromCentre);
        // Test
        Debug.Log(symbol.ToString() + suit + " P:" + points);
    }

    // For computer's cards
    public void Move(bool downwards)
    {
        if (downwards)
        {
            playPos = new Vector3(transform.position.x, -yDistFromCentre);
        }
        else
        {
            playPos = new Vector3(transform.position.x, yDistFromCentre);
        }
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
}
