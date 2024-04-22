using System.Collections;
using System.Text;

namespace Arrays
{
    public class DynamicArray<T> : ICloneable
    {
        private T[]? _array;
        public int Size { get; private set; }

        public DynamicArray(object? array)
        {
            if (array == null)
            {
                _array = null;
                Size = 0;
            }
            if (array is int[] arr)
            {
                _array = (T[])arr.Clone();
                Size = arr.Length;
            }
            else
            {
                throw new ArgumentException("The argument cannot be cast to a DynamicArray");
            }
        }
        public DynamicArray() : this(null) { }
        public DynamicArray(int size)
        {
            Size = size;
            _array = new T[Size];
        }


        public object Clone()
        {
            T[] tmpArr = new T[Size];
            for (int i = 0; i <  Size; i++) {
                tmpArr[i] = _array[i];
            }
            return tmpArr;
        }
        public IEnumerable GetArray(bool returnReversed = false)
        {
            if (_array == null) throw new NullReferenceException("It is not possible to reference a null array");
            return CurrentImplementation();

            IEnumerable CurrentImplementation()
            {
                if (!returnReversed)
                {
                    foreach (var item in _array)
                        yield return item;
                }
                else
                {
                    for (int i = Size; i != 0; --i)
                        yield return _array[i - 1];
                }
            }
        }
        public T GetValue(int index)
        {
            if (_array == null) throw new NullReferenceException("It is not possible to reference a null array");
            if (index < 0 || index >= Size) throw new ArgumentOutOfRangeException("Index invalid");
            return _array[index];
        }
        public void SetValue(int index, T value) { 
            if (index < 0 || index >= Size) throw new ArgumentOutOfRangeException("index");
            if (_array == null) throw new NullReferenceException("It is not possible to reference a null array");
            _array[index] = value;
        }
        public void InsertValue(int index, T value)
        {
            if (index < 0 || index > Size) throw new ArgumentOutOfRangeException("index");
            ++Size;
            if (_array == null)
            {
                _array = new T[Size];
                _array[0] = value;
            }
            else
            {
                T[] tmpArr = new T[Size];
                for (int i = 0, j = 0; i < Size; ++i)
                {
                    if (i == index)
                    {
                        tmpArr[i] = value;
                        continue;
                    }
                    tmpArr[i] = _array[j];
                    ++j;
                }
                
                _array = tmpArr;
            }
        }
        public void Push(T value)
        {
            this.InsertValue(Size - 1, value);
        }
        public void Resize(int newSize)
        {
            if (newSize < 0) throw new ArgumentException("Size cannot be less than zero");
            if (_array == null)
            {
                _array = new T[newSize];
            } 
            if (Size == newSize) return;
            else if (Size > newSize)
            {
                T[] tmpArr = new T[newSize];
                for (int i = 0; i < newSize; ++i)
                    tmpArr[i] = _array[i];
                _array = tmpArr;
            }
            else if (Size < newSize)
            {
                T[] tmpArr = new T[newSize];
                for (int i = 0; i < Size; ++i)
                    tmpArr[i] = _array[i];
                for (int i = Size; i < newSize; ++i)
                    tmpArr[i] = default(T);
                _array = FillDefault(tmpArr, Size, newSize);

            }
            Size = newSize;
            static T[] FillDefault(T[] arr, int start, int count)
            {
                for (int i = start; i < count; ++i)
                    arr[i] = default(T);
                return arr;
            }
        }
        public bool Contains(T val)
        {
            if (_array == null) return false;
            T[] tmpArr = (T[])_array.Clone();

            Array.Sort(tmpArr);
            return Array.BinarySearch(tmpArr, val) > -1 ? true : false;
        }
        public bool RemoveAll(T val)
        {
            if (_array == null) return false;
            if (!this.Contains(val)) return false;

            int k = this.Size;
            T[] tmpArr = (T[])_array.Clone();
            for (int i = 0; i < k; ++i)
            {
                bool flag = false;
                if (val.Equals(tmpArr[i]))
                {
                    Swap(ref tmpArr[i], ref tmpArr[k - 1]);
                    --k;
                    flag = true;
                }
                if (flag) --i;
            }
            _array = new T[k];
            Array.Copy(tmpArr, _array, k);
            return true;
            static void Swap(ref T a, ref T b)
            {
                T tmp = a;
                a = b;
                b = tmp;
            }
        }
        public void Sort(IComparer comparer)
        {
            if (_array == null) throw new NullReferenceException("It is not possible to reference a null array");
            Array.Sort(_array, comparer);
        }
        public override string ToString()
        {
            StringBuilder tmp = new StringBuilder();
            foreach (var item in this.GetArray())
                tmp.Append("[" + item.ToString() + "] ");
            return tmp.ToString().TrimEnd();
        }
        public override bool Equals(object? obj)
        {
            if (obj == null) throw new ArgumentNullException($"It is not possible to reference a null {nameof(obj)}");
            if (obj is DynamicArray<T> arr)
                return this.ToString() == arr.ToString() ? true : false;
            throw new ArgumentException($"Unable to convert {nameof(obj)} to DynamicArray");
        }
        public override int GetHashCode()
        {
            return this.ToString().GetHashCode();
        }
    }
}
