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
            Console.WriteLine("Number of Students: ");
            int size = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Number of variants: ");
            int variantNr = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Which method do you want to use(possible values: 1,2,3)?");
            int solutionType = Convert.ToInt32(Console.ReadLine());
            int[][] matrix = CreateMatrix(size);
            List<Student> students = CreateStudents(matrix, variantNr);
            PrintMatrix(matrix);
            switch (solutionType)
            {
                case 1:
                    SearchSolution(matrix, variantNr);
                    break;
                case 2:
                    Console.WriteLine();
                    Console.Write("Is solution: ");
                    Console.Write(ChooseVariants(students));
                    break;
                case 3:
                    break;
            }
            Console.ReadKey();
        }

        private static bool ChooseVariants(List<Student> students)
        {
            if (IsSolution(students))
            {
                return true;
            }
            int index = MVR(students);
            for (int i = 0; i < students[index].Variants.Count; i++)
            {
                if (IsSafe(students, index, students[index].Variants[i]))
                {
                    var studentsTemp = students.ToList();
                    students[index].SelectedVariant = students[index].Variants[i];
                    students = ForwardChecking(students, index, students[index].SelectedVariant);
                    if (ChooseVariants(students))
                    {
                        return true;
                    }
                    students = studentsTemp;
                }
            }
            return false;
        }

        private static bool IsSolution(List<Student> students)
        {
            foreach (Student student in students)
            {
                if (student.SelectedVariant == 0)
                {
                    return false;
                }
            }
            foreach (int index in students[students.Count - 1].Neighbours)
            {
                if (students[students.Count - 1].SelectedVariant == students[index].SelectedVariant)
                {
                    return false;
                }
            }
            return true;
        }

        private static bool IsSafe(List<Student> students, int index, int variant)
        {
            foreach (int neighbour in students[index].Neighbours)
            {
                if (students[neighbour].SelectedVariant == variant)
                {
                    return false;
                }
            }
            return true;
        }

        private static List<Student> ForwardChecking(List<Student> students, int index, int variant)
        {
            for (int i = 0; i < students[index].Neighbours.Count; i++)
            {
                students[students[index].Neighbours[i]].Variants.Remove(variant);
            }
            return students;
        }

        private static int MVR(List<Student> students)
        {
            int index = 0, minVariants = students[0].Variants.Count;
            foreach (Student student in students)
            {
                if (student.Variants.Count < minVariants)
                {
                    minVariants = student.Variants.Count;
                    index = students.IndexOf(student);
                }
            }
            return index;
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
                    variants[variants.Length - 1] += 1;
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
            int[] variants = new int[matrix.Length+1];
            variants[variants.Length - 1] = 0;
            for (int i = 0; i < variantNr; i++)
            {
                variants[i] = 0;
            }
            if (ChooseVariants(matrix, variantNr, variants, 0) == false)
            {
                Console.WriteLine("No solution found!");
            }
            else
            {
                Console.WriteLine("Solution: ");
                PrintSolution(variants);
                Console.WriteLine();
                Console.Write("Number of initializations: ");
                Console.Write(variants[variants.Length - 1]);
            }
        }

        private static void PrintSolution(int[] variants)
        {
            for (int i = 0; i < variants.Length - 1; i++)
            {
                Console.Write(variants[i]);
                Console.Write(" ");
            }
        }
    }

}
