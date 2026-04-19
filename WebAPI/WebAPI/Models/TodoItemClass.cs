using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace WebAPI.Models
{
    public class TodoItemClass
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        //[JsonPropertyName("title")]
        [Required]
        public string Title { get; set; } = string.Empty;
        //[JsonPropertyName("isCompleted")]
        public bool IsDone { get; set; } = false;
    }
}
