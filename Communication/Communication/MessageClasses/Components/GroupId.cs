using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Communication.MessageClasses.Components
{
    public class GroupId
    {
        private string _Id;

        public GroupId()
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
