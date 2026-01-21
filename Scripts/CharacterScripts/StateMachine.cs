using UnityEngine;

/// <summary>
/// A finite state machine implementation that manages state transitions and updates.
/// Handles the current state's lifecycle including Enter, Update, FixedUpdate, and Exit methods.
/// </summary>
public class StateMachine
{
    /// <summary>
    /// The currently active state in the state machine.
    /// </summary>
    public CharacterBaseState currentState { get; private set; }

    /// <summary>
    /// Initializes the state machine with a starting state.
    /// Calls the Enter method on the starting state.
    /// </summary>
    /// <param name="startState">The initial state to begin with.</param>
    public void Initialize(CharacterBaseState startState)
    {
        currentState = startState;
        currentState.Enter();
    }

    /// <summary>
    /// Transitions from the current state to a new state.
    /// Calls Exit on the current state, updates the current state reference, and calls Enter on the new state.
    /// </summary>
    /// <param name="newState">The state to transition into.</param>
    public void ChangeState(CharacterBaseState newState)
    {
        currentState.Exit();
        currentState = newState;
        currentState.Enter();
    }

    /// <summary>
    /// Updates the current state. Should be called in Unity's Update method.
    /// Delegates to the current state's Update method.
    /// </summary>
    public void UpdateActiveState()
    {
        currentState.Update();
    }

    /// <summary>
    /// Fixed updates the current state. Should be called in Unity's FixedUpdate method.
    /// Delegates to the current state's FixedUpdate method for physics-based updates.
    /// </summary>
    public void FixedUpdateActiveState()
    {
        currentState.FixedUpdate();
    }
}