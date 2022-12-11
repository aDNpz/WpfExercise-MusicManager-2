using Repository.Logic.Models;
using Repository.Logic.Repos;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MusicManager.WpfApp.ViewModels
{
    public class TrackEditViewModel : BaseViewModel
    {
        private ICommand? saveCommand = null;
        private ICommand? closeCommand = null;
        private Track track = new();

        public Action? CloseAction { get; set; }

        public Track Track
        {
            get => track; set
            {
                track = value;
                OnPropertyChanged(nameof(Name));
                OnPropertyChanged(nameof(Composer));
            }
        }
        public string Name
        {
            get => Track.Name;
            set { Track.Name = value; }
        }

        public string Composer
        {
            get => Track.Composer;
            set { Track.Composer = value; }
        }

        public ICommand SaveCommand
        {
            get
            {
                return RelayCommand.CreateCommand(ref saveCommand, (p) => Save());
            }
        }

        private void Save()
        {
            var rep = new TrackRepository();

            if (Track.Id == 0)
            {
                rep.AddAsync(Track).Wait();
            }
            else
            {
                rep.UpdateAsync(Track).Wait();
            }

            rep.SaveChangesAsync().Wait();

            Close();
        }

        public ICommand CloseCommand
        {
            get
            {
                return RelayCommand.CreateCommand(ref closeCommand, (p) => Close());
            }



        }

        private void Close()
        {
            CloseAction?.Invoke();
        }
    }
}
