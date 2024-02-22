using System.Collections.Generic;

namespace mfc_tomsk.Models
{
    public partial class Position
    {
        public Position()
        {
            Users = new HashSet<User>();
        }

        public long Id { get; set; }
        public string? PositionName { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}
