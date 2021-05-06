using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpressionSolver : MonoBehaviour
{
    public string CreateExpressionFromComponents(List<Component> components)
    {
        List<string> letters = new List<string>();
        int i = 0;
        foreach(Component c in components)
        {
            letters.Add(((char)((i+65)%91)).ToString() + ((i/26) > 0 ? i / 26 : ""));
        }
        return "";
    }
    public List<Component> BuildFromExpression(string expression, bool simplify = true)
    {
        if(simplify) expression = SimplifyExpression(expression);
        return null;
    }

    public string SimplifyExpression(string expression)
    {

        return "";
    }
}
