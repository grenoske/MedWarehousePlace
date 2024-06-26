﻿using DAL.Entities;

namespace DAL.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        void Update(User obj);
    }
}
