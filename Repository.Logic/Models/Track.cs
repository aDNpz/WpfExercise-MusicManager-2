namespace Repository.Logic.Models
{
    public class Track : ModelObject, ICloneable
    {
        public int AlbumId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Composer { get; set; } = string.Empty;

        public object Clone()
        {
            return new Track
            {
                Id = Id,
                AlbumId = AlbumId,
                Name = Name,
                Composer = Composer,
            };
        }
    }
}
