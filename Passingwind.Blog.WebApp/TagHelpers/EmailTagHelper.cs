using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Threading.Tasks;

namespace Passingwind.Blog.WebApp.TagHelpers
{
    public class EmailTagHelper : TagHelper
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "a";    // Replaces <email> with <a> tag
        }
    }
}
