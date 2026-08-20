using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class SnakeController : MonoBehaviour
{
    [SerializeField] float moveInterval = 0.1f;
    Vector2 direction = Vector2.right;

    [SerializeField]Transform segmentPrefab;
    int initialSize = 4;

    private List<Transform> segments = new List<Transform>();
    private Vector2 inputDirection;
    private float nextMoveTime;

    void Start()
    {
        resetState();
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
    }

    void FixedUpdate()
    {
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
            Destroy(segments[i].gameObject);
        }

        segments.Clear();
        segments.Add(transform);

        transform.position = Vector3.zero;
        direction = Vector2.right;
        inputDirection = Vector2.right;

        for(int i = 0; i < initialSize; i++)
        {
            Grow();
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Food"))
        {
            Grow();
        }
        else if (collision.CompareTag("Obstacle"))
        {
            resetState();
        }
    }
}
