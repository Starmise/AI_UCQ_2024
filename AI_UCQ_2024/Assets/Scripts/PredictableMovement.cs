using System.Drawing;
using System.Reflection;
using UnityEngine;

public class PredictableMovement : SimpleMovement
{
    [SerializeField]
    private GameObject[] PatrolPoints;

    private int CurrentPatrolPoint = 0;

    [SerializeField] private float PatrolPointToleranceRadius;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Utilities.Utility.IsInsideRadius(PatrolPoints[CurrentPatrolPoint].transform.position,
            transform.position, PatrolPointToleranceRadius))
        {
            CurrentPatrolPoint++; 
            CurrentPatrolPoint %= PatrolPoints.Length;

            // 0 % 4 = 0
            // 1 % 4 = 1
            // 2 % 4 = 2
            // 3 % 4 = 3
            // 4 % 4 = 0
            // 5 % 4 = 1

            // La otra forma sería usando un if
            // if (CurrentPatrolPoint >= PatrolPoints.Length)
            // {
            //     // Entonces lo reseteamos a 0.
            //     CurrentPatrolPoint = 0;
            // }
        }

        //Hacemos que el personaje haga seek al PatrolPoint que estemos yendo actualmente
        Vector3 PosToTarget = PuntaMenosCola(PatrolPoints[CurrentPatrolPoint].transform.position, transform.position);

        velocity += PosToTarget.normalized * maxAcceleration * Time.deltaTime;

        velocity = Vector3.ClampMagnitude(velocity, maxSpeed);

        transform.position += velocity * Time.deltaTime;
    }
}
