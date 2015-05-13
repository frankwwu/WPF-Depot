using System;

namespace WpfCustomWindow
{
    public class GenericEventArgs<T> : EventArgs
    {
        public T EventData { get; set; }

        public GenericEventArgs(T EventData)
        {
            this.EventData = EventData;
        }
    }
}
