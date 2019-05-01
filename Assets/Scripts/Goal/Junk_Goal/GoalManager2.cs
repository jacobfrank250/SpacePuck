using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalManager2 : MonoBehaviour
{

    void OnEnable()
    {
        Goal.OnGoalScored += OnGoalScored;
    }
        // Start is called before the first frame update
        void Start()
    {
        
    }

    void OnGoalScored(Enums.Teams Team)
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
