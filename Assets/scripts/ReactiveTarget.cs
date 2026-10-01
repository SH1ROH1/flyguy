
using UnityEngine;
using System.Collections;

public class ReactiveTarget : MonoBehaviour
{
    [Header("Health")]
    public int health = 3;

    [Header("Hit Reaction")]
    public float hitRotation = 15f;
    public float deathDelay = 1.5f;

    private bool dead = false;
    private WanderingAI wanderingAI;

    private void Start()
    {
        wanderingAI = GetComponent<WanderingAI>();
    }

    public void ReactToHit()
    {
        if (dead)
            return;

        health--;

        Debug.Log("Enemy hit! HP: " + health);

        if (health <= 0)
        {
            StartCoroutine(Die());
        }
        else
        {
            StartCoroutine(HitReaction());
        }
    }

    private IEnumerator HitReaction()
    {
        if (wanderingAI != null)
            wanderingAI.SetAlive(false);

        transform.Rotate(0f, hitRotation, 0f);

        yield return new WaitForSeconds(0.15f);

        if (!dead && wanderingAI != null)
            wanderingAI.SetAlive(true);
    }

    private IEnumerator Die()
    {
        dead = true;

        if (wanderingAI != null)
            wanderingAI.SetAlive(false);

        transform.Rotate(90f, 0f, 0f);

        yield return new WaitForSeconds(deathDelay);

        Destroy(gameObject);
    }
}
