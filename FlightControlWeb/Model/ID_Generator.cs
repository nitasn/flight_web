using System;
namespace FlightControlWeb.Model
{
    interface I_ID_Generator
    {
        string GenerateID();
    }

    /// <summary>
    /// Result ID foramt:
    /// "LLL-DDDD" (L is a capital Letter, D is a Digit).
    /// </summary>
    class RandomBased_ID_Generator : I_ID_Generator
    {
        private Random rand = new Random();

        private const string LETTERS = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private const string NUMBERS = "0123456789";

        private char Ltr() => LETTERS[rand.Next(LETTERS.Length)];
        private char Num() => NUMBERS[rand.Next(NUMBERS.Length)];

        public string GenerateID() =>
            "" + Ltr() + Ltr() + Ltr() + Num() + Num() + Num() + Num();
    }
}
