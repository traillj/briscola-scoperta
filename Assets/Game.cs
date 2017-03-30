// Briscola Scoperta
// Author: traillj

using UnityEngine;

public class Game : MonoBehaviour
{
    // For positioning hands
    public Vector3[] playerCardPositions;
    public Vector3[] compCardPositions;

    private Hand playerHand;
    private Hand compHand;

    private Deck deck;
    private Points pointsRef = new BriscolaPoints();

    void Start()
    {
        deck = new Deck();
        playerHand = new Hand(deck, playerCardPositions, pointsRef);
        compHand = new Hand(deck, compCardPositions, pointsRef);

        playerHand.EnableTouch();
    }

    void Update()
    {
        playerHand.UpdateTouch();

        GameObject topCard = deck.PeekTopCard();
        CardFactory.AddCardScript(topCard, pointsRef);
    }
}
