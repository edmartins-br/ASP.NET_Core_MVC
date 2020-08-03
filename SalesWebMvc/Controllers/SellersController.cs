using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SalesWebMvc.Models;
using SalesWebMvc.Services;
using SalesWebMvc.Models.ViewModels;

namespace SalesWebMvc.Controllers
{
    public class SellersController : Controller
    {
        // declarar dependencia para o SellerService - READ ONLY PREVINE QUE A DEPENDENCIA SEJA ALTARADA
        private readonly SellerService _sellerService;
        private readonly DepartmentService _departmentService;

        public SellersController(SellerService sellerService, DepartmentService departmentService)
        {
            _sellerService = sellerService;
            _departmentService = departmentService;
        }        
         
        public IActionResult Index()
        {
            var list = _sellerService.FindAll(); // retorna uma lista de seller
            return View(list); // passa a lista como argumento no metodo View
        }

        public IActionResult Create()
        {
            var departments = _departmentService.FindAll();
            var viewModel = new SellerFormViewModel { Departments = departments };
            return View(viewModel); // ja vai receber este objeto com os departamentos populados
        }

        [HttpPost] // indica que é um método POST e não um étodo GET
        [ValidateAntiForgeryToken] // previnir que a aplicação sofra ataque CSRF - quando alguem aproveita a sua sessao de autenticação e envia adados maliciosos aproveitando sua autenticação
        public IActionResult Create(Seller seller)
        {
            _sellerService.Insert(seller);
            return RedirectToAction(nameof(Index)); // usa Name Of pois se mudar a string da ação no metodo de cima, não precisa mudar nada aqui
        }

        public IActionResult Delete(int? id) // int? significa que é opcional
        {
            if(id == null)
            {
                return NotFound();
            }

            var obj = _sellerService.FindById(id.Value);
            if (obj == null)
            {
                return NotFound();
            }
            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            _sellerService.Remove(id);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var obj = _sellerService.FindById(id.Value);
            if (obj == null)
            {
                return NotFound();
            }
            return View(obj);
        }
    }
}