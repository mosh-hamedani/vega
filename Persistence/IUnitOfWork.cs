using System;
using System.Threading.Tasks;

namespace vega.Persistence
{
  public interface IUnitOfWork
  {
    Task CompleteAsync();
  }
}