using UnityEngine;

public class ArrayExample : MonoBehaviour
{
    // Define the size of the 2D array
    public int rows = 3;
    public int columns = 4;

    // Expose the 2D array to the Unity Inspector
    [SerializeField]
    private int[,] myArray;

    void Start()
    {
        // Initialize the 2D array with random values (you can use your own logic here)
        InitializeArray();

        // Display the 2D array in the console
        DisplayArray();
    }

    void InitializeArray()
    {
        // Initialize the 2D array with random values
        myArray = new int[rows, columns];

        // You can set your own logic for initializing values
        // For now, let's fill the array with zeros
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                myArray[i, j] = 0;
            }
        }
    }

    void DisplayArray()
    {
        // Display the 2D array in the console
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                Debug.Log("Element at [" + i + "," + j + "]: " + myArray[i, j]);
            }
        }
    }
}
