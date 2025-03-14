using UnityEngine;

public class RocketController : MonoBehaviour
{
    #region Rocket Components

    [SerializeField] private Rigidbody2D rocketRigidbody2d;
    [SerializeField] private Transform rocketTransform;

    #endregion

    #region Rocket Modifiers

    [SerializeField] private float thrustSpeed = 100f;
    [SerializeField] private float rotationSpeed = 50f;

    #endregion

    private enum State { Alive, Dead, Paused };
    private State rocketState = State.Alive;

    private void Update()
    {
        if (rocketState == State.Alive) { HandleInput(); }
    }

    private void HandleInput()
    {
        KeyCode thrustKey = KeybindManager.Instance.GetKeybind("Thrust");
        if (Input.GetKey(thrustKey)) { Thrust(); }
    }

    private void Thrust()
    {
        rocketRigidbody2d.AddRelativeForce(new Vector3(0, thrustSpeed * Time.deltaTime, 0));
    }
}