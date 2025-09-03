using System;
using UnityEngine;

public class EventBus : MonoBehaviour
{
    private static EventBus _instance;

    public static EventBus Instance
    {
        get
        {
            // If we don't have an instance yet, and the game is running, find or create it.
            if (_instance == null && Application.isPlaying)
            {
                // First, try to find one in the scene.
                _instance = FindFirstObjectByType<EventBus>();

                // If none exists, create one.
                if (_instance == null)
                {
                    var singletonObject = new GameObject(typeof(EventBus).ToString() + " (Auto-Generated)");
                    _instance = singletonObject.AddComponent<EventBus>();
                }
            }
            // Return the instance (which will be null if the game isn't playing)
            return _instance;
        }
    }

    public event Action<int> OnGoal;
    public event Action OnReflection;

    private void Awake()
    {
        if (_instance == null)
        {
            // If this is the first instance, make it the singleton and make it persistent.
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (_instance != this)
        {
            // If a singleton already exists, this one is a duplicate. Destroy it.
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        // If this is the singleton instance that is being destroyed...
        if (_instance == this)
        {
            // Clear event subscribers to prevent memory leaks from dangling references
            OnGoal = null;
            OnReflection = null;
            // Clear the static instance reference
            _instance = null;
        }
    }

    // --- Your Trigger Methods ---
    public void TriggerGoal(int direction) { OnGoal?.Invoke(direction); }
    public void TriggerReflection() { OnReflection?.Invoke(); }
}
