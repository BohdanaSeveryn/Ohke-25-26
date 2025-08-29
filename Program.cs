using Newtonsoft.Json;
namespace Ohke_25_26;

class Program
{
    static void Main(string[] args)
    {
        //Using loop as menu
        while (true)
        {
            //Writeline menu options
            Console.WriteLine("Alkuaine-testi");
            Console.WriteLine("Haluatko pelata? (p)");
            Console.WriteLine("Haluatko tarkastella tuloksia? (t)");
            Console.WriteLine("Haluatko lopettaa? (l)");
            //Write on same line as input
            Console.Write("Valintasi: ");
            //Read input and convert to lowercase
            string? valinta = Console.ReadLine()!.ToLower();
            //Switch case for menu options
            switch (valinta)
            {
                case "p":
                    PelaPeli();
                    break;
                case "t":
                    TemporaryMethod();
                    break;
                case "l":
                    Console.WriteLine("Lopetetaan ohjelma.");
                    return;
                default:
                    Console.WriteLine("Virheellinen valinta. Yritä uudelleen.");
                    break;
            }
        }
    }
    //Temporary method for testing UI
    static void TemporaryMethod()
    {
        Console.WriteLine("This is a Method only for testing purposes.");
    }

    static void PelaPeli()
    {
        string elementsFile = "alkuaineet.txt";
        var correctElements = new HashSet<string>(
            File.ReadAllLines(elementsFile),
            StringComparer.OrdinalIgnoreCase
            );

        var userAnswers = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        Console.WriteLine("Anna 5 alkuainetta (ensimmäisistä 20).");
        while (userAnswers.Count < 5)
        {
            Console.Write($"Alkuaine {userAnswers.Count + 1}: ");
            string? answer = Console.ReadLine()?.Trim();

            if (string.IsNullOrEmpty(answer))
            {
                Console.WriteLine("Et syöttänyt mitään. Yritä uudelleen.");
                continue;
            }

            if (userAnswers.Contains(answer))
            {
                Console.WriteLine("Olet jo syöttänyt tämän alkuaineen. Kokeile toista.");
                continue;
            }

            userAnswers.Add(answer);
        }

        int correctCount = 0;
        foreach (var answer in userAnswers)
        {
            if (correctElements.Contains(answer))
                correctCount++;
        }

        int wrongCount = 5 - correctCount;
        Console.WriteLine($"\nOikein: {correctCount} / {5}");
        Console.WriteLine($"Väärin: {wrongCount} / {5}");

        string dateFolder = DateTime.Now.ToString("ddMMyyyy");
        Directory.CreateDirectory(dateFolder);

        string resultsPath = Path.Combine(dateFolder, "tulokset.json");

        List<string> results = new List<string>();

        if (File.Exists(resultsPath))
        {
            string existingJson = File.ReadAllText(resultsPath);
            results = JsonConvert.DeserializeObject<List<string>>(existingJson) ?? new List<string>();
        }
        string resultAsString = $"{correctCount} / {5}";

        results.Add(resultAsString);

        string json = JsonConvert.SerializeObject(results, Formatting.Indented);
        File.WriteAllText(resultsPath, json);

        Console.WriteLine($"Tulos tallennettu tiedostoon: {resultsPath}");
        Console.WriteLine();
    }

}




