using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class SnakeController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI scoreText;
    int score = 0;
    
    [SerializeField] float startMoveInterval = 0.3f;
    [SerializeField] Vector2 direction = Vector2.right;

    [SerializeField] Transform segmentPrefab;
    [SerializeField] int initialSize = 4;

    private List<Transform> segments = new List<Transform>();
    private Vector2 inputDirection;
    private float nextMoveTime;
    float moveInterval;

    public List<Transform> Segments => segments;
    
    void Start()
    {
        resetState();
        scoreText.text = score.ToString();
    }

    void Update()
    {
        if(direction.x != 0f)
        {
            if(Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                inputDirection = Vector2.up;
            }else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                inputDirection = Vector2.down;
            }
        }
        if(direction.y != 0f)
        {
            if(Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                inputDirection = Vector2.left;
            }else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                inputDirection = Vector2.right;
            }
        }

        if(Time.time >= nextMoveTime)
        {
            nextMoveTime = Time.time + moveInterval;
            Move();
        }
    }

    private void Move()
    {
        if(inputDirection != Vector2.zero)
        {
            direction = inputDirection;
        }

        for(int i = segments.Count - 1; i > 0; i--)
        {
            segments[i].position = segments[i-1].position;
        }

        transform.position = new Vector3(Mathf.Round(transform.position.x) + direction.x, Mathf.Round(transform.position.y) + direction.y, 0f);
    }

    void Grow()
    {
        Transform segment = Instantiate(segmentPrefab);

        segment.position = segments[segments.Count-1].position;
        segments.Add(segment);
    }

    void resetState()
    {
        for(int i = 1; i < segments.Count; i++)
        {
            if (segments[i] != null)
            {
                Destroy(segments[i].gameObject);
            }
        }

        segments.Clear();
        segments.Add(transform);

        transform.position = Vector3.zero;
        direction = Vector2.right;
        inputDirection = Vector2.right;
        moveInterval = startMoveInterval;
        score = 0;
        scoreText.text = score.ToString();

        for (int i = 1; i < initialSize; i++)
        {
            Transform segment = Instantiate(segmentPrefab);
            segment.position = new Vector3(-i, 0f, 0f);
            segments.Add(segment);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Food"))
        {
            Grow();
            if(moveInterval > 0.08f)
            {
                moveInterval -= 0.02f;
            }
            else
            {
                moveInterval = 0.08f;
            }
            score += 100;
            scoreText.text = score.ToString();
        } 
        else if (collision.CompareTag("Obstacle") || collision.CompareTag("Body"))
        {
            resetState();
        }
    }
}
