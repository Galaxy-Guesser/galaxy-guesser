
using System.ComponentModel.DataAnnotations;

namespace GalaxyGuesserApi.Models.DTO
   {
    public class CreatePlayerRequest
    {
        [Required]
        public string Guid { get; set; } = string.Empty;
        [Required]
        public string UserName { get; set; } = string.Empty;
    }
   }