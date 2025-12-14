using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Globalization;
using static System.Net.Mime.MediaTypeNames;

namespace PraceSTextovymiSoubory
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8; //díky tomu můžu psát ř do konzole

            using (StreamWriter sw = new StreamWriter("2.txt"))
            {
                sw.WriteLine("Ahoj \tsvěte!\n");
            }
            using (StreamWriter sw = new StreamWriter("4.txt"))
            {
                sw.WriteLine("1");
                sw.WriteLine("2");
                sw.WriteLine("3");
            }
            using (StreamWriter sw = new StreamWriter("5.txt"))
            {
                sw.Write("1\n2\n3");
            }



            //------------------------------ ÚLOHA 1 ------------------------------//
            // (10b) 1. Jaký je počet znaků v souboru 1.txt a jaký v 2.txt?
            // Zkontrolujte s VS Code a vysvětlete rozdíly.
            // Tip: Při Debugování uvidíte všchny čtené znaky.

            //TXT1 ... spočítá jinak ... kvůli: Ahoj[mezera][TAB]světe![\n][\r][\n]
            string text_1_1 = File.ReadAllText("1.txt");
            int pocet_znaku_1_1 = text_1_1.Length;
            Console.WriteLine("Počet všech znaků v TXT 1: " + pocet_znaku_1_1);

            string text_1_2 = File.ReadAllText("2.txt");
            int pocet_znaku_1_2 = text_1_2.Length;
            Console.WriteLine("Počet všech znaků v TXT 2: " + pocet_znaku_1_2);



            //------------------------------ ÚLOHA 2 ------------------------------//
            // (10b) 2. Jaký je počet znaků v souboru 1.txt, když pomineme bílé znaky?
            // Tip: Struktura Char má statickou funkci IsWhiteSpace().
            //buď
            string text_1_3 = File.ReadAllText("1.txt");
            int pocetZnaku1_2 = text_1_3.Count(c => !char.IsWhiteSpace(c)); //vezme vše jen ne bílé znaky
            Console.WriteLine("Počet znaků v TXT 1 (1): " + pocetZnaku1_2);

            //nebo
            string text_string_2 = File.ReadAllText("1.txt");
            string text_strip_2 = text_string_2.Trim().Replace(" ", ""); //Trim() ... odstraní bílé znaky na začátku a na konci řetězce
                                                                         //nahradí mezery za nic ... "Ahoj ahoj" -> "Ahojahoj"
            int pocet_znaku_2 = text_strip_2.Length; //počet znaků
            Console.WriteLine("Počet znaků v TXT 1 (2): " + pocet_znaku_2);



            //------------------------------ ÚLOHA 3 ------------------------------//
            // (5b) 3. Jaké znaky (vypište jako integery) jsou použity pro oddělení řádků v souboru 3.txt?
            // Porovnejte s 4.txt a 5.txt.
            // Jakým znakům odpovídají v ASCII tabulce? https://www.ascii-code.com/
            // Zde se stačí podívat do VS Code a napsat sem odpověď, není potřeba nic programovat.
            string text_lines_3 = File.ReadAllText("3.txt");
            foreach (char c in text_lines_3)
            {
                if (c == '\n') //různé typy odělovače
                {
                    Console.Write($"Hodnota: {(int)c}; ");
                }
                if (c == '\r')
                {
                    Console.Write($"Hodnota: {(int)c}; ");
                }
            }


            Console.WriteLine();


            string text_lines_4 = File.ReadAllText("4.txt");
            foreach (char c in text_lines_4)
            {
                if (c == '\n') //různé typy odělovače
                {
                    Console.Write($"Hodnota: {(int)c}; ");
                }
                if (c == '\r')
                {
                    Console.Write($"Hodnota: {(int)c}; ");
                }
            }

            Console.WriteLine();


            string text_lines_5 = File.ReadAllText("5.txt");
            foreach (char c in text_lines_5)
            {
                if (c == '\n') //různé typy odělovače
                {
                    Console.Write($"Hodnota: {(int)c}; ");
                }
                if (c == '\r')
                {
                    Console.Write($"Hodnota: {(int)c}; ");
                }
            }


            Console.WriteLine();


            // (10b) 4. Kolik slov má soubor 6.txt?
            // Za slovo teď považujme neprázdnou souvislou posloupnost nebílých znaků oddělené bílými.
            // Tip: Split defaultně odděluje na základě libovolných bílých znaků, ale je tam jeden háček.. jaký?
            // V souboru je vidět 52 slov.
            int pocet_slov = 0;
            string text_6 = File.ReadAllText("6.txt");

            if (text_6.Length > 0)
            {
                pocet_slov = text_6.Split((char[])null, StringSplitOptions.RemoveEmptyEntries).Length; //split bez argumentů odebere všechny bílé znaky ... splitnu ho dle b9l7ch znak; (když zdvojené, tak beru jen jeden) a spočítám)
            }
            Console.WriteLine("Počet slov v 6.txt: " + pocet_slov);


            Console.WriteLine();


            // (15b) 5. Zapište do souboru 7.txt slovo "řeřicha". Povedlo se? 
            // Vypište obsah souboru do konzole. V čem je u konzole problém a jak ho spravit?
            // Jaké kódování používá C#? Kolik bytů na znak? - používá UTF-16 (Unicode) a 2 bajty (16 bitů) na jeden char
            using (StreamWriter sw = new StreamWriter("7.txt"))
            {
                sw.Write("řeřicha");
            }
            string text_7 = File.ReadAllText("7.txt");
            Console.WriteLine("Výpis 7.txt: " + text_7);



            // (25b) 6. Vypište četnosti jednotlivých slov v souboru 8.txt do souboru 9.txt ve formátu slovo:četnost na samostatný řádek.
            // Tentokrát však slova nejprve očištěte od diakritiky a všechna písmena berte jako malá (tak je i ukládejte do slovníku).
            // Tip: Využijte slovník: Dictionary<string, int> slova = new Dictionary<string, int>();
            string text_8 = File.ReadAllText("8.txt");
            string text_8_strip = RemoveDiacritics(text_8).ToLower(); //textu odeberu diakritiku a dám vše na malá písmena
            string[] text_8_slova = text_8_strip.Split((char[])null /*rozdělím text podle libovolného bílého znaku*/, StringSplitOptions.RemoveEmptyEntries /*když budou např. 2 mezery za sebou, tak to rozdělí jen podle jedné a neudále to řetezec i s jen mezerou*/);

            Dictionary<string, int> cetnost_slov = new Dictionary<string, int>();
            foreach (string slovo in text_8_slova)
            {
                if (cetnost_slov.ContainsKey(slovo))
                {
                    cetnost_slov[slovo]++; //když slovo už jako klíč mám, zvýším u toho klíče value
                }

                else
                {
                    cetnost_slov[slovo] = 1; //jestli slovo nemám, tak ho přidám a nastavím četnost na 1 ... poprvé objeveno
                }
            }

            using (StreamWriter sw = new StreamWriter("9.txt"))
            {
                foreach (var radek in cetnost_slov)
                {
                    sw.WriteLine(radek.Key + ":" + radek.Value);
                }
            }



            // (+15b) Bonus: Vypište četnosti jednotlivých znaků abecedy (malá a velká písmena) v souboru 8.txt do konzole.

            //#endregion
        }


        //credit na toto patří zde https://stackoverflow.com/questions/249087/how-do-i-remove-diacritics-accents-from-a-string-in-net ... funkci chápu, ale nevymyslela bych ji...
        static string RemoveDiacritics(string text)
        {
            var normalizovany_text = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder(capacity: normalizovany_text.Length);
            for (int i = 0; i < normalizovany_text.Length; i++)
            {
                char c = normalizovany_text[i];
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }

    }
}
