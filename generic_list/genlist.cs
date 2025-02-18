using System;

public class genlist<T>
{
    public T[] data;

    /* old add 
    public int size => data.Length; // property size
    public T this[int i] => data[i]; // indexer 
    public genlist() { data = new T[0]; } // constructor

    public void add(T item)
    { 
        T[] newdata = new T[size + 1];
        System.Array.Copy(data, newdata, size); // copy data to newdata
        newdata[size] = item; // add item to the end of newdata
        data = newdata; // assign newdata to data
    }
    */
    public int size = 0, capacity = 8; // size and capacity
    public genlist() { data = new T[capacity]; } // constructor
    public void add(T item)
    { /* add item to list */
        if (size == capacity)
        {
            T[] newdata = new T[capacity *= 2]; // double capacity if full 
            System.Array.Copy(data, newdata, size); // copy data to newdata
            data = newdata; // assign newdata to data
        }
        data[size] = item; // add item to the end of data
        size++; // increment size
    }

    /* old remove
    public void remove(int i)
    { 
        T[] newdata = new T[size - 1];
        System.Array.Copy(data, newdata, i); // copy from data[0] to data[i-1] to newdata
        System.Array.Copy(data, i + 1, newdata, i, size - i - 1); // copy from data[i+1] to data[size-1]
        data = newdata; // assign newdata to data

    }
    */
    public void remove(int i)
    { /* remove item at index i */
        if (i < 0 || i >= size) throw new IndexOutOfRangeException(); // check if i is out of range
        for (int j = i; j < size - 1; j++) data[j] = data[j + 1];
        size--;
    }

    public T this[int i] => data[i]; // indexer

}