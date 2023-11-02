using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CourseRegisterApplication_SE214.Models;

namespace CourseRegisterApplication_SE214.Services
{
	public interface ILoginServices
	{
		Task<User> LoginUser(string username, string password);
	}
}
