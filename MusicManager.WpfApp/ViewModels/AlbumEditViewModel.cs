using Repository.Logic.Models;
using Repository.Logic.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MusicManager.WpfApp.ViewModels
{
    public class AlbumEditViewModel : BaseViewModel
    {
        private ICommand? saveCommand = null;
        private ICommand? closeCommand = null;
        private Album album = new();

        public Action? CloseAction { get; set; }

        public Album Album
        {
            get => album; set
            {
                album = value;
                OnPropertyChanged(nameof(Title));
                OnPropertyChanged(nameof(Interpreter));
            }
        }
        public string Title
        {
            get => Album.Title;
            set { Album.Title = value; }
        }

        public string Interpreter
        {
            get => Album.Interpreter;
            set { Album.Interpreter = value; }
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
            var rep = new AlbumRepository();

            if (Album.Id == 0)
            {
                rep.AddAsync(Album).Wait();
            }
            else
            {
                rep.UpdateAsync(Album).Wait();
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
