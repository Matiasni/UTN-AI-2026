using System;
using System.Collections.Generic;
using UnityEngine;

public class ExampleLambdaActionFunc : MonoBehaviour
{
    //Delegado: tipo que permite guardar una referencia a un método(función) y luego ejecutarlo a través de esa referencia.
    //Action: delegado que representa un método que no devuelve ningún valor (void).
    //Lambda: función anónima que se puede definir directamente donde se necesita.
    //Func: delegado que representa un método que devuelve un valor.


    public Action action;
    Func<int , bool> func;

    void Start()
    {
        action += () => Debug.Log("HOLA");

        func = isTrue;

        SarasaTwo(sarsa => sarsa > 0);

        SarasaTwo(isTrue);
    }


    public void DebugAsd()
    {
        Debug.Log("HOla");
    }

    bool isTrue(int i)
    {
        return i > 0;
    }

    public void Debug2(int i)
    {
        Debug.Log(i);
    }

    public void Sarasa(Action action)
    {
        action();
    }

    public void SarasaTwo(Func<int, bool> func)
    {
        func(3);
    }
}