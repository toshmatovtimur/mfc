using System;
using System.Collections.Generic;

namespace mfc_tomsk.Models
{
    public partial class User
    {
        public long Id { get; set; }
        public string? Firstname { get; set; }
        public string? Name { get; set; }
        public string? Lastname { get; set; }
        public string? Login { get; set; }
        public string? Password { get; set; }
        public string? PasswordFhraze { get; set; }
        public string? Snils { get; set; }
        public string? DateGets { get; set; }
        public long? IdAdmin { get; set; }
        public long? IdPosition { get; set; }
        public long? IdDepartment { get; set; }

        public virtual Admin? IdAdminNavigation { get; set; }
        public virtual Department? IdDepartmentNavigation { get; set; }
        public virtual Position? IdPositionNavigation { get; set; }
    }
}
