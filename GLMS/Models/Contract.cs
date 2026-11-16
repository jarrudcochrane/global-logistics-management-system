using System.ComponentModel.DataAnnotations;

namespace GLMS.Models
{
    // Represents a contract agreement between the company and a client
    public class Contract
    {
        public int Id { get; set; } 

        [Required(ErrorMessage = "Please select a client")]
        public int ClientId { get; set; } 
        public Client? Client { get; set; } 

        [Required(ErrorMessage = "Start date is required")]
        public DateTime? StartDate { get; set; }  

        [Required(ErrorMessage = "End date is required")]
        public DateTime? EndDate { get; set; }    

        [Required(ErrorMessage = "Status is required")]
        public string Status { get; set; } = "Draft";  

        [Required(ErrorMessage = "Service level is required")]
        public string ServiceLevel { get; set; } = ""; 

        public string? SignedAgreementPath { get; set; } 

        public ICollection<ServiceRequest> ServiceRequests { get; set; } = new List<ServiceRequest>();
    }
}