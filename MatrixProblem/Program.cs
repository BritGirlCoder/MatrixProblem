using System;

namespace MatrixProblem
{

    // The problem:
    // Consider a three by three matrix
    // Each square in the matrix contains a value 'x'
    // On each iteration through the matrix, for each square where x >= 4, then the following operations will be applied, if possible:
    // 1. Increment 'x' in the square directly above in the matrix by 1
    // 2. Increment 'x' in the square directly to the left in the matrix by 1
    // 3. Increment 'x' in the square directly to the right in the matrix by 1
    // 4. Increment 'x' in the square directly below in the matrix by 1
    // 5. For each successfully completed operation, decrement n in the original square by 1

    // Input: a three by three matrix (a more general solution would be n by m where n is the number of rows and m is the number of columns); number of times to iterate through the matrix
    // Output: a three by three matrix updated with the new values (a more general solution would be n by m)

    // Data structure: use a multidimensional array to represent the matrix (note this is a 3 x 4 array)
    // This is an improvement over the jagged array as it will enforce the specified requirement of each row having the same number of columns; 
    // a jagged array would be appropriate if we did not need the same number of columns in each row - better for a more general solution to this problem
    // int[,] matrix = {{1,2,3,4}, {4,3,2,1}, {0,2,4,6}}

    // Validations - confirm with business requirements:
    // 1. That the matrix is a 3 by 3 array, or the size required (3 x 4 in this code sample)
    // 2. That the matrix does not exceed the maximum size permitted for a two dimensional array structure - this will result in a compile time error on creation of the multidimensional array
    // 3. That m (#cols) is the same for each n (row) (I believe this will also take care of the #rows being the same for each col) - this will result in a compile time error if using a multidimensional array
    // 4. That each value n is != null - again, if defined as int rather than nullable int?, this will result in a compile time error when creating the multidimensional array
    // 5. That each value n is >= 0 - business rule - do we care? Omitted for now.
    // 6. That each value is a positive number - business rule - do we care? Omitted for now.

    public class Program
    {
        static void Main(string[] args)
        {
            // this replaces user input
            // normally this would come in as a JSON object from the UI to an appropriate endpoint

            int[,] matrix = { 
                { 1, 2, 3, 4 }, 
                { 4, 3, 2, 1 }, 
                { 0, 2, 4, 6 }
            };

            // Could be wrapped into the JSON object or could come from a query string
            // I'd usually map this and the JSON object into its own object (perhaps named "Matrix" object?) so I could run some validation methods in the object and return it to the UI
            int numIterations = 2;

            int[,] result = MatrixIterator.UpdateMatrix(matrix, numIterations);

            // This replaces a return of a MatrixResponse object back to the UI as part of an ActionResult
            // MatrixResponse would contain just the updated matrix itself and perhaps other useful information (userId, time requested, any messages, etc.)
            for (int row = 0; row < matrix.GetLength(0); row++)
            {
                for (int col = 0; col < matrix.GetLength(1); col++)
                {
                    Console.WriteLine(matrix[row, col]);
                }
            }

            Console.ReadLine();

            // expected output from for loop above:
            // (not output to the console) iteration 1:
            // 2 (1+1), 2 (2+0), 4 (3+1), 2 (4-2)
            // 1 (4-3), 4 (3+1), 3 (2+1), 2 (1+1)
            // 1 (0+1), 3 (2+1), 3 (4-3+1), 4 (6-2)
            // 
            // This is the actual output to the console: 
            // iteration 2:
            // 2 (1+0), 4 (2+2), 1 (4-3), 3 (2+1)
            // 2 (1+1), 0 (4-4), 5 (3+2), 3 (2+1)
            // 1 (1+0), 4 (3+1), 4 (3+1), 2 (4-2)
        }
    }

    public static class MatrixIterator
    {
       public static int[,] UpdateMatrix(int[,] matrix, int numIterations)
        {
            // counter to track how many times we have iterated?
            int iteration = 0;

            while (iteration < numIterations)
            {
                // loop through each row
                for (int row = 0; row < matrix.GetLength(0); row++)
                {
                    // loop through each column in each row - this will give me the value x of each element
                    for (int col = 0; col < matrix.GetLength(1); col++)
                    {
                        int currentValue = matrix[row, col];

                        // we know this is a two dimensional array; more readable
                        bool isFirstRow = row == matrix.GetLowerBound(0);
                        bool isLastRow = row == matrix.GetUpperBound(0);
                        bool isFirstCol = col == matrix.GetLowerBound(1);
                        bool isLastCol = col == matrix.GetUpperBound(1);

                        // step 1: is the current value >= 4?                        
                        if (currentValue >=4)
                        {
                            // step 2: increment above: row - 1, col, val++, currentValue-- if !isFirstRow
                            matrix[row-1,col] = !isFirstRow ? 
                            // step 3: increment left: row, col - 1, val++, currentValue--
                            // step 4: increment right: row, col + 1, val++, currentValue--
                            // step 5: increment down: row + 1, col, val++, currentValue--
                            
                        }                        
                    }
                }
            }

            return matrix;
        }

        private static int IncrementValue(int val)
        {
            return val++;
        }

        private static int DecrementValue(int val)
        {
            return val--;
        }
    }
}
