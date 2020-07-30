using System.Collections.Generic; // para o ICollection
using System.Linq;
using System;


namespace SalesWebMvc.Models
    
{
    public class Department
    {
        public int Id { get; set; }
        public string Name { get; set; }
        // Um departamento tem vários vendedores - Implementado a associação do departamento com o Seller
        public ICollection<Seller> Sellers { get; set; } = new List<Seller>(); // instancia a coleção para garantir que a lista seja instanciada

        public Department()
        {

        }

        public Department(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public void AddSeller(Seller seller)
        {
            Sellers.Add(seller);
        }

        public double TotalSales(DateTime initial, DateTime final)
        {
            // Chama a lista Sellers para pegar a lsita de vendedores deste departamento
            // chama a funçao soma para apenas as vendas neste periodo de data
            // Pega cada vendedor da lista, chama o total Sales de cada vendedor, e depois
            // faz uma soma pra todos os vendedores do departamento
            return Sellers.Sum(seller => seller.TotalSales(initial, final));
        }
    }
}
