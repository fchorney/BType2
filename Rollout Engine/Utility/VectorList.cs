namespace Rollout.Utility
{
    public sealed class VectorList<T>
    {

        private T[] array;

        public int Count { get; private set; }

        public T this[int index]
        {
            get
            {
                return array[index];
            }
            set
            {
                array[index] = value;
            }
        }

        public VectorList()
        {
            array = new T[8];
        }

        public VectorList(int reserved)
        {
            array = new T[reserved];
        }

        public void Add(T value)
        {
            array[Count] = value;
            Count++;
            if (Count >= array.Length)
            {
                System.Array.Resize(ref array, array.Length << 1);
            }
        }

        public void Clear()
        {
            Count = 0;
        }

        public T[] ToArray()
        {
            if (Count == array.Length)
            {
                return array;
            }
            T[] newArray = new T[Count];
            System.Array.Copy(array, newArray, Count);
            return newArray;
        }

    }

}
