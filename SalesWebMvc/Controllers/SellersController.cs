using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SalesWebMvc.Models;
using SalesWebMvc.Services;

namespace SalesWebMvc.Controllers
{
    public class SellersController : Controller
    {
        // declarar dependencia para o SellerService - READ ONLY PREVINE QUE A DEPENDENCIA SEJA ALTARADA
        private readonly SellerService _sellerService;

        public SellersController(SellerService sellerService)
        {
            _sellerService = sellerService;
        }        
         
        public IActionResult Index()
        {
            var list = _sellerService.FindAll(); // retorna uma lista de seller
            return View(list); // passa a lista como argumento no metodo View
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost] // indica que é um método POST e não um étodo GET
        [ValidateAntiForgeryToken] // previnir que a aplicação sofra ataque CSRF - quando alguem aproveita a sua sessao de autenticação e envia adados maliciosos aproveitando sua autenticação
        public IActionResult Create(Seller seller)
        {
            _sellerService.Insert(seller);
            return RedirectToAction(nameof(Index)); // usa Name Of pois se mudar a string da ação no metodo de cima, não precisa mudar nada aqui
        }
    }
}