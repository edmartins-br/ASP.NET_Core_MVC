using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SalesWebMvc.Models;
using SalesWebMvc.Data;
using Microsoft.EntityFrameworkCore;

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

        // Transformar o acesso a banco de dados de Sincrono para assincrono desta forma a aplicação não precisa
        // esperar a tarefa ser concluida para continuar rodando a aplicação. O acesso é feito enquanto a aplicação está
        // rodando, por meio de Taks.
        public async Task<List<Department>> FindAllAsync()
        {
            return await _context.Department.OrderBy(x => x.Name).ToListAsync(); // order by vem do LINQ
            // é preciso incluir a palavra AWAIT na frente para que o compilador entenda que é uma função assincrona, retornando um task
            // de link department, dessa forma a execução na bloqueia a aplicação.
        }
    }
}
