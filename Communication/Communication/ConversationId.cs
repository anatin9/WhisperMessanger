using System;

namespace Communication
{
    public class ConversationId
    {
        private string _Id;

        public ConversationId()
        {
            _Id = Guid.NewGuid().ToString();
        }

        public string Id
        {
            get { return _Id; }
            set { this._Id = value; }
        }

        public override string ToString()
        {
            return _Id;
        }

        /*
        public static bool operator ==(ConversationId Left, ConversationId Right)
        {

            if (IsNullOrEmpty(Left) && IsNullOrEmpty(Right))
            {

            }
            if (Left == null && Right == null)
            {
                return true;
            }
            if (Left == null || Right == null)
            {
                return false;
            }

            return Left.ToString() == Right.ToString();
        }
        

        public static bool operator !=(ConversationId Left, ConversationId Right)
        {
            return Left.ToString() != Right.ToString();
        }
        */

        public override bool Equals(Object obj)
        {
            if (obj is ConversationId)
            {
                var that = obj as ConversationId;
                return this.ToString() == that.ToString();
            }
            return false;
        }
    }
}