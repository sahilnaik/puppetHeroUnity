using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointController : MonoBehaviour
{
    public Sprite CheckpointUp;
    public Sprite CheckpointDown;
    private SpriteRenderer checkpointSpriteRenderer;
    public bool checkpointReached;
    [SerializeField] private AudioSource cp;
    // Start is called before the first frame update
    void Start()
    {
        checkpointSpriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void OnTriggerEnter2D(Collider2D other){
        if(other.tag == "Player")
        {
            checkpointSpriteRenderer.sprite = CheckpointDown;
            checkpointReached = true;
            cp.Play();
                    }
    }
}
