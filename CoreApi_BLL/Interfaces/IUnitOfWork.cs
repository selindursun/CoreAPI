using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreApi_BLL.Interfaces
{
    public interface IUnitOfWork:IDisposable
    {
        //Kaç tablo varsa o kadar repository olacak burada.
        IAssignmentRepository AssignmentRepository { get; }
    }
}
