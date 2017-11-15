using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using System;

namespace TodoList.Models
{
    public class Task
    {    
        [Key] 
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Comments { get; set; }

      
    }
}