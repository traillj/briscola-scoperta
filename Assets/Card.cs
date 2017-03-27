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

    public void DisableTouch()
    {
        Destroy(this.GetComponent<BoxCollider2D>());
    }

    public int GetPoints()
    {
        return points;
    }

    public void SetPoints(int points)
    {
        this.points = points;
    }

    public char GetSymbol()
    {
        return symbol;
    }

    public void SetSymbol(char symbol)
    {
        this.symbol = symbol;
    }

    public char GetSuit()
    {
        return suit;
    }

    public void SetSuit(char suit)
    {
        this.suit = suit;
    }
}
