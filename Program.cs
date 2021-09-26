using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace CS538_HW1
{
    class Program
    {
        static void Main(string[] args)
        {
            List<char> alphabet = new List<char>("abcdefghijklmnopqrstuvwxyz".AsEnumerable());

            var frequencies = new List<double>() { 0.08167, 0.01492, 0.02782, 0.04253, 0.12702, 0.02228, 0.02015,
                      0.06094, 0.06966, 0.00153, 0.00772, 0.04025, 0.02406, 0.06749,
                      0.07507, 0.01929, 0.00095, 0.05987, 0.06327, 0.09056, 0.02758,
                      0.00978, 0.02360, 0.00150, 0.01974, 0.00074 };

            var cipherText = args.Length > 0 && !string.IsNullOrEmpty(args[0]) ? args[0] : "rimqessfntwpwtelqdeerkwdpezrfxjumpqxqqurxhifdjaffirlffnsyahrxtoutszwaxwvdspwqbpidwhitvpdnvizstnexzrhpujfcwmfvihdcspkq" +
                "brvpczztxugnfdlhdzreheyrkworezrnrpcmatrsdesifqpwwfvnxdmssnsysgltzbgwyqbpidwoegidgnvxvgbyhivvgwrffcixogiskcwlvuchwpjdfnxewamcwcsxefwbrhvqvcmfmfyryotoefopscwweprfwnpxkmelxmwax" +
                "txfjecmbqejkvfyxuqnxxfbnzhqfaggpduzkdoclnzbupvespxhkvfomeqvtazbfdsranxwvabemogpsbgiupveqvictsbyhqzrgiiwdlpqbtmcvsstrsoctazqbemabfsutfzaxaueeeymjygxiqipkadlvpgpsbgiupvbofwlff" +
                "edezrrptthszruqpsbdssniofltifzprcbfvsgkcusiycqigeohpamgnpbfgudczcacbfithmfvrrrimqemabglttcogidgvscftjyjafzeizcoqvaanvtrrbmpqggeivhpltboeickbpywqbfiiystprpsevtkojyiphuipswmtx" +
                "khbhttfzaxfvrmcwcsxefwbrperusidsssgvowzmpiaapehfotqffscjpftrsooptkcspepwgwxeqfhsdzqapiwbyhfvresmsoesrhuirfaqfxqfgltdsusspghwtuhpnedflsjkqsjtfcysvpvbginspsbvwonvqofmcxzznsydy" +
                "imrbetxeoctazqbemabzsgvkjoiedeipuapoidbpvnghprvmducufzmzaeofxgfbhwceqvickwgtgmdcvdrqilrprrwxxbtnvkdgsvioqsmooykdiwusqeoesjerdzqbigeizcolptoehcvgtlweiztizcodqmyvrvjidsexubvxk" +
                "vndlmfqxdsfflonmnrpujfcwmflmizgusiafrxxtomwcbcfwxszfesnfrezjidseemfxtdpvemfwfmcwsbdmnzrxductzfkoaceiodemooyqtrbtelqgrwrysnpwmfrxwvffqsdspsbgiulxucaeacmtpggfrxwvffpbuggmcwcsx" +
                "efwbriyspcifwpeacmtpggfrwrysnpwfvnxeicwlfxmpececuminfboteoopbmacptzgusiabrxxdsqlhnigxwvgfdgtszihrffxsdsqmuwwdfpfhbmbgzfxizhglpehipxtsbvtkwdlpxmovtrybmpqphxrfaqfxmhvscrzmjwqq" +
                "hvtdsdsezwfqhtfzaxazbknismlxqrgirybpwssmuehiojdipoaybssszjxsteazgtfiewaxwviotxqrxmcxrpxeprvxxfbtesfvrvtximlxucasuzbwpwfwteiffzasisewpthspugweihrgvdtqqgistfjxmzoyxdyoooshsexw" +
                "vwsprofltizcovikwsehbsemcxojicwcsniysaxdkvfcaugrxwvitpviwypurqflgdwzmcrzdsedurxwvsmpgffbrxttszrfwrvufiooefwbrxjwogsxjrhxeodlwqwaxwvgvavqargdlfuzjfvryczhfowfogihnvjnliwyppjqf" +
                "cxmwamuisrfmdwakhlgqpgfsqggzajyexggseicwthqhuixisonvkdgmdeyfjwfcyelvbgzvoszickwtfrocawizhvemabnpiysfqjugnvvlworxtogxwzgjdehwbppkwpysrhuigzuiesrbbxqvworjafpiskcjygdwzmcrhfzrq" +
                "grpurghtzqbvriysgtjfvnqternprf";

            int patternLength = 5;
            var patternDict = new Dictionary<string, int>();

            for (int i = 0; i < cipherText.Length; i += patternLength)
            {
                if (i + patternLength > cipherText.Length - 1) continue;

                var substring = cipherText.Substring(i, patternLength);
                if (!patternDict.ContainsKey(substring)) patternDict.Add(substring, 0);

                patternDict[substring]++;
            }

            var maxPatternCount = patternDict.Max(kvp => kvp.Value);

            var distances = new List<int>();
            foreach (var kvp in patternDict.Where(kvp => kvp.Value == maxPatternCount))
            {
                bool searching = true;
                int currentIndex = -1;
                while (searching)
                {
                    var nextIndex = cipherText.IndexOf(kvp.Key, currentIndex == -1 ? 0 : currentIndex + patternLength);

                    if (nextIndex == -1)
                    {
                        searching = false;
                        continue;
                    }
                    else if (currentIndex != -1)
                    {
                        distances.Add(nextIndex - currentIndex);
                    }

                    currentIndex = nextIndex;
                }
            }

            var period = distances.Aggregate(GCD);

            var slices = new List<string>();

            for (int i = 0; i < cipherText.Length; i += period)
            {
                var substring = "";
                if (i + period > cipherText.Length - 1)
                    substring = cipherText.Substring(i);
                else
                    substring = cipherText.Substring(i, period);
                slices.Add(substring);
            }

            var kList = new List<int>();
            for (int t = 0; t < period; t++)
            {
                var tText = "";

                foreach (var slice in slices.Where(sl => sl.Length == period)) tText += slice[t];

                var iDict = new Dictionary<int, double>();
                for (int j = 0; j < alphabet.Count; j++)
                {
                    double I = 0;
                    for (int i = 0; i < alphabet.Count; i++)
                    {
                        var alphabetIndex = i + j;
                        if (alphabetIndex < 0) alphabetIndex += 26;
                        if (alphabetIndex > 25) alphabetIndex -= 26;
                        I += frequencies[i] * tText.Where(c => c == alphabet[alphabetIndex]).Count() / tText.Length;
                    }
                    iDict.Add(j, I);
                }

                var nearestJ = iDict.OrderBy(kvp => Math.Abs(kvp.Value - 0.065)).First();
                kList.Add(nearestJ.Key);
            }

            Console.WriteLine($"Key: {string.Join("", kList.Select(x => alphabet[x]))}");

            var plaintext = "";
            foreach (var slice in slices)
            {
                for (int i = 0; i < slice.Length; i++)
                {
                    var alphabetIndex = alphabet.IndexOf(slice[i]) - kList[i];
                    if (alphabetIndex < 0) alphabetIndex += 26;
                    if (alphabetIndex > 25) alphabetIndex -= 26;

                    plaintext += alphabet[alphabetIndex];
                }
            }

            Console.WriteLine("Plaintext: " + plaintext);
            Console.WriteLine("\nPress any key to exit");
            Console.ReadKey();
        }

        static int GCD(int a, int b)
        {
            return b == 0 ? a : GCD(b, a % b);
        }
    }
}
