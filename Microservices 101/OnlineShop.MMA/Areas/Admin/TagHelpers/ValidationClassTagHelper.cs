using System.Linq;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace OnlineShop.MMA.Areas.Admin.TagHelpers
{
    [HtmlTargetElement("*", Attributes = ValidationForAttributeName + "," + ValidationErrorClassName + "," + AddErrorMessageFlag)]
    public class ValidationClassTagHelper : TagHelper
    {
        private const string ValidationForAttributeName = "pf-validation-for";
        private const string ValidationErrorClassName = "pf-validationerror-class";
        private const string AddErrorMessageFlag = "pf-adderrormessage-flag";

        [HtmlAttributeName(ValidationForAttributeName)]
        public ModelExpression For { get; set; }

        [HtmlAttributeName(ValidationErrorClassName)]
        public string ValidationErrorClass { get; set; }

        [HtmlAttributeName(AddErrorMessageFlag)]
        public bool ErrorMessageFlag { get; set; }


        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            ModelStateEntry entry;

            ViewContext.ViewData.ModelState.TryGetValue(For.Name, out entry);
            if (entry == null || !entry.Errors.Any())
            {
                return;
            }

            var tagBuilder = new TagBuilder("*");
            tagBuilder.AddCssClass(ValidationErrorClass);
            output.MergeAttributes(tagBuilder);

            if (ErrorMessageFlag == true)
            {
                var errorMessage = entry.Errors.FirstOrDefault().ErrorMessage;
                output.Content.SetContent(errorMessage);
            }
        }
    }
}
