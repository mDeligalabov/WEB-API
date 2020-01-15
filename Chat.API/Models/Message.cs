using System;
using System.Collections.Generic;
using System.Text;

namespace HBProducts.Chat.API.Models
{
    public class Message
    {
        private bool isEmployee;
        private string text;
        private string timeSend;
        private int id;

        public Message(bool isEmployee, string text, string timeSend, int id)
        {
            this.isEmployee = isEmployee;
            this.text = text;
            this.timeSend = timeSend;
            this.id = id;
        }

        public bool IsEmployee
        {
            get { return isEmployee; }
            set { isEmployee = value; }
        }

        public string Text
        {
            get { return text; }
          set { text = value; }
        }

        public string TimeSend
        {
            get { return timeSend; }
            set { timeSend = value; }
        }

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

    }
}
