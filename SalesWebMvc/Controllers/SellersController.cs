using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SalesWebMvc.Models;
using SalesWebMvc.Services;
using SalesWebMvc.Models.ViewModels;
using SalesWebMvc.Services.Exceptions;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;

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
         
        public async Task<IActionResult> Index()
        {
            var list = await _sellerService.FindAllAsync(); // retorna uma lista de seller
            return View(list); // passa a lista como argumento no metodo View
        }

        public async Task<IActionResult> Create()
        {
            var departments = await _departmentService.FindAllAsync();
            var viewModel = new SellerFormViewModel { Departments = departments };
            return View(viewModel); // ja vai receber este objeto com os departamentos populados
        }

        [HttpPost] // indica que é um método POST e não um étodo GET
        [ValidateAntiForgeryToken] // previnir que a aplicação sofra ataque CSRF - quando alguem aproveita a sua sessao de autenticação e envia adados maliciosos aproveitando sua autenticação
        public async Task<IActionResult> Create(Seller seller)
        {
            // ete if serve para impedir a requisição ao servidor caso o JavScript esteja desabilitado e não faça a verificação
            if (!ModelState.IsValid)
            {
                var departments = await _departmentService.FindAllAsync();
                var viewModel = new SellerFormViewModel { Seller = seller, Departments = departments };
                return View(viewModel);
            }
            await _sellerService.InsertAsync(seller);
            return RedirectToAction(nameof(Index)); // usa Name Of pois se mudar a string da ação no metodo de cima, não precisa mudar nada aqui
        }

        public async Task<IActionResult> Delete(int? id) // int? significa que é opcional
        {
            if(id == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not provided!"});
            }

            var obj = await _sellerService.FindByIdAsync(id.Value);
            if (obj == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not found!" });
            }
            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _sellerService.RemoveAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch (IntegrityEcpetion)
            {
                return RedirectToAction(nameof(Error), new { message = "You can not delete the Seller because he/she has sales" });
            }
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not provided!" });
            }

            var obj = await _sellerService.FindByIdAsync(id.Value);
            if (obj == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not found!" });
            }
            return View(obj);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if(id == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not provided!" });
            }
            var obj = await _sellerService.FindByIdAsync(id.Value);
            if(obj == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not found!" });
            }

            List<Department> departments = await _departmentService.FindAllAsync();
            SellerFormViewModel viewModel = new SellerFormViewModel { Seller = obj, Departments = departments };

            return View(viewModel);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Edit(int id, Seller seller)
        {
            // ete if serve para impedir a requisição ao servidor caso o JavScript esteja desabilitado e não faça a verificação
            if (!ModelState.IsValid)
            {
                var departments = await _departmentService.FindAllAsync();
                var viewModel = new SellerFormViewModel { Seller = seller, Departments = departments };
                return View(viewModel);
            }

            if (id != seller.Id)
            {
                return RedirectToAction(nameof(Error), new { message = "Id mismatch!" });
            }

            try
            {
                await _sellerService.UpdateAsync(seller);
                return RedirectToAction(nameof(Index));
            }
            catch (ApplicationException e)
            {
                return RedirectToAction(nameof(Error), new { message = e.Message });
            }            
                               
        }

        public IActionResult Error(string message) // nao precisa ser assincrona pois nao tem nenhum acesso a dados
        {
            var viewModel = new ErrorViewModel
            {
                Message = message,
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier //macete para pegar o Id interno da requisição
            };

            return View(viewModel);
        }
    }
}