using System;

namespace PersonInfoApp.Models
{
    public static class ZodiacCalculator
    {
        public static string GetWesternZodiac(DateTime birthDate)
        {
            int day = birthDate.Day;
            int month = birthDate.Month;

            return month switch
            {
                1 when day <= 19 => "Capricorn",
                1 => "Aquarius",
                2 when day <= 18 => "Aquarius",
                2 => "Pisces",
                3 when day <= 20 => "Pisces",
                3 => "Aries",
                4 when day <= 19 => "Aries",
                4 => "Taurus",
                5 when day <= 20 => "Taurus",
                5 => "Gemini",
                6 when day <= 20 => "Gemini",
                6 => "Cancer",
                7 when day <= 22 => "Cancer",
                7 => "Leo",
                8 when day <= 22 => "Leo",
                8 => "Virgo",
                9 when day <= 22 => "Virgo",
                9 => "Libra",
                10 when day <= 22 => "Libra",
                10 => "Scorpio",
                11 when day <= 21 => "Scorpio",
                11 => "Sagittarius",
                12 when day <= 21 => "Sagittarius",
                12 => "Capricorn",
                _ => "Unknown"
            };
        }

        public static string GetChineseZodiac(int year)
        {
            string[] animals = { "Monkey", "Rooster", "Dog", "Pig", "Rat", "Ox", "Tiger", "Rabbit", "Dragon", "Snake", "Horse", "Goat" };
            return animals[year % 12];
        }
    }
}
