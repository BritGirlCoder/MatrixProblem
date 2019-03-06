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
    // 7. That each value is a valid integer (int.TryParse)
    // 8. That the number of iterations is reasonable and is not a negative number

    // Unit tests - tests for each validation item above
    // Also that the returned values are valid according to the business rules and for the type

    public class Program
    {
        static void Main(string[] args)
        {
            // this replaces user input (assuming this is coming from a UI)
            // normally this would come in as a JSON object from the UI to an appropriate endpoint
            // see above notes on validations.

            int[,] matrix = { 
                { 1, 2, 3, 4 }, 
                { 4, 3, 2, 1 }, 
                { 0, 2, 4, 6 }
            };

            // Could be wrapped into the JSON data or could come from a query string
            // I'd usually map this and the JSON object into its own class (perhaps named "Matrix"?) so I could run some validation methods in the object and return it to the UI; 
            // would use NewtonSoft for this - JsonConvert.DeserializeObject<T>(string) where T is the Matrix type and string is the JSON data
            // Having a Matrix class would also allow for a default setting of the number of iterations, if not set in the API call
            int numIterations = 2;

            // we could also simply call this method by the number of iterations from the request, and avoid both an extra parameter and the While outer loop
            int[,] result = MatrixIterator.UpdateMatrix(matrix, numIterations);

            // This replaces a return of a (for example) MatrixResponse object back to the UI as part of an ActionResult
            // response object being kept separate from the Matrix object allows for returning only what's needed
            // MatrixResponse would contain just the updated matrix itself and perhaps other useful information (userId, time requested, any messages, etc.)
            for (int row = 0; row < matrix.GetLength(0); row++)
            {
                for (int col = 0; col < matrix.GetLength(1); col++)
                {
                    Console.WriteLine($"{row}, {col}: {result[row, col]}");
                }
            }

            Console.ReadLine();
        }
    }

    public static class MatrixIterator
    {
       public static int[,] UpdateMatrix(int[,] matrix, int numIterations)
        {
            // counter to track how many times we have iterated
            int iteration = 0;
            
            while (iteration < numIterations)
            {
                // we need a way to store the increment and decrement values for each position; it will be cleared after each round
                int[,] changeTrackerMatrix = new int[matrix.GetLength(0), matrix.GetLength(1)];

                // loop through each row
                for (int row = 0; row < matrix.GetLength(0); row++)
                {
                    // loop through each column in each row - this will give me the value x of each element
                    for (int col = 0; col < matrix.GetLength(1); col++)
                    {
                        // not necessary, but adds a little to readability
                        int currentValue = matrix[row, col];

                        // we know this is a two dimensional array; adding bool variables for sake of readability
                        bool isFirstRow = row == matrix.GetLowerBound(0);
                        bool isLastRow = row == matrix.GetUpperBound(0);
                        bool isFirstCol = col == matrix.GetLowerBound(1);
                        bool isLastCol = col == matrix.GetUpperBound(1);

                        // step 1: is the current value >= 4?                        
                        if (currentValue >=4)
                        {
                            // step 2: if we have a row above, add 1 to that position in the change tracker and subtract 1 for current position
                            if (!isFirstRow)
                            {
                                // increment value for position above by 1 in change tracker
                                changeTrackerMatrix[row - 1, col] = changeTrackerMatrix[row - 1, col] + 1;
                                // decrement value for this position by 1
                                changeTrackerMatrix[row, col] = changeTrackerMatrix[row, col] - 1;
                            }

                            // step 3: if we have a column to the left, add 1 to that position in the change tracker and subtract 1 for current position
                            if (!isFirstCol)
                            {
                                // increase value for position to left by 1 in change tracker
                                changeTrackerMatrix[row, col - 1] = changeTrackerMatrix[row, col - 1] + 1;
                                // decrement value for this position by 1
                                changeTrackerMatrix[row, col] = changeTrackerMatrix[row, col] - 1;
                            }

                            // step 4: if we have a column to the right, add 1 to that position in the change tracker and subtract 1 for current position
                            if (!isLastCol)
                            {
                                // increase value for position to right by 1 in change tracker
                                changeTrackerMatrix[row, col + 1] = changeTrackerMatrix[row, col + 1] + 1;
                                // decrement value for this position by 1
                                changeTrackerMatrix[row, col] = changeTrackerMatrix[row, col] - 1;
                            }
                            // step 5: if we have a row below, add 1 to that position in the change tracker and subtract 1 for current position
                            if (!isLastRow)
                            {
                                // increase value for position below by 1 in change tracker
                                changeTrackerMatrix[row + 1, col] = changeTrackerMatrix[row + 1, col] + 1;
                                // decrement value for this position by 1
                                changeTrackerMatrix[row, col] = changeTrackerMatrix[row, col] - 1;
                            }                            
                        }                        
                    }
                }
                iteration++;
                // after each round we need to ensure we'll be using the new values for the next round
                 matrix = ModifyMatrix(changeTrackerMatrix, matrix);
            }            

            // I also could have created another 2-dimensional array to hold the updated values; chose to omit as this is 
            return matrix;
        }

        private static int[,] ModifyMatrix(int[,] changeTracker, int[,] matrixToUpdate)
        {
            for (int row = 0; row < matrixToUpdate.GetLength(0); row++)
            {
                // loop through each column in each row - this will allow me to access each value and modify it if necessary
                for (int col = 0; col < matrixToUpdate.GetLength(1); col++)
                {
                    // only make changes if the changeTracker value != 0
                    if (changeTracker[row,col] != 0)
                    {
                        matrixToUpdate[row, col] = matrixToUpdate[row, col] + changeTracker[row, col];
                    }
                }
            }
            return matrixToUpdate;
        }
    }
}
