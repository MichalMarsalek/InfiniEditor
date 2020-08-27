using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfiniEditor
{
    public class ListItem<T>
    {
        public string Text { get; private set; }
        public T Value { get; private set; }

        public ListItem(T value, string text)
        {
            Text = text;
            Value = value;
        }

        public override string ToString()
        {
            return Text;
        }
    }
}
