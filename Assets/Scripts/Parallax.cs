using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    private Vector2 size;
    private Vector2 startpos;
    public GameObject cam;
    public float parallaxEffect;
    
    // Start is called before the first frame update
    void Start()
    {
        startpos = transform.position;
        size = GetComponent<SpriteRenderer>().bounds.size;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        var camPosition = cam.transform.position;
        Vector2 offset = camPosition * parallaxEffect;
        Vector2 temp = camPosition * (1 - parallaxEffect);
        transform.position = startpos + offset;
        if (temp.x > startpos.x + size.x)
        {
            startpos.x += size.x;
        } else if (temp.x < startpos.x)
        {
            startpos.x -= size.x;
        }
        
        if (temp.y > startpos.y + size.y)
        {
            startpos.y += size.y;
        } else if (temp.y < startpos.y)
        {
            startpos.y -= size.y;
        }
    }
}
