using Repository.Logic.Models;

namespace Repository.Logic.Repos
{
    public class TrackRepository : Repository<Models.Track>
    {
        public TrackRepository()
            : base()
        {

        }
        public override Track Create()
        {
            return new Track();
        }
    }
}
