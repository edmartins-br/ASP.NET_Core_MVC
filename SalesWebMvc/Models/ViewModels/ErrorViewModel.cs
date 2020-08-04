using System;

namespace SalesWebMvc.Models.ViewModels
{
    public class ErrorViewModel
    {
        // arquivo criado automaticamente ao criar o projeto
        public string RequestId { get; set; } // id interno da requisição
        public string Message { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}