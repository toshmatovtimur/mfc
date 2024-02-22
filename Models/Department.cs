using System.Collections.Generic;

namespace mfc_tomsk.Models
{
    public partial class Department
    {
        public Department()
        {
            Users = new HashSet<User>();
        }

        public long Id { get; set; }
        public string? DepartmentName { get; set; }
        public string? Address { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}
