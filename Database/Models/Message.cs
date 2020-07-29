using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Database.Models
{
    public class Message
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [Column("topic")]
        [JsonPropertyName("topic")]
        public string Topic { get; set; }

        [Column("payload")]
        [JsonPropertyName("payload")]
        public string Payload { get; set; }

        [Column("qos")]
        [JsonPropertyName("qos")]
        public uint Qos { get; set; }

        [Column("retain")]
        [JsonPropertyName("retain")]
        public bool Retain { get; set; }
    }
}
