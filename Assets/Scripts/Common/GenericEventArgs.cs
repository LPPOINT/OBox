using System;

namespace Assets.Scripts.Common
{
    public class GenericEventArgs<T> : EventArgs
    {
        public GenericEventArgs(T result)
        {
            Result = result;
        }

        public T Result { get; private set; }
    }

}
