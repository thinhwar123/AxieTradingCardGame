using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface State<T> 
{
    public void Enter(T go);
    public void Execute(T go);

    public void Exit(T go);
}
