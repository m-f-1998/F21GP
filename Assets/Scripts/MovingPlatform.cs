using UnityEngine;

public class MovingPlatform : MonoBehaviour {
    
    public float speed;
    public float distance;
    private Vector3 startPosition;
    public enum Direction{Horizontal, Vertical, DiagonalLeft, DiagonalRight};
    public Direction direction;

    
    void Start () {
        startPosition = transform.position;
    }

    void Update () {
         switch(direction){
            case Direction.Horizontal:
                if ((speed < 0 && transform.position.x < startPosition.x) || (speed > 0 && transform.position.x > startPosition.x + distance)) speed *= -1;
                transform.position = new Vector2(transform.position.x + speed * Time.deltaTime, transform.position.y);
                break;
            case Direction.Vertical:
                if ((speed < 0 && transform.position.y < startPosition.y) || (speed > 0 && transform.position.y > startPosition.y + distance)) speed *= -1;
                transform.position = new Vector2(transform.position.x, transform.position.y + speed * Time.deltaTime);
                break;
            case Direction.DiagonalLeft:
                if ((speed < 0 && transform.position.x > startPosition.x) || (speed > 0 && transform.position.x < startPosition.x + distance)
                    && (speed < 0 && transform.position.y < startPosition.y) || (speed > 0 && transform.position.y > startPosition.y + distance)
                ) speed *= -1;
                transform.position = new Vector2(transform.position.x - speed * Time.deltaTime, transform.position.y + speed * Time.deltaTime);
                break;
            case Direction.DiagonalRight:    
                if ((speed < 0 && transform.position.x < startPosition.x) || (speed > 0 && transform.position.x > startPosition.x + distance)
                    && (speed < 0 && transform.position.y < startPosition.y) || (speed > 0 && transform.position.y > startPosition.y + distance)
                ) speed *= -1;
                transform.position = new Vector2(transform.position.x + speed * Time.deltaTime, transform.position.y + speed * Time.deltaTime);
                break;
        }
    }
}
