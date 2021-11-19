using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Obstacle : MonoBehaviour, IComparable<Obstacle>
{
    public SpriteRenderer spriteRenderer { get; set; }

    private Color defaultColor;

    private Color fadedColor;


    public int CompareTo(Obstacle other)
    {
        if (spriteRenderer.sortingOrder > other.spriteRenderer.sortingOrder)
        {
            return 1;
        }
        else if (spriteRenderer.sortingOrder < other.spriteRenderer.sortingOrder)
        {
            return -1;
        }

        return 0;
    }

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        defaultColor = spriteRenderer.color;
        fadedColor = defaultColor;
        fadedColor.a = 0.7f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FadeOut()
    {
        spriteRenderer.color = fadedColor;
    }

    public void FadeIn()
    {
        spriteRenderer.color = defaultColor;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.name == "ObstacleCollider")
        {
            FadeOut();
        }  
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.name == "ObstacleCollider")
        {
            FadeIn();
        }
    }
}
