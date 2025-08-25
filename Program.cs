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
                    TemporaryMethod();
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

}
