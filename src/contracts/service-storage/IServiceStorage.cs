using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bust {
  interface IServiceStorage {
    Task<IEnumerable<Service>> FetchAll();
    Task<Service> FetchOne(string id);
  }
}
