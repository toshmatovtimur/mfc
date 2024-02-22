using System.Collections.Generic;

namespace mfc_tomsk.Models
{
    public partial class Admin
    {
        public Admin()
        {
            Users = new HashSet<User>();
        }

        public long Id { get; set; }
        public string? Firstname { get; set; }
        public string? Name { get; set; }
        public string? Lastname { get; set; }
        public long? Number { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}
