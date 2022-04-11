using Godot;
using System;

namespace BehaviorTree.Base
{

    public class MessageByTick : BaseNode
    {
        [Export]
        private string _message;


        private enum MessageType : byte { LOG, ERROR, ADV }
        [Export]
        private MessageType _type;   
        

        public override void InitNode(in TreeController controller)
        {

        }

        public override States Tick(in TreeController controller)
        {
            if (_message != null)
            {
                switch (_type)
                {
                    case MessageType.ERROR:
                        MyConsole.WriteError(_message);
                        break;
                    case MessageType.ADV:
                        MyConsole.WriteWarning(_message);
                        break;
                    case MessageType.LOG:
                        MyConsole.Write(_message);
                        break;
                }
            }
            
            controller.ExitNode(this, States.SUCCESS);
            return this.NodeState;
        }
    }
}
