using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Ohke_25_26
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("Alkuaine-testi");
                Console.WriteLine("Haluatko pelata? (p)");
                Console.WriteLine("Haluatko tarkastella tuloksia? (t)");
                Console.WriteLine("Haluatko lopettaa? (l)");
                Console.Write("Valintasi: ");
                string? valinta = Console.ReadLine()!.ToLower();

                switch (valinta)
                {
                    case "p":
                        PelaPeli();
                        break;
                    case "t":
                        TarkasteleTuloksia();
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

            int correctCount = userAnswers.Count(answer => correctElements.Contains(answer));
            int wrongCount = 5 - correctCount;

            Console.WriteLine($"\nOikein: {correctCount} / 5");
            Console.WriteLine($"Väärin: {wrongCount} / 5");

            string dateFolder = DateTime.Now.ToString("ddMMyyyy");
            Directory.CreateDirectory(dateFolder);

            string resultsPath = Path.Combine(dateFolder, "tulokset.json");

            List<string> results = new List<string>();

            if (File.Exists(resultsPath))
            {
                string existingJson = File.ReadAllText(resultsPath);
                results = JsonConvert.DeserializeObject<List<string>>(existingJson) ?? new List<string>();
            }

            string resultAsString = $"{correctCount} / 5";
            results.Add(resultAsString);

            string json = JsonConvert.SerializeObject(results, Formatting.Indented);
            File.WriteAllText(resultsPath, json);

            Console.WriteLine($"Tulos tallennettu tiedostoon: {resultsPath}\n");
        }

        static void TarkasteleTuloksia()
        {
            string currentDirectory = Directory.GetCurrentDirectory();

            var resultFiles = Directory.GetDirectories(currentDirectory)
                .Where(dir =>
                {
                    string folderName = Path.GetFileName(dir);
                    return folderName.Length == 8 && folderName.All(char.IsDigit);
                })
                .Select(dir => Path.Combine(dir, "tulokset.json"))
                .Where(File.Exists)
                .ToList();

            if (resultFiles.Count == 0)
            {
                Console.WriteLine("Ei löytynyt yhtään tulostiedostoa.");
                return;
            }

            int totalResults = 0;
            int totalScore = 0;

            foreach (var file in resultFiles)
            {
                try
                {
                    string json = File.ReadAllText(file);
                    var results = JsonConvert.DeserializeObject<List<string>>(json);
                    if (results == null) continue;

                    foreach (var result in results)
                    {
                        var parts = result.Split('/');
                        if (parts.Length == 2 &&
                            int.TryParse(parts[0].Trim(), out int correct) &&
                            int.TryParse(parts[1].Trim(), out int max))
                        {
                            totalResults++;
                            totalScore += correct;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Virhe luettaessa tiedostoa {file}: {ex.Message}");
                }
            }

            if (totalResults == 0)
            {
                Console.WriteLine("Ei pystytty laskemaan tuloksia.");
                return;
            }

            double average = (double)totalScore / totalResults;
            Console.WriteLine($"\nKeskiarvo {totalResults} pelistä: {average:F2} / 5 ({average * 20:F1}%)\n");
        }
    }
}
