using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Repository;
using Models.Models;

namespace Data.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Company> CompanyRepository { get; }
        IRepository<Storer> StorerRepository { get; }
    }
}
