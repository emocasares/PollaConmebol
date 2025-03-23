using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PollaEngendrilClientHosted.Shared.Models.Entity
{
    public class PreloadPointsPerUser
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int UserId { get; set; }
        public int InitialPoints { get; set; }

        [ForeignKey("UserId")]
        public User? User { get; set; } // Made nullable to fix CS8618
    }
}
