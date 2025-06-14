using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Models;

namespace Application.Services.Interfaces
{
    public interface IAuthService
    {
        public UserModel TryLogin(string username, string password);
        public bool Registration(string username, string password);

    }
}
