using System.ComponentModel.DataAnnotations;

namespace GLMS.Models
{
    // Represents a client/customer in the system
    public class Client
    {
        public int Id { get; set; } 

        [Required(ErrorMessage = "Client name is required")]
        public string Name { get; set; } = ""; 

        [Required(ErrorMessage = "Contact details are required")]
        public string ContactDetails { get; set; } = ""; 

        [Required(ErrorMessage = "Region is required")]
        public string Region { get; set; } = ""; 

        public ICollection<Contract> Contracts { get; set; } = new List<Contract>(); 
        public ICollection<ServiceRequest> ServiceRequests { get; set; } = new List<ServiceRequest>(); 
    }
}