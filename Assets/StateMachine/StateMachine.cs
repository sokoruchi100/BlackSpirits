using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class StateMachine : MonoBehaviour
{
    private State currentState;


    private void Update()
    {
        currentState?.Tick(Time.deltaTime);
        if (Input.GetKeyDown(KeyCode.Escape)) {
            Application.Quit();
        }
        if (Input.GetKeyDown(KeyCode.R)) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    public void SwitchState(State newState) {
        currentState?.Exit();
        currentState = newState;
        currentState?.Enter();
    }
}
