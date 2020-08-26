using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIManager : ScriptableObject
{
    private List<AINavigator> _navigators = new List<AINavigator>();


    public AINavigator[] Navigators => _navigators.ToArray();


    public void Initialize()
    {
        _navigators = new List<AINavigator>();
    }
    
    public void AddNavigator(AINavigator navigator)
    {
        if (!_navigators.Contains(navigator))
            _navigators.Add(navigator);
    }

    public void RemoveNavigator(AINavigator navigator)
    {
        if (_navigators.Contains(navigator))
            _navigators.Remove(navigator);
    }
}
