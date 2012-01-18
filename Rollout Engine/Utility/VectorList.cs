namespace Rollout.Utility
{
    public sealed class VectorList<T>
    {

        private T[] Array = null;

        public int Count { get; private set; }

        public T this[int Index]
        {
            get
            {
                return Array[Index];
            }
            set
            {
                Array[Index] = value;
            }
        }

        public VectorList()
        {
            Array = new T[8];
        }

        public VectorList(int Reserved)
        {
            Array = new T[Reserved];
        }

        public void Add(T Value)
        {
            Array[Count] = Value;
            Count++;
            if (Count >= Array.Length)
            {
                System.Array.Resize(ref Array, Array.Length << 1);
            }
        }

        public void Clear()
        {
            Count = 0;
        }

        public T[] ToArray()
        {
            if (Count == Array.Length)
            {
                return Array;
            }
            T[] NewArray = new T[Count];
            System.Array.Copy(Array, NewArray, Count);
            return NewArray;
        }

    }

}
