using System.ComponentModel.DataAnnotations;

namespace GalaxyGuesserApi.Models.DTO
{
    public class UpdatePlayerRequest
    {
        [Required]
        public string UserName { get; set; } = string.Empty;
    }

} 