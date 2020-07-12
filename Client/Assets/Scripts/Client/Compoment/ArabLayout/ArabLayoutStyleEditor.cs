//
// Author:  Johance
//
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArabLayoutStyleEditor : Attribute
{
    public Type [] CompmentTypes { get; set; }
    public string Name { get; set; }
    public ArabLayoutStyleEditor(Type compmentType, string name)
    {
        Name = name;
        CompmentTypes = new Type[1];
        CompmentTypes[0] = compmentType;
    }
    public ArabLayoutStyleEditor(string name, params Type[] compmentTypes)
    {
        Name = name;
        CompmentTypes = compmentTypes;
    }
    public bool HasCompament(RectTransform transform)
    {
        foreach(var item in CompmentTypes)
        {
            if (transform.GetComponent(item) != null)
            {
                return true;
            }
        }
        return false;
    }
};