using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Passingwind.Blog.WebApp.Areas.Admin.Models
{
    public class TagsViewModel : BaseModel
    {
        [Required, MaxLength(100)]
        public string Name { get; set; }


        public int Count { get; set; }
    }
}
