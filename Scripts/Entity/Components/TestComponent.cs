using Entities;
using Entities.Components;
using Godot;
using MySystems;
using System;
using Base.IO;

public class TestComponent : Node, IComponentNode
{
    public Entity MyEntity { get; set; }

    public void OnAwake()
    {
        
    }

    public void OnSetFree()
    {
        MyConsole.Write("Disposing the component");
    }

    public void OnStart()
    {
        string a = LoadDialogFromXML.LoadXmlElement("test_1", "second", LoadDialogFromXML.TextType.CHOICE);
        MyConsole.Write(a);
        //MyConsole.Write("Hola caracloa. \nmierda hasta los topes\n hoer como vamos\n no caigo ahora\nvamos a probar mas\njoder estoy hasta los winflis\nononono\nmieradad\nasdasda");
    }
    
    public void Reset()
    {
        //throw new NotImplementedException();
        
    }

    public override void _PhysicsProcess(float delta)
    {
        
    }


}
