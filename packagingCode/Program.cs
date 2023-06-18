using System;
using System.Collections.Generic;
using System.Linq;

class Program
{
    static void Main(string[] args)
    {
        var codeManager = new CodeManager();

     
        var codes = codeManager.GenerateCodes(1000);

      
        if (codeManager.CheckCode(codes[0]))
        {
            Console.WriteLine("CODE CORRECT!"); 
        }
        else
        {
            Console.WriteLine("CODE WRONG!"); 
        }
    }

    public class CodeManager
    {
        private static readonly char[] AvailableCharacters =
               { 'A', 'C', 'D', 'E', 'F', 'G', 'H', 'K', 'L', 'M', 'N', 'P', 'R', 'T', 'X', 'Y', 'Z', '2', '3', '4', '5', '7', '9' };
        private static int counter = 0;

        public List<string> GenerateCodes(int count)
        {
            return Enumerable.Range(1, count)
                             .Select(_ => GenerateCode())
                             .ToList();
        }

        private string GenerateCode()
        {
            counter = (counter + 1) % (int)Math.Pow(AvailableCharacters.Length, 8);
            var temp = counter;


            // // The counter loops up to 8 times the length of the available characters, and we generate a different code each loop
            return new string(Enumerable.Range(0, 8)
                                        .Select(i => AvailableCharacters[temp / (int)Math.Pow(AvailableCharacters.Length, i) % AvailableCharacters.Length])
                                        .Reverse()
                                        .ToArray());
        }

        public bool CheckCode(string code)
        {
            // We check the validity of the code
            return code.Length == 8 && code.All(c => AvailableCharacters.Contains(c));
        }
    }
}
