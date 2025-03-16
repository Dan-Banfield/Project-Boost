using System.Collections;
using UnityEngine;

public class RocketController : MonoBehaviour
{
    #region Rocket Components

    [SerializeField] private Rigidbody2D rocketRigidbody2d;
    [SerializeField] private Transform rocketTransform;

    [SerializeField] private ParticleSystem jetParticleSystem;
    [SerializeField] private ParticleSystem successParticleSystem;
    [SerializeField] private ParticleSystem explosionParticleSystem;

    #endregion

    #region Rocket Modifiers

    [SerializeField] private float thrustSpeed = 800f;
    [SerializeField] private float rotationSpeed = 300f;

    #endregion

    [SerializeField] private GameManager gameManager;

    private enum State { Alive, Dead, Paused };
    private State rocketState = State.Alive;

    //Cache user-keybinds so they don't repeatadly have to be fetched.
    private KeyCode thrustKey;
    private KeyCode rotateLeftKey;
    private KeyCode rotateRightKey;

    private void Awake()
    {
        //Get custom user keybinds when script initialised.
        GetKeybinds();
    }

    private void Update()
    {
        if (rocketState == State.Alive) { HandleInput(); }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //When rocket collides, check collision object's tag.
        //Act accordingly.
        switch (collision.gameObject.tag)
        {
            case "Obstacle":
                StartCoroutine(LoseLevel());
                jetParticleSystem.Stop();
                break;
            case "Finish":
                StartCoroutine(FinishLevel(TimerManager.Instance.GetElapsedTime()));
                jetParticleSystem.Stop();
                break;
            default: break;
        }
    }

    private void GetKeybinds()
    {
        //Cache the keybinds for actions.
        thrustKey = KeybindManager.Instance.GetKeybind("Thrust");
        rotateLeftKey = KeybindManager.Instance.GetKeybind("RotateLeft");
        rotateRightKey = KeybindManager.Instance.GetKeybind("RotateRight");
    }

    private void HandleInput()
    {
        //Prevent unnecesarry rotation.
        rocketRigidbody2d.freezeRotation = true;

        if (Input.anyKeyDown)
            if (!TimerManager.Instance.Running()) { TimerManager.Instance.StartTimer(); }

        if (Input.GetKey(thrustKey)) { Thrust(); }
        else { StopThrusting(); }

        if (Input.GetKey(rotateLeftKey)) { RotateLeft(); }
        if (Input.GetKey(rotateRightKey)) { RotateRight(); }

        //Allow rotation again after user has stopped inputting.
        rocketRigidbody2d.freezeRotation = false;
    }

    private void Thrust()
    {
        //Achieve physics-based movement by adding force to the Y axis' facing direction.
        rocketRigidbody2d.AddRelativeForce(new Vector3(0, (thrustSpeed * Time.deltaTime), 0));

        PlayParticle("Thrust");
    }

    private void StopThrusting()
    {
        if (jetParticleSystem.isPlaying) { jetParticleSystem.Stop(); }
    }

    private void RotateLeft()
    {
        rocketTransform.Rotate(new Vector3(0, 0, (rotationSpeed * Time.deltaTime)));
    }

    private void RotateRight()
    {
        rocketTransform.Rotate(new Vector3(0, 0, -(rotationSpeed * Time.deltaTime)));
    }

    private void PlayParticle(string particleName)
    {
        if (rocketState != State.Alive) return;

        switch (particleName)
        {
            case "Explosion":
                if (!explosionParticleSystem.isPlaying) explosionParticleSystem.Play();
                break;
            case "Success":
                if (!successParticleSystem.isPlaying) successParticleSystem.Play();
                break;
            case "Thrust":
                if (!jetParticleSystem.isPlaying) jetParticleSystem.Play();
                break;
        }
    }

    private IEnumerator FinishLevel(float elapsedTime)
    {
        PlayParticle("Success");
        rocketState = State.Dead;

        yield return new WaitForSeconds(2);
        gameManager.FinishLevel(elapsedTime);
    }

    private IEnumerator LoseLevel()
    {
        PlayParticle("Explosion");
        rocketState = State.Dead;

        yield return new WaitForSeconds(2);
        gameManager.LoseLevel();
    }
}