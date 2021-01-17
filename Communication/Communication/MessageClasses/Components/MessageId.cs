using System;

namespace Communication.MessageClasses.Components
{
    public class MessageId
    {
        private string _Id;

        public MessageId()
        {
            _Id = Guid.NewGuid().ToString();
        }

        override public string ToString()
        {
            return Id;
        }

        public override bool Equals(Object obj)
        {
            if (obj is MessageId)
            {
                var that = obj as MessageId;
                return this.ToString() == that.ToString();
            }
            return false;
        }

        public string Id
        {
            get { return this._Id; }
            set { this._Id = value; }
        }
    }
}