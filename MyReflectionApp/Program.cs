using MyReflectionApp;
using MyReflectionClassLibrary;
using System.Diagnostics;
using System.Text.Json;

internal class Program
{
    private static void Main(string[] args)
    {
        var myF = new F().Get();

        // количество итераций
        int iterations = 100000;

        // csv
        var csvTime1 = TestSerializeToCsv(myF, iterations, false);
        var csvTime2 = TestSerializeToCsv(myF, iterations, true);
        string csv = MySerializer.ObjToCsv(myF);
        var csvTime3 = TestDeserializeFromCsv(csv, iterations);

        // json
        var jsonTime1 = TestSerializeToJson(myF, iterations, false);
        var jsonTime2 = TestSerializeToJson(myF, iterations, true);
        string json = JsonSerializer.Serialize(myF, new JsonSerializerOptions { IncludeFields = true });
        var jsonTime3 = TestDeserializeFromJson<F>(json, iterations);

        // выводим результаты
        Console.WriteLine($"\nIterations count = {iterations}");

        Console.WriteLine($"\nMy reflection (csv):");
        Console.WriteLine($"Serialization without console output: {csvTime1} ms");
        Console.WriteLine($"Serialization with console output: {csvTime2} ms");
        Console.WriteLine($"Deserialization: {csvTime3} ms");

        Console.WriteLine($"\nStandard System.Text.Json.JsonSerializer:");
        Console.WriteLine($"Serialization without console output: {jsonTime1} ms");
        Console.WriteLine($"Serialization with console output: {jsonTime2} ms");
        Console.WriteLine($"Deserialization: {jsonTime3} ms");

        Console.ReadLine();
    }

    /// <summary>
    /// Тестирование сериализации в CSV.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="t">Объект для сериализации</param>
    /// <param name="iterations">Количество итераций</param>
    /// <param name="isShow">Выводить в консоль</param>
    /// <returns></returns>
    static long TestSerializeToCsv<T>(T t, int iterations, bool isShow = false) where T : class
    {
        var timer = Stopwatch.StartNew();
        string csv;

        for (int i = 0; i <= iterations; i++)
        {
            csv = MySerializer.ObjToCsv(t);
            if (isShow)
                Console.WriteLine($"#{i}\n{csv}");
        }

        timer.Stop();
        return timer.ElapsedMilliseconds;
    }

    /// <summary>
    /// Тестирование сериализации в JSON.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="t">Объект для сериализации</param>
    /// <param name="iterations">Количество итераций</param>
    /// <param name="isShow">Выводить в консоль</param>
    /// <returns></returns>
    static long TestSerializeToJson<T>(T t, int iterations, bool isShow = false) where T : class
    {
        string json;
        var timer = Stopwatch.StartNew();

        for (int i = 0; i <= iterations; i++)
        {
            json = JsonSerializer.Serialize(t, new JsonSerializerOptions { IncludeFields = true });
            if (isShow)
                Console.WriteLine($"#{i}\n{json}");
        }

        timer.Stop();
        return timer.ElapsedMilliseconds;
    }

    /// <summary>
    /// Тестирование десериализации из CSV.
    /// </summary>
    /// <param name="csv">Сериализованная строка CSV</param>
    /// <param name="iterations">Количество итераций</param>
    /// <returns></returns>
    static long TestDeserializeFromCsv(string csv, int iterations)
    {
        var timer = Stopwatch.StartNew();

        for (int i = 0; i <= iterations; i++)
        {
            var v = MySerializer.CsvToObj<F>(csv);
        }

        timer.Stop();
        return timer.ElapsedMilliseconds;
    }

    /// <summary>
    /// Тестирование десериализации из JSON.
    /// </summary>
    /// <typeparam name="T">Тип</typeparam>
    /// <param name="json">Сериализованная строка JSON</param>
    /// <param name="iterations">Кол-во итераций</param>
    /// <returns></returns>
    static long TestDeserializeFromJson<T>(string json, int iterations)
    {
        var timer = Stopwatch.StartNew();
        for (int i = 0; i <= iterations; i++)
        {
            var v = JsonSerializer.Deserialize(json, typeof(T), new JsonSerializerOptions { IncludeFields = true });
        }
        timer.Stop();
        return timer.ElapsedMilliseconds;
    }
}