using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TutoricaFront.Models
{
    public class Item
    {
        public int id {get; set;}  
        public string? firstName {get; set;} 
        public string? lastName {get; set;} 
        public string? name {get; set;} 
        public string? phoneNumber {get; set;} 
        public string? email {get; set;} 
        public string? description {get; set;} 
        public int years {get; set;} 
        public float investmentUSD {get; set;} 
        public float course {get; set;} 
        public float investmentUAH {get; set;} 
        
    }
}