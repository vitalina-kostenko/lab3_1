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
        string directoryPath = "C:\\Users\\koste\\source\\repos\\lab_3\\lab_3\\input";
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
                if (!File.Exists(filePath))
                {
                    int num1 = random.Next(50000, 100000);
                    int num2 = random.Next(50000, 100000);
                    File.WriteAllText(filePath, $"{num1}\n{num2}");
                    noFile.AppendLine($"File {filePath} was created with numbers: {num1} and {num2}.");
                    isNewFile = true;
                }

                string[] lines = File.ReadAllLines(filePath);
                AssertValidData(lines);

                int num1Parsed = int.Parse(lines[0]);
                int num2Parsed = int.Parse(lines[1]);

                if (isNewFile && num1Parsed == 0 && num2Parsed == 0)
                {
                    continue;
                }

                int product = checked(num1Parsed * num2Parsed);

                sum += product;
                count++;
            }
            catch (FormatException)
            {
                badData.AppendLine(filePath);
            }
            catch (OverflowException)
            {
                overflow.AppendLine(filePath);
            }
        }

        string noFilePath = Path.Combine(directoryPath, "no_file.txt");
        string badDataPath = Path.Combine(directoryPath, "bad_data.txt");
        string overflowPath = Path.Combine(directoryPath, "overflow.txt");

        try
        {
            File.WriteAllText(noFilePath, noFile.ToString());
            File.WriteAllText(badDataPath, badData.ToString());
            File.WriteAllText(overflowPath, overflow.ToString());
        }
        catch
        {
            Console.WriteLine("Помилка запису в файли.");
            return;
        }

        if (count > 0)
        {
            double average = (double)sum / count;
            Console.WriteLine($"Середнє арифметичне: {average}");
        }
        else
        {
            Console.WriteLine("Немає допустимих добутків для обчислення середнього.");
        }
    }

    static void AssertValidData(string[] lines)
    {
        if (lines.Length < 2 || !int.TryParse(lines[0], out _) || !int.TryParse(lines[1], out _))
        {
            throw new FormatException();
        }
    }

    static void EnsureDirectoryExists(string directoryPath)
    {
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }
    }
}
