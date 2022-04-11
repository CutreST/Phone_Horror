using Godot;
using System;

namespace BehaviorTree.Base{
    public abstract class BaseNode : Node, IBehaviorNode
    {
        public States NodeState { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public abstract void InitNode(in TreeController controller);


        public abstract States Tick(in TreeController controller);

    }
}

