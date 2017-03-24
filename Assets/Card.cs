// Briscola Scoperta
// Author: traillj

using UnityEngine;

public class Card : MonoBehaviour
{
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
    }

    public void DisableTouch()
    {
        Destroy(this.GetComponent<BoxCollider2D>());
    }
}
