using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackTrack
{
    class Program
    {
        static void Main(string[] args)
        {
            int size = 5, variantNr = 3;
            int[][] matrix = CreateMatrix(size);
            PrintMatrix(matrix);
            SearchSolution(matrix, variantNr);
            List<Student> students = CreateStudents(matrix, variantNr);
            Console.ReadKey();
        }

        private static List<Student> CreateStudents(int[][] matrix, int variantNr)
        {
            List<Student> students = new List<Student>(matrix.Length);
            for (int i = 0; i < matrix.Length; i++)
            {
                Student student = new Student();
                List<int> neighbours = new List<int>();
                for (int j = 0; j < matrix.Length; j++)
                {
                    if (matrix[i][j] == 1)
                    {
                        neighbours.Add(j);
                    }
                }
                student.Neighbours = neighbours;
                students.Add(student);
            }
            List<int> variants = new List<int>(variantNr);
            for (int i = 1; i <= variantNr; i++)
            {
                variants.Add(i);
            }
            foreach (var student in students)
            {
                student.Variants = variants;
                student.SelectedVariant = 0;
            }
            return students;
        }

        private static void PrintMatrix(int[][] matrix)
        {
            foreach (var row in matrix)
            {
                foreach (var column in row)
                {
                    Console.Write(column);
                    Console.Write(" ");
                }
                Console.WriteLine();
            }
        }

        private static int[][] CreateMatrix(int size)
        {
            int[][] matrix = new int[size][];
            Random rand = new Random();
            for (int i = 0; i < size; i++)
            {
                matrix[i] = new int[size];
            }
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (i == j)
                    {
                        matrix[i][j] = 0;
                        matrix[i][j] = 0;
                    }
                    else if (rand.Next(1, 10) <= 5)
                    {
                        matrix[i][j] = 0;
                        matrix[j][i] = 0;
                    }
                    else
                    {
                        matrix[i][j] = 1;
                        matrix[j][i] = 1;
                    }
                }
            }
            return matrix;
        }

        private static bool IsSafe(int index, int[][] matrix, int[] variants, int variant)
        {
            for (int i = 0; i < matrix.Length; i++)
            {
                if (matrix[index][i] == 1 && variant == variants[i])
                {
                    return false;
                }
            }
            return true;
        }

        private static bool ChooseVariants(int[][] matrix, int variantNr, int[] variants, int index)
        {
            if (index == matrix.Length)
            {
                return true;
            }
            for (int i = 1; i < variantNr; i++)
            {
                if (IsSafe(index, matrix, variants, i))
                {
                    variants[index] = i;
                    if (ChooseVariants(matrix, variantNr, variants, index + 1))
                    {
                        return true;
                    }
                    variants[i] = 0;
                }
            }
            return false;
        }

        private static void SearchSolution(int[][] matrix, int variantNr)
        {
            int[] variants = new int[matrix.Length];
            for (int i = 0; i < variantNr; i++)
            {
                variants[i] = 0;
            }
            if (ChooseVariants(matrix, variantNr, variants, 0) == false)
            {
                Console.WriteLine("No solution found!");
            }
            Console.WriteLine("Solution: ");
            PrintSolution(variants);
        }

        private static void PrintSolution(int[] variants)
        {
            foreach (var item in variants)
            {
                Console.WriteLine(item);
            }
        }
    }

}
