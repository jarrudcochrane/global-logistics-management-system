using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GLMS.Models
{
    // Represents a service request made by a client under their contract
    public class ServiceRequest
    {
        public int Id { get; set; } 

        [Required(ErrorMessage = "Please select a client.")]
        public int ClientId { get; set; } 
        public Client? Client { get; set; } 

        [Required(ErrorMessage = "Please select a contract.")]
        public int ContractId { get; set; } 
        public Contract? Contract { get; set; } 

        [NotMapped]
        [Required(ErrorMessage = "Please select a contract type.")]
        public string ContractType { get; set; } = "";

        [Required(ErrorMessage = "Please enter a description.")]
        public string Description { get; set; } = ""; 

        public decimal CostUSD { get; set; } 
        public decimal CostZAR { get; set; } 

        [Required(ErrorMessage = "Please select a status.")]
        public string Status { get; set; } = "Pending"; 
    }
}