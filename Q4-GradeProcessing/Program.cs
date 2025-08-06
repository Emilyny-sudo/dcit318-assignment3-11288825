using System;
using System.Collections.Generic;
using System.Linq;

class Program
{
    static void Main()
    {
        Dictionary<string, double> studentGrades = new Dictionary<string, double>();

        Console.Write("Enter number of students: ");
        int count = int.Parse(Console.ReadLine());

        for (int i = 0; i < count; i++)
        {
            Console.Write($"Enter name for student {i + 1}: ");
            string name = Console.ReadLine();

            Console.Write($"Enter grade for {name}: ");
            double grade = double.Parse(Console.ReadLine());

            studentGrades[name] = grade;
        }

        double average = studentGrades.Values.Average();
        Console.WriteLine($"\nClass average: {average:F2}");

        Console.WriteLine("\nStudents who scored above average:");
        foreach (var student in studentGrades)
        {
            if (student.Value > average)
            {
                Console.WriteLine($"{student.Key} - {student.Value}");
            }
        }
    }
}
