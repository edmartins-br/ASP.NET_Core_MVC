using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SalesWebMvc.Models;
using System.Linq;
using SalesWebMvc.Data;

namespace SalesWebMvc.Services
{
    public class DepartmentService
    {
        // readOnly para garantir que esta dependencia não possa ser alterada
        private readonly SalesWebMvcContext _context;

        public DepartmentService(SalesWebMvcContext context)
        {
            _context = context;
        }

        public List<Department> FindAll()
        {
            return _context.Department.OrderBy(x => x.Name).ToList(); // order by vem do LINQ
        }
    }
}
