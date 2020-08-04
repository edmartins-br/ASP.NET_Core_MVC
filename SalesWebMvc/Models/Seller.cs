using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace SalesWebMvc.Models
{
    public class Seller
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "{0} required!")]
        [StringLength(60, MinimumLength = 3, ErrorMessage = "{0} should be between {2} and {1} characters!")]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "{0} required!")]
        [Display(Name = "E-mail")]
        [DataType(DataType.EmailAddress)] // coloca o Mailto nos emails
        [EmailAddress(ErrorMessage = "Enter a valid E-mail address!")]
        public string Email { get; set; }

        [Required(ErrorMessage = "{0} required!")]
        [Display(Name="Birth Date")] // mostra o que está nesta TAG e não o nome da classe
        [DataType(DataType.Date)] // Elimina hora e minuto do modelo padrão e deixa somente a data
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime BirthDate { get; set; }

        [Required(ErrorMessage = "{0} required!")]
        [Range(100.0, 50000.0, ErrorMessage = "{0} must be from {1} to {2}")]
        [DisplayFormat(DataFormatString = "{0:F2}")] // formata o valor com duas casas decimais
        [Display(Name = "Base Salary")]
        public double BaseSalary { get; set; }

        // Seller possui um departamento
        public Department Department { get; set; }
        public int DepartmentId { get; set; }

        // Associação do vendedor com as vendas
        public ICollection<SalesRecord> Sales { get; set; } = new List<SalesRecord>();

        public Seller()
        {

        }

        public Seller(int id, string name, string email, DateTime birthDate, double baseSalary, Department department)
        {
            Id = id;
            Name = name;
            Email = email;
            BirthDate = birthDate;
            BaseSalary = baseSalary;
            Department = department;
        }

        public void AddSales(SalesRecord sr)
        {
            Sales.Add(sr);
        }

        public void RemoveSales(SalesRecord sr)
        {
            Sales.Remove(sr);
        }

        // double pois vai retornar o total de vendas
        public double TotalSales(DateTime initial, DateTime final)
        {
            // utilizando o LINQ
            return Sales.Where(sr => sr.Date >= initial && sr.Date <= final).Sum(SalesRecord => SalesRecord.Amount);
        }
    }
}
