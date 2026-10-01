using UnityEngine;

public class WanderingAI : MonoBehaviour
{
    // За 19 варіантом
    public float speed = 3.5f;
    public float obstacleRange = 5.0f;
    public float sphereCastRadius = 0.65f;
    public float minTurnAngle = -100.0f;
    public float maxTurnAngle = 100.0f;

    public float speedIncreaseRate = 0.5f;

    float currentspeed = 0.0f;

    private bool _alive;

    private void Start()
    {
        _alive = true;
        currentspeed = speed;
    }

    private void Update()
    {
        if (!_alive)
            return;

        transform.Translate(0, 0, currentspeed * Time.deltaTime);

        Ray ray = new Ray(transform.position, transform.forward);
        if (Physics.SphereCast(ray, sphereCastRadius, out RaycastHit hit))
        {
            if (hit.distance < obstacleRange)
            {
                float angle = Random.Range(minTurnAngle, maxTurnAngle);
                transform.Rotate(0, angle, 0);
                currentspeed = speed;
            }
        }
                currentspeed += speedIncreaseRate * Time.deltaTime;
    }

    public void SetAlive(bool alive)
    {
        _alive = alive;
    }
}
