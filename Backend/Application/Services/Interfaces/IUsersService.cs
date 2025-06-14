using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Models;

namespace Application.Services.Interfaces
{
    public interface IUsersService
    {
        public IEnumerable<UserModel> GetAll();
        public UserModel GetById(int id);
        public void Update(UserModel user);
        public void Delete(int id);
    }
}
