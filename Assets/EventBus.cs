using System;
using System.Dynamic;
using Unity.VisualScripting;
using UnityEngine;

public class EventBus : MonoBehaviour
{
    public static EventBus Instance { get; private set; }


    public event Action<int> OnGoal;

    public event Action OnReflection; 





    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }    
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);  
        }
    }

    public void TriggerGoal(int x) // -1 for left goal and 1 for right one 
    {
        Debug.Log(" goal event "); 

        OnGoal?.Invoke(x);
    }

    public void TriggerReflection()
    {
        //Debug.Log("refelction event tirggred");

        OnReflection?.Invoke();
    }


}
