using UnityEngine;

/// <summary>
/// Abstract base class for all character states in the state machine pattern.
/// Provides core functionality for state lifecycle management, animation control, and timing.
/// </summary>
public abstract class CharacterBaseState
{
   protected readonly StateMachine stateMachine;
   protected readonly string animBoolName;
   
   /// <summary>
   /// Controls whether this state can be transitioned into.
   /// When false, the state machine will not allow transitions to this state.
   /// </summary>
   protected bool stateActive = true;
   
   /// <summary>
   /// A general-purpose timer that counts down in Update.
   /// Can be used by derived states for timed behaviors.
   /// </summary>
   protected float stateTimer;
   
   /// <summary>
   /// Tracks whether an animation trigger has been called from an animation event.
   /// Reset to false on Enter, set to true via CallAnimationTrigger.
   /// </summary>
   protected bool triggerCalled;

   /// <summary>
   /// Constructor for creating a new character state.
   /// </summary>
   /// <param name="stateMachine">Reference to the state machine managing this state.</param>
   /// <param name="animBoolName">The name of the animation boolean parameter to control.</param>
   protected CharacterBaseState(StateMachine stateMachine, string animBoolName)
   {
      this.stateMachine = stateMachine;
      this.animBoolName = animBoolName;
   }

   /// <summary>
   /// Called when entering this state. Override to implement state-specific initialization.
   /// Base implementation resets the triggerCalled flag.
   /// </summary>
   public virtual void Enter()
   {
      triggerCalled = false;
   }

   /// <summary>
   /// Called every frame while this state is active. Override to implement state-specific logic.
   /// Base implementation decrements the stateTimer.
   /// </summary>
   public virtual void Update()
   {
      stateTimer -= Time.deltaTime;
   }

   /// <summary>
   /// Called during FixedUpdate while this state is active. Override for physics-based logic.
   /// </summary>
   public virtual void FixedUpdate() { }

   /// <summary>
   /// Called when exiting this state. Override to implement cleanup logic.
   /// </summary>
   public virtual void Exit() { }
   
   /// <summary>
   /// Called by animation events to signal that a specific point in the animation has been reached.
   /// Sets triggerCalled to true, which can be checked in the Update method.
   /// </summary>
   public void CallAnimationTrigger()
   {
      triggerCalled = true;
   }
   
   /// <summary>
   /// Checks whether this state is currently active and can be transitioned into.
   /// </summary>
   /// <returns>True if the state is active and can be entered, false otherwise.</returns>
   public bool isStateActive() => stateActive;
   
   /// <summary>
   /// Enables or disables this state, controlling whether it can be transitioned into.
   /// Useful for implementing skill unlocking or temporarily disabling certain moves.
   /// </summary>
   /// <param name="value">True to enable the state, false to disable it.</param>
   public void SetStateActive(bool value)
   {
      stateActive = value;
   }
}