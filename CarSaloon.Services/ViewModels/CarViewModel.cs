using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarSaloon.Services.ViewModels
{
    public class CarViewModel:BaseViewModel
    {
        [Required]
        public string CarModel { get; set; }
        [Required]
        public string Manufacturer { get; set; }
        [Required]
        public string Category { get; set; }
        [Required]
        public string Engine { get; set; }
        [Required]
        public double Length { get; set; }
        [Required]
        public double Width { get; set; }
        [Required]
        public string Color { get; set; }
        public string ImageString { get; set; } = null;
        
    }
}
