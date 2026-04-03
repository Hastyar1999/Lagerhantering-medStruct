using System;
using System.Collections.Generic;
using Microsoft.Win32;
using System.IO;

namespace Butik
{
    public struct Shop                     // skriver ett OOP, detta handlar om att hantera en butik. 
                                           // koden skrivs i class
    {
        public int ArtikelNummer;
        public string Vara;
        public double Pris;
        public int LagerSaldo;
        public string Datum;

        public void PrintOut() // vi behöver dett metod för att Skriva ut resultatet. 
        { Console.WriteLine($"ArtikelNummer:{ArtikelNummer}, Vara: {Vara}, Pris: {Pris}, Lagersald: {LagerSaldo}, Datum: {Datum}");
        }

        public string WriteInFile() // används för att skriva till filen.
        {
            return ArtikelNummer + ";" + Vara + ";" + Pris + ";" + LagerSaldo + ";" + Datum;  //semikolon används för att skilja varje variabel sen när man åter använda koden. 
        }
    }


    internal class Program
    {
        // Här körs programmet
        static void Main(string[] args)
        {
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;

            List<Shop> butik = new List<Shop>(); // Skapar en lista som lagrar flera värden, där värden öka/minska. ´För att hantera lager i en butik är det bästa ha en lista. 

            bool avsluta = false;
            while (avsluta == false)             // Variablen för att köra menyn, blir true när användare väljer att avluta och då slutar menyn
            {
                Console.Clear();
                Console.WriteLine("=== Butik ===");
                Console.WriteLine("A) Read file");
                // LasInFil
                Console.WriteLine("B) Show data");
                // Visa Data
                Console.WriteLine("C) Sorting");
                // sortera datan
                Console.WriteLine("D) Add an item");
                // Lägga till data(genom append)
                Console.WriteLine("E) Remove data");
                // Ta bort data
                Console.WriteLine("F) Edit data) ");
                // Redigera data
                Console.WriteLine("G) Search) ");
                //Sökfunktion
                Console.WriteLine("H) Create/Overwrite File");
                // Skapar / skriver över fil
                Console.WriteLine("I) Gives recommendations");
                // Rekommendation
                Console.WriteLine("J) Save changes");
                // spara ändringar 
                Console.WriteLine("Q) Exit");
                Console.WriteLine();
                Console.Write("Val: ");

                string val = Console.ReadLine().Trim().ToUpper(); // Trim tar bort mellanslag, TOUpper omvandlar bokstäver till stor bokstav.

                switch (val)
                {
                    case "A":
                        Läsfrånfilen(butik);
                        Console.WriteLine("Inläsning klar.");
                        Console.WriteLine("Tryck valfri tangent för att fortsätta...");
                        break;
                    case "B":
                        VisaVarden(butik);
                        Console.WriteLine("Tryck valfri tangent för att fortsätta...");
                        break;
                    case "C":
                        SorteraEfterPris(butik);
                        Console.WriteLine("Tryck valfri tangent för att fortsätta...");
                        break;

                    case "D":
                        LäggTillData(butik);
                        break;
                    case "E":
                        TaBort(butik);
                        Console.WriteLine("Tryck valfri tangent för att fortsätta...");
                        break;
                    case "F":
                        RedigeraData(butik);
                        break;
                    case "G":
                        Sökväg(butik);
                        break;
                    case "H":
                        SkapaNyFil();
                        Console.WriteLine("Tryck valfri tangent för att fortsätta...");
                        break;
                    case "I":
                        rekomend(butik);
                        Console.WriteLine("Tryck valfri tangent för att fortsätta...");
                        break;
                    case "J":
                        SparaTillFil(butik);
                        Console.WriteLine("Sparningen lyckades");
                        break;

                    case "Q":
                        avsluta = true; // blir true och avslutar program
                        Console.WriteLine("Tryck valfri tangent för att avsluta");
                        break;

                }

                Console.ReadKey();
            }
        }
        // Här slutar Main()
        // Denna metod gör ju att artikelnummret går i ordning automatiskt istället för att användaren matar in, på så sätt kan hindra fel i programet.
        static int HögstaArtikelnummer(List<Shop> butik)
        {
            int max = 0;

            foreach (Shop b in butik)
            {
                if (b.ArtikelNummer > max)
                {
                    max = b.ArtikelNummer;
                }
            }

            return max;
        }

        //Denna metod Lägger till data, där den frågar användaren varans namn, pris, lagersaldo och Datum och sedan skrivs den in bakom senaste artikelnummret dvs +1.
        //Metoden returnerar inget och har list som parameter.
        static void LäggTillData(List<Shop> butik)
        {
            Console.Clear();  // rensa skärmen så att användaren inte blir förvirrad 

            Console.WriteLine("=== Lägg till vara ===");

            Shop b = new Shop();

            b.ArtikelNummer = HögstaArtikelnummer(butik) + 1; // Anropar Metod som hittar max värde på artikelnummer och sedan + 1 . listan butik används som argument till förre metoden.

            Console.WriteLine("Artikelnummer: " + b.ArtikelNummer); // skriver ut nya artikelnummer

            Console.Write("Vara: ");
            b.Vara = Console.ReadLine();

            Console.Write("Pris: ");
            // Sparar den inmatade värdet till b.pris i decimaltal, om fel inkommer så returnerar tryparse från false till true tack vare !. Så fortsätter koden tills rätt inmatning.
            // || b.Pris < 0  - tittar om inputen är mindre än 0 isåfall fortsätter koden tills rätt pris angett.
            while (!double.TryParse(Console.ReadLine(), out b.Pris) || b.Pris < 0)
                Console.Write("Fel. Ange pris: ");

            Console.Write("Lagersaldo: ");
            //Samma princip
            while (!int.TryParse(Console.ReadLine(), out b.LagerSaldo) || b.LagerSaldo < 0)
                Console.Write("Fel. Ange lagersaldo: ");

            Console.Write("Datum: ");
            b.Datum = Console.ReadLine();

            butik.Add(b);  // lägg värdet i våran huvud listan " butik"

            string fil;

            bool hitta = false;
            while (hitta == false)
            {
                Console.WriteLine("Ange filnamn att varan ska sparas i (eller skriv (X) för att avbryta):");
                fil = Console.ReadLine().Trim();

                if (fil.ToUpper() == "X")  // denna vilkor är för avsluta loopen
                {
                    hitta = true;
                    Console.WriteLine("Sparande avbrutet.");
                    break;
                }

                if (File.Exists(fil))  // om fillen finns så slutas våran loop direkt.
                {
                    using (StreamWriter sw = new StreamWriter(fil, true))  // append True.Detta gör att vi lägga till data på slutet i filen utan att överskriva hela filen.
                    {
                        sw.WriteLine(b.WriteInFile());        // anropa metoden WriteInFile för att skriva till filen.
                        b.PrintOut();
                    }

                    Console.WriteLine("Vara sparad i befintlig fil.");
                    break;  // loopen slutar här om filen redan finns.
                }
                else // om fillen inte finns, dubbel kolla med användare om hen har skrivit rätt filnamn. 
                {
                    Console.WriteLine($"Filen {fil} finns inte.");
                    Console.WriteLine("Vill du skapa den? JA / NEJ");

                    string svar = Console.ReadLine().Trim().ToUpper();

                    if (svar == "JA")   // när svaret är JA då skapas en ny fill på detta namn som användaren angav.
                    {
                        using (StreamWriter sw = new StreamWriter(fil, true))  // True
                        {
                            sw.WriteLine(b.WriteInFile());
                        }

                        Console.WriteLine("Ny fil skapad och ny vara sparingen lyckades.");
                        break; // slutar loopen då.
                    }
                    else if (svar == "NEJ") // om nej, kan detta betyda att användaren har råkat stt skriva fel. 
                                            // låt användaren rätta sig utan att tappa allt data som är inmatat av användaren.
                    {
                        Console.WriteLine("Okej, ange ett annat filnamn.");
                        continue;   // här gör att man inte tappar inmatade data och ge användaren möjlighet att rätta sig
                    }
                    else
                    {
                        Console.WriteLine("Fel svar. Försök igen.");
                        continue;  // här  tillåter koden endast (JA eller nej) från användaren.
                    }
                }
            }
            Console.WriteLine("Tryck valfri tangent för att fortsätta...");
            Console.ReadKey();
        }

        //Denna metod är en sök funktion med 4 val
        // List som parameter och returnerar inget
        static void Sökväg(List<Shop> butik)
        {
            bool loop = false;
            while (loop == false)
            {
                Console.Clear();
                Console.WriteLine("===Sök===");
                Console.WriteLine("Välj om du söker efter Artikelnummer eller Varans namn:");
                Console.WriteLine("A) Artikelnummer"); // skriver ut vald artikelnummer
                Console.WriteLine("B) Varans namn"); // skriver ut namn som matchar någon varunamn i listan
                Console.WriteLine("C) Sortera via max pris och max antal");

                Console.WriteLine("x) Avsluta");

                string val = Console.ReadLine().Trim().ToUpper(); // sparar användarens svar och gör om till rätt , trim tar bort mellanslag, toUpper omvandlar allt till stora bokstäver.

                bool hitta = false;

                if (val == "A")
                {
                    Console.Write("Ange artikelnummer: ");
                    string input = Console.ReadLine().Trim();
                    if (int.TryParse(input, out int artikelNummer))
                    {
                        foreach (Shop b in butik)
                        {
                            if (b.ArtikelNummer == artikelNummer)
                            {
                                b.PrintOut();
                                hitta = true;
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("Ogiltigt nummer!");
                    }
                }
                else if (val == "B")
                {
                    Console.Write("Ange varans namn: ");
                    string sök = Console.ReadLine().Trim();

                    foreach (Shop b in butik)
                    {
                        // stringcomparison.OrdinalIgnoreCase gör att stora och små bokstäver räknas som samma. 
                        if (b.Vara.Trim().Equals(sök, StringComparison.OrdinalIgnoreCase))
                        {
                            b.PrintOut();
                            hitta = true;
                        }
                    }
                }
                else if (val == "C")
                {
                    Console.Write("Ange maxpris: ");
                    double maxPris;

                    while (!double.TryParse(Console.ReadLine(), out maxPris))
                    {
                        Console.WriteLine("Fel! Ange ett giltigt pris:");
                    }

                    Console.Write("Ange max lagersaldo: ");
                    int maxSaldo;

                    while (!int.TryParse(Console.ReadLine(), out maxSaldo))
                    {
                        Console.WriteLine("Fel! Ange ett giltigt antal:");
                    }

                    foreach (Shop b in butik)
                    {
                        if (b.Pris <= maxPris && b.LagerSaldo <= maxSaldo)
                        {
                            b.PrintOut();
                            hitta = true;
                        }
                    }
                }
                else if (val == "X")
                {
                    Console.WriteLine("Tryck valfri tangent för att fortsätta...");
                    loop = true;
                    break;

                }
                else
                {
                    Console.WriteLine("Fel val, välj A - B - C - X.");
                    Console.WriteLine("Tryck valfri tangent för att fortsätta...");
                    Console.ReadKey();
                    continue;
                }

                if (!hitta && val != "X")
                {
                    Console.WriteLine("Varan hittades inte.");
                }

                Console.WriteLine("Tryck valfri tangent för att fortsätta...");
                Console.ReadKey();
            }
        }

        //Denna metod gör så användaren kan redigera befintlig data genom att ange artikelnummer, sen kan ange ny pris och lagersaldo.
        // returnerar inget och List butik som parameter
        static void RedigeraData(List<Shop> butik)
        {
            Console.Clear();
            Console.WriteLine("=== Redigera vara ===");

            Console.WriteLine("Ange artikelnummer");
            int nr;
            while (!int.TryParse(Console.ReadLine(), out nr))// Kollar att det är ett nummer.
            {
                Console.WriteLine("Fel. Ange artikelnummer");
            }
            for (int i = 0; i < butik.Count; i++)// Går igenom hela listan
            {
                if (butik[i].ArtikelNummer == nr)
                {
                    Shop temp = butik[i];// skapar en kopia av structen för att ändra den
                    Console.WriteLine("Vara hittad");
                    temp.PrintOut(); //skriver ut information om varan

                    // Kod loopar tills användaren anger ett giltigt positivt tal / double
                    // TryParse försöker konvertera inmatningen till double och spara värdet i temp.Pris
                    // Om konverteringen misslyckas ELLER priset är negativt så visas ett felmeddelande
                    Console.WriteLine("Nytt pris");
                    while (!double.TryParse(Console.ReadLine(), out temp.Pris) || temp.Pris < 0)
                    {
                        Console.WriteLine("Fel. Ange nytt pris");
                    }
                    Console.WriteLine("Nytt lagersaldo");
                    while (!int.TryParse(Console.ReadLine(), out temp.LagerSaldo) || temp.LagerSaldo < 0)
                    {
                        Console.WriteLine("Fel. Ange lagersaldo ");
                    }
                    butik[i] = temp;// sätter tillbaka kopian i listan så att ändringar sparars
                    Console.WriteLine("Vill du spara till fil? JA = 1 eller NEJ = 2");
                    string svar = Console.ReadLine();
                    if (svar == "1")
                    {
                        SparaTillFil(butik);

                    }
                    else
                    {
                        Console.WriteLine("Fil sparas ej");

                    }

                    Console.WriteLine("Tryck valfri tangent för att fortsätta...");
                    Console.ReadKey();
                    return;
                }

            }
            Console.WriteLine("Ingen vara hittades med det artikelnumret");
            Console.WriteLine("Tryck valfri tangent för att fortsätta...");
            Console.ReadKey();

        }

        //Metod Tittar om Lagersaldo är under 5 om den är så skrivs det ut att varans lagersaldo behövs fylla på.
        //Returnerar inget, har list butik som parameter.
        static void rekomend(List<Shop> butik)
        {
            double min = 5;
            bool hittad = false;


            foreach (Shop b in butik)
            {

                if (b.LagerSaldo <= min)
                {
                    b.PrintOut();
                    Console.WriteLine("lagersaldo behövs fylla på");
                    hittad = true;
                }

            }
            if (!hittad) Console.WriteLine("Alla varor har godkänt lagersaldo.");

        }


        //Metod Sorterar varor efter pris eller lagersaldo, även stigandes eller fallande beroende på input av användare.
        //Returnerar inget, har list butik som parameter.
        static void SorteraEfterPris(List<Shop> butik)
        {
            bool loop = false;

            while (!loop)
            {
                Console.WriteLine("Sortera efter:");
                Console.WriteLine("1 = Pris");
                Console.WriteLine("2 = Lagersaldo");
                Console.WriteLine("0 = Avsluta");
                Console.Write("Val: ");

                if (!int.TryParse(Console.ReadLine(), out int val))
                {
                    Console.WriteLine("Du måste välja rätt!");
                    continue;
                }

                if (val == 0)
                {
                    Console.WriteLine("Avslutar...");
                    return;
                }

                Console.WriteLine("Sortera:");
                Console.WriteLine("1 = Stigande");
                Console.WriteLine("2 = Fallande");
                Console.Write("Val: ");

                if (!int.TryParse(Console.ReadLine(), out int valS))
                {
                    Console.WriteLine("Du måste välja rätt!");
                    continue;
                }

                if (val == 1 && valS == 1)
                {
                    butik.Sort((a, b) => a.Pris.CompareTo(b.Pris));
                }
                else if (val == 1 && valS == 2)
                {
                    butik.Sort((a, b) => b.Pris.CompareTo(a.Pris));
                }
                else if (val == 2 && valS == 1)
                {
                    butik.Sort((a, b) => a.LagerSaldo.CompareTo(b.LagerSaldo));
                }
                else if (val == 2 && valS == 2)
                {
                    butik.Sort((a, b) => b.LagerSaldo.CompareTo(a.LagerSaldo));
                }
                else
                {
                    Console.WriteLine("Fel val.");
                    continue;
                }

                Console.WriteLine("=== Sorterad lista ===");

                foreach (Shop b in butik)
                {
                    b.PrintOut();
                }

                loop = true;
            }
        }


        //Metod läser in inmatad fil
        //Returnerar inge, List som parameter
        static void Läsfrånfilen(List<Shop> butik)
        {
            Console.WriteLine("Ange Filnamn");
            string val = Console.ReadLine();

            while (string.IsNullOrWhiteSpace(val))  // låter inte användaren tycka bara på enter. måste skriva filnamn 
            {
                Console.WriteLine("Du måste skriva ett filnamn.");
                val = Console.ReadLine();
            }

            if (!File.Exists(val))
            {
                Console.WriteLine("Filen finns inte än.");
                return;
            }
            butik.Clear();

            using (StreamReader sr = new StreamReader(val))
            {
                string rad;
                while ((rad = sr.ReadLine()) != null)
                {
                    string[] d = rad.Split(';');

                    Shop p = new Shop();
                    p.ArtikelNummer = int.Parse(d[0]);
                    p.Vara = d[1];
                    p.Pris = double.Parse(d[2]);
                    p.LagerSaldo = int.Parse(d[3]);
                    p.Datum = d[4];

                    butik.Add(p);
                }
            }
        }


        //Metod skapar ny fil med redan definerad 10 rader av värden.
        //Returnerar inget, ingen parameter
        static void SkapaNyFil() // Skapar helt ny fil / skriver över befintlig fil
        {
            Console.WriteLine("Ange filnamn att skapa eller skriva över: ");
            string filnamn = Console.ReadLine();


            try // felhantering
            {
                using (StreamWriter a = new StreamWriter(filnamn)) // Skriver i filnamn användare gav
                {
                    a.WriteLine("1;Sneakers Nike Air;1299;15;2026-03-01");
                    a.WriteLine("2;Känga Timberland;2100;8;2026-03-01");
                    a.WriteLine("3;Löparsko Adidas;1550;22;2026-03-02");
                    a.WriteLine("4;Sandaler Birkenstock;950;30;2026-03-02");
                    a.WriteLine("5;Festsko Läder;1800;5;2026-03-03");
                    a.WriteLine("6;Tofflor Cozy;299;40;2026-03-03");
                    a.WriteLine("7;Vinterstövel North;1999;12;2026-03-04");
                    a.WriteLine("8;Gympasko Puma;1200;18;2026-03-04");
                    a.WriteLine("9;Flipflops Basic;199;50;2026-03-05");
                    a.WriteLine("10;Chelsea Boot;1750;9;2026-03-05");
                }
                Console.WriteLine("Filen skapades / ändrades korrekt!");
            }
            catch (Exception) // felhantering
            {
                Console.WriteLine("Fel!");
            }
        }


        //Metoden tar bort en rad beroende på artikelnummer angivet av användaren.
        static void TaBort(List<Shop> butik)
        {
            Console.WriteLine("Ange ArtikelNummer att ta bort: ");
            string input = Console.ReadLine();

            int nummer;


            // double.TryParse returnerar true om konverteringen lyckas, annars false
            // Konverterad värde / input sparas i variablen nummer
            if (int.TryParse(input, out nummer))
            {

                bool hittad = false;

                for (int i = 0; i < butik.Count; i++)
                {
                    // ! Jämför artikelnumret i listan med det användaren gav
                    if (butik[i].ArtikelNummer == nummer)
                    {
                        butik.RemoveAt(i);
                        hittad = true;
                        Console.WriteLine("Posten är borttagen");
                        break;
                    }
                }

                // ! Om vi inte hittade posten, skriv felmeddelande
                if (!hittad)
                {
                    Console.WriteLine("Fel post nummer!");
                }
                else
                {
                    //Annars fråga användar
                    Console.WriteLine("Vill du Spara till fil? JA = 1 eller Nej = 2");
                    string inputs = Console.ReadLine();

                    if (inputs == "1")
                    {
                        // Anropar spara till fil metoden med nya världen
                        SparaTillFil(butik);
                    }
                    else if (inputs == "2")
                    {
                        Console.WriteLine("Fil sparas ej");
                    }
                    else
                    {
                        Console.WriteLine("Felaktig inmatning!");
                    }
                }
            }
            else
            {
                Console.WriteLine("Fel input!");
            }

        }


        //Metod skriver ut alla världen i fil genom att anropa metod VisaVarden
        //Metod returnerar inget, List som parameter
        static void VisaVarden(List<Shop> butik)
        {
            if (butik.Count == 0)
            {
                Console.WriteLine("Ingen data laddad.");
                return;
            }
            foreach (Shop b in butik)
            {
                b.PrintOut();

            }
        }

        //Metod sparar till fil där användare anger filnamn.
        //Returnerar inget, List butik som parameter.
        static void SparaTillFil(List<Shop> butik)
        {
            Console.WriteLine("Ange Filnamn att spara ändring till: ");
            string input = Console.ReadLine();


            try
            {
                using (StreamWriter a = new StreamWriter(input)) // StreamWriter gör så att vi kan skriva i filen
                {
                    foreach (Shop b in butik)
                    {
                        a.WriteLine(b.WriteInFile());
                    }
                }
                Console.WriteLine("Ändringar sparade.");
            }
            catch (Exception)
            {
                Console.WriteLine("Fel vid sparning.");
            }

        }
    }
}


