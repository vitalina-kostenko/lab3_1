using System.Text;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

internal class Program1
{
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Program_1();
    }

    static void Program_1()
    {
        string directoryPath = "C:\\Users\\koste\\source\\repos\\Lab3_1\\lab_3\\input\\";
        string[] files = {
            "10.txt", "11.txt", "12.txt", "13.txt", "14.txt", "15.txt", "16.txt",
            "17.txt", "18.txt", "19.txt", "20.txt", "21.txt", "22.txt", "23.txt",
            "24.txt", "25.txt", "26.txt", "27.txt", "28.txt", "29.txt"
        };

        int sum = 0;
        int count = 0;

        StringBuilder noFile = new StringBuilder();
        StringBuilder badData = new StringBuilder();
        StringBuilder overflow = new StringBuilder();

        Random random = new Random();

        EnsureDirectoryExists(directoryPath);

        foreach (var file in files)
        {
            string filePath = Path.Combine(directoryPath, file);
            bool isNewFile = false;

            try
            {
                string[] lines = File.ReadAllLines(filePath);
                AssertValidData(lines);
                int num1Parsed = int.Parse(lines[0]);
                int num2Parsed = int.Parse(lines[1]);

                int product = checked(num1Parsed * num2Parsed);

                sum += product;
                count++;
            }
            catch (FileNotFoundException)
            {
                int num1 = random.Next(1, 100);
                int num2 = random.Next(1, 100);
                File.WriteAllText(filePath, $"{num1}\n{num2}");
                noFile.AppendLine($"File {filePath} was created with numbers: {num1} and {num2}.");
                isNewFile = true;
            }
            catch (FormatException)
            {
                badData.AppendLine(filePath);
            }
            catch (OverflowException)
            {
                overflow.AppendLine(filePath);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка з файлом {filePath}: {ex.Message}");
            }
        }

        WriteToFile(Path.Combine(directoryPath, "no_file.txt"), noFile.ToString());
        WriteToFile(Path.Combine(directoryPath, "bad_data.txt"), badData.ToString());
        WriteToFile(Path.Combine(directoryPath, "overflow.txt"), overflow.ToString());

        double average = count > 0 ? (double)sum / count : 0;
        Console.WriteLine(count > 0 ? $"Середнє арифметичне: {average}" : "Немає допустимих добутків для обчислення середнього.");
    }

    static void AssertValidData(string[] lines)
    {
        int.Parse(lines[0]);
        int.Parse(lines[1]);
    }

    static void EnsureDirectoryExists(string directoryPath)
    {
        Directory.CreateDirectory(directoryPath);
    }

    static void WriteToFile(string filePath, string content)
    {
        File.AppendAllText(filePath, content + Environment.NewLine);
    }
}
