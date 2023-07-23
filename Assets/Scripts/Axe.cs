using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Axe : MonoBehaviour
{
    Animator animator;
    [SerializeField] Plank plank;

    SpriteRenderer plankSpriteRenderer;

    float respawnTime = 0.8f;

    void Awake()
    {
        animator = GetComponent<Animator>();
        plankSpriteRenderer = plank.GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            animator.SetTrigger("Cut");
        }
    }

    public void Cut()
    {
        if (plank.Cut())
        {
            plankSpriteRenderer.enabled = false;
            Invoke("RespawnPlank", respawnTime);
        }
    }

    void RespawnPlank()
    {
        plankSpriteRenderer.enabled = true;
    }
}
