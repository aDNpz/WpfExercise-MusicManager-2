using Repository.Logic.Models;
using Repository.Logic.Repos;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace MusicManager.WpfApp.ViewModels
{
    public class MainViewModel : BaseViewModel
    {

        private ICommand? addAlbumCommand = null;
        private ICommand? editAlbumCommand = null;
        private ICommand? deleteAlbumCommand = null;

        private ICommand? addTrackCommand = null;
        private ICommand? editTrackCommand = null;
        private ICommand? deleteTrackCommand = null;
        public ObservableCollection<Album> Albums
        {
            get
            {
                var albumRep = new AlbumRepository();
                var albums = string.IsNullOrWhiteSpace(searchAlbum) ? albumRep.GetAllAsync().Result.OrderBy(a => a.Title) : albumRep.GetAllAsync().Result.Where(x => x.Interpreter.ToUpper().Contains(searchAlbum.ToUpper()) || x.Title.ToUpper().Contains(searchAlbum.ToUpper())).OrderBy(x => x.Title);
                return new ObservableCollection<Album>(albums);
            }
        }

        public ObservableCollection<Track> Tracks
        {
            get
            {
                var trackRepo = new TrackRepository();
                var tracks = selectedAlbum != null ? trackRepo.GetAllAsync().Result.Where(x => x.AlbumId == SelectedAlbum.Id) : Array.Empty<Track>();
                return new ObservableCollection<Track>(tracks);
            }
        }

        public ICommand EditTrackCommand => RelayCommand.CreateCommand(ref editTrackCommand, (p) => EditTrack(), (p) => SelectedAlbum != null && SelectedTrack != null);
        private void EditTrack()
        {
            var trackEditWindow = new TrackEditWindow();

            if(trackEditWindow.ViewModel is TrackEditViewModel tevm &&  SelectedTrack != null)
            {
                tevm.Track = SelectedTrack!;
                trackEditWindow.ShowDialog();
                OnPropertyChanged(nameof(Tracks));
            }
            
        }

        public ICommand DeleteTrackCommand => RelayCommand.CreateCommand(ref deleteTrackCommand, (p) => DeleteTrack(), (p) => SelectedAlbum != null && SelectedTrack != null);

        private void DeleteTrack()
        {
            if (SelectedAlbum != null && SelectedTrack != null)
            {

                if (MessageBox.Show($"Delete item {SelectedTrack}?", "Delete", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    var repo = new TrackRepository();

                    repo.DeleteAsync(SelectedTrack.Id).Wait();
                    repo.SaveChangesAsync().Wait();
                    OnPropertyChanged(nameof(Tracks));
                }
            }
        }

        public ICommand AddTrackCommand => RelayCommand.CreateCommand(ref addTrackCommand, (p) => AddTrack(), (p) => SelectedAlbum != null);

        private void AddTrack()
        {
            if (SelectedAlbum != null)
            {
                var trackEditWindow = new TrackEditWindow();

                if (trackEditWindow.ViewModel is TrackEditViewModel tevm)
                {
                    tevm.Track = new Track()
                    {
                        AlbumId = SelectedAlbum.Id,
                    };
                }
                trackEditWindow.ShowDialog();
                OnPropertyChanged(nameof(Tracks));
            }
        }

        public ICommand AddAlbumCommand
        {
            get
            {
                return RelayCommand.CreateCommand(ref addAlbumCommand, (p) => AddAlbum());
            }
        }

        private void AddAlbum()
        {
            var albumEditWindow = new AlbumEditWindow();
            albumEditWindow.ShowDialog();
            OnPropertyChanged(nameof(Albums));
        }

        public ICommand EditAlbumCommand
        {
            get
            {
                return RelayCommand.CreateCommand(ref editAlbumCommand, (p) => EditAlbum(), (p) => SelectedAlbum != null);
            }

        }

        private void EditAlbum()
        {
            var albumEditWindow = new AlbumEditWindow();
            if (albumEditWindow.ViewModel is AlbumEditViewModel aevm && SelectedAlbum != null)
            {
                aevm.Album = SelectedAlbum;
                albumEditWindow.ShowDialog();
            }
            OnPropertyChanged(nameof(Albums));

        }

        public ICommand DeleteAlbumCommand
        {
            get
            {
                return RelayCommand.CreateCommand(ref deleteAlbumCommand, (p) => DeleteAlbum(), (p) => SelectedAlbum != null);
            }
        }

        private void DeleteAlbum()
        {
            if (SelectedAlbum != null)
            {
                var response = MessageBox.Show($"Soll dieses Item wirklich gelöscht werden {SelectedAlbum.Title} {SelectedAlbum.Interpreter}", "Album löschen", MessageBoxButton.YesNo, MessageBoxImage.Stop);

                if (response == MessageBoxResult.Yes)
                {
                    var repo = new AlbumRepository();
                    repo.DeleteAsync(SelectedAlbum.Id).Wait();
                    repo.SaveChangesAsync().Wait();
                    OnPropertyChanged(nameof(Albums));
                }
            }

        }

        private Album? selectedAlbum = null;
        private string? searchAlbum;

        public Album? SelectedAlbum
        {
            get => selectedAlbum;
            set
            {
                selectedAlbum = value;
                OnPropertyChanged(nameof(Tracks));
            }
        }

        public Track? SelectedTrack { get; set; } = null;

        public string? SearchAlbum
        {
            get => searchAlbum; set
            {
                searchAlbum = value;
                OnPropertyChanged(nameof(Albums));
            }
        }
    }
}
