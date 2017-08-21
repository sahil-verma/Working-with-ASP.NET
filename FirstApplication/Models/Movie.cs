namespace FirstApplication.Models
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Linq;

    public partial class Movie
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string MovieId { get; set; }

        [Required]
        [StringLength(250)]
        [Display(Name = "Movie Name")]
        public string Name { get; set; }

        [Display(Name= "HitMovie")]
        public bool HitMovie { get; set; }

        [Display(Name = "Create Date")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreateDate { get; set; }

        [Display(Name = "Edit Date")]
        public DateTime EditDate { get; set; } = DateTime.UtcNow;

        [Display(Name = "Actors")]
        [InverseProperty("Movie")]
        public virtual ICollection<MovieActor> Actors { get; set; } = new HashSet<MovieActor>();

        [Display(Name = "Ratings")]
        [InverseProperty("Movie")]
        public virtual ICollection<Rating> Ratings { get; set; } = new HashSet<Rating>();

        [NotMapped]
        public decimal OverallRating
        {
            get
            {
                if(Ratings.Count > 0)
                {
                    return (Ratings.Average(x => x.Rank));
                }

                return (9);
            }
        }

        public override string ToString()
        {
            return String.Format("{0}", Name);
        }
    }
}
