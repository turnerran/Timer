using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models.Requests;

public class UrlTask
{
    [Required]
    [Range(0, int.MaxValue, ErrorMessage = "The field {0} must be greater than {1}.")]
    public int Hours { get; set; }
    [Range(0, int.MaxValue, ErrorMessage = "The field {0} must be greater than {1}.")]
    [Required]
    public int Minutes { get; set; }
    [Range(0, int.MaxValue, ErrorMessage = "The field {0} must be greater than {1}.")]
    [Required]
    public int Seconds { get; set; }
    [Required]
    [StringLength(2000, ErrorMessage = "The maximum url length is 2000")]
    public string Url { get; set; }
}
