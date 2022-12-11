using Repository.Logic.Models;

namespace Repository.Logic.Repos
{
    public class AlbumRepository : Repository<Models.Album>
    {
        public AlbumRepository()
            : base()
        {

        }
        public override Album Create()
        {
            return new Album();
        }

        public override Task DeleteAsync(int id)
        {
            var trackRepo = new TrackRepository();
            var delTracks = trackRepo.GetAllAsync().Result.Where(x => x.AlbumId == id);

            foreach (var item in delTracks)
            {
                trackRepo.DeleteAsync(item.Id).Wait();
            }
            trackRepo.SaveChangesAsync().Wait();

            return base.DeleteAsync(id);
        }
    }
}
