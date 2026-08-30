using UnityEngine;

public class Food : MonoBehaviour
{
    [SerializeField] BoxCollider2D gridArea;
    [SerializeField] SnakeController snake;

    void Start()
    {
        RandomizePosition();
    }

    void RandomizePosition()
    {
        Bounds bounds = gridArea.bounds;
        Vector3 newPos;
        int attempts = 0;

        do
        { 
            float x = Mathf.Round(Random.Range(bounds.min.x, bounds.max.x));
            float y = Mathf.Round(Random.Range(bounds.min.y, bounds.max.y));
            newPos = new Vector3(x,y,0f);
            attempts++;
        }

        while (isOccupiedBySnake(newPos) && attempts <100);

        transform.position = newPos;
    }

    bool isOccupiedBySnake(Vector3 targetPosition)
    {
        if(snake == null) return false;

        foreach(Transform segment in snake.Segments)
        {
            if(segment != null && segment.position == targetPosition)
            {
                return true;
            }
        }
        return false;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            RandomizePosition();
        }
    }
}
