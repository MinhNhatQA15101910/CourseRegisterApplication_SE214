<<<<<<< HEAD
﻿namespace CourseRegisterApplication.Shared
=======
﻿using System.ComponentModel.DataAnnotations.Schema;

namespace CourseRegisterApplication.Shared
>>>>>>> a49543f85fa7dd442c5bd1c935369537482f6a13
{
    public class User
    {
        public int Id { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? Email { get; set; }

        [ForeignKey("Role")]
        public int RoleId { get; set; }
        public Role Role { get; set; } = null!;
    }
}
