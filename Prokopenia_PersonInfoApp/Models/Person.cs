using System.Windows;

namespace PersonInfoApp.Models
{
    public class Person
    {
        public string FirstName { get; }
        public string LastName { get; }
        public string Email { get; }
        public DateTime? BirthDate { get; }
        public bool IsAdult { get; private set; }
        public int Age { get; private set; }
        public string SunSign { get; private set; }
        public string ChineseSign { get; private set; }
        public bool IsBirthday { get; private set; }

        public Person(string firstName, string lastName, string email, DateTime birthDate)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            BirthDate = birthDate;
        }

        public Person(string firstName, string lastName, string email)
            : this(firstName, lastName, email, default) { }

        public Person(string firstName, string lastName, DateTime birthDate)
            : this(firstName, lastName, "", birthDate) { }

        public async Task ComputePropertiesAsync()
        {
            if (BirthDate.HasValue)
            {
                int age = DateTime.Today.Year - BirthDate.Value.Year;
                if (DateTime.Today.Month < BirthDate.Value.Month ||
                   (DateTime.Today.Month == BirthDate.Value.Month && DateTime.Today.Day < BirthDate.Value.Day))
                    age--;

                if (age < 0 || age > 135)
                {
                    MessageBox.Show("Invalid age! Please enter a valid birth date.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    OnInvalidAge();
                    return;
                }

                Age = age;
                IsAdult = Age >= 18;
                SunSign = await Task.Run(() => ZodiacCalculator.GetWesternZodiac(BirthDate.Value));
                ChineseSign = await Task.Run(() => ZodiacCalculator.GetChineseZodiac(BirthDate.Value.Year));
                IsBirthday = BirthDate.Value.Day == DateTime.Today.Day && BirthDate.Value.Month == DateTime.Today.Month;
            }
        }

        public event Action InvalidAge;
        private void OnInvalidAge() => InvalidAge?.Invoke();
    }
}
