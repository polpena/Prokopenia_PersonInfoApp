using System.ComponentModel;
using System.Windows.Input;
using PersonInfoApp.Models;

namespace PersonInfoApp.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private string _firstName, _lastName, _email;
        private DateTime? _birthDate;
        private bool _isProcessing;
        private bool _canProceed;
        private bool _isUserInfoVisible;

        private string _computedAge, _computedWesternZodiac, _computedChineseZodiac, _computedIsAdult, _computedBirthdayMessage;

        public event PropertyChangedEventHandler PropertyChanged;

        public string FirstName { get => _firstName; set { _firstName = value; OnPropertyChanged(nameof(FirstName)); UpdateCanProceed(); } }
        public string LastName { get => _lastName; set { _lastName = value; OnPropertyChanged(nameof(LastName)); UpdateCanProceed(); } }
        public string Email { get => _email; set { _email = value; OnPropertyChanged(nameof(Email)); UpdateCanProceed(); } }
        public DateTime? BirthDate { get => _birthDate; set { _birthDate = value; OnPropertyChanged(nameof(BirthDate)); UpdateCanProceed(); } }
        public bool IsProcessing { get => _isProcessing; set { _isProcessing = value; OnPropertyChanged(nameof(IsProcessing)); } }
        public bool CanProceed { get => _canProceed; set { _canProceed = value; OnPropertyChanged(nameof(CanProceed)); } }
        public bool IsUserInfoVisible { get => _isUserInfoVisible; set { _isUserInfoVisible = value; OnPropertyChanged(nameof(IsUserInfoVisible)); OnPropertyChanged(nameof(IsUserInfoHidden)); } }

        public string ComputedAge { get => _computedAge; set { _computedAge = value; OnPropertyChanged(nameof(ComputedAge)); } }
        public string ComputedWesternZodiac { get => _computedWesternZodiac; set { _computedWesternZodiac = value; OnPropertyChanged(nameof(ComputedWesternZodiac)); } }
        public string ComputedChineseZodiac { get => _computedChineseZodiac; set { _computedChineseZodiac = value; OnPropertyChanged(nameof(ComputedChineseZodiac)); } }
        public string ComputedIsAdult { get => _computedIsAdult; set { _computedIsAdult = value; OnPropertyChanged(nameof(ComputedIsAdult)); } }
        public string ComputedBirthdayMessage { get => _computedBirthdayMessage; set { _computedBirthdayMessage = value; OnPropertyChanged(nameof(ComputedBirthdayMessage)); } }

        public ICommand ProceedCommand { get; }
        public ICommand BackCommand { get; }

        public MainViewModel()
        {
            IsUserInfoVisible = false;
            ProceedCommand = new RelayCommand(async _ => await ProceedAsync(), _ => CanProceed);
            BackCommand = new RelayCommand(_ => GoBack());
        }

        private void GoBack()
        {
            IsUserInfoVisible = false; 
        }

        private void UpdateCanProceed()
        {
            CanProceed = !string.IsNullOrWhiteSpace(FirstName) &&
                         !string.IsNullOrWhiteSpace(LastName) &&
                         !string.IsNullOrWhiteSpace(Email) &&
                         BirthDate.HasValue;
        }

        public bool IsUserInfoHidden => !IsUserInfoVisible;
        private async Task ProceedAsync()
        {
            IsProcessing = true;

            var person = new Person(FirstName, LastName, Email, BirthDate.Value);
            person.InvalidAge += () => BirthDate = null;
            await person.ComputePropertiesAsync();

            if (BirthDate == null)
            {
                IsProcessing = false;
                return;
            }

            ComputedAge = $"Age: {person.Age} years";
            ComputedWesternZodiac = $"Western Zodiac: {person.SunSign}";
            ComputedChineseZodiac = $"Chinese Zodiac: {person.ChineseSign}";
            ComputedIsAdult = $"Adult: {(person.IsAdult ? "Yes" : "No")}";
            ComputedBirthdayMessage = person.IsBirthday ? "🎉 Happy Birthday! 🎉" : "";

            IsProcessing = false;
            IsUserInfoVisible = true;
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

            if (propertyName == nameof(IsUserInfoVisible))
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CanProceed)));
            }
        }
    }
}