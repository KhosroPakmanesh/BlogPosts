using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace OnlineShop.MMA.Areas.Admin.TagHelpers
{
    [HtmlTargetElement("*", Attributes = ClassApplyingCondition + "," + ClassApplyingClassName)]
    public class ConditionalClassApplyingTagHelper : TagHelper
    {
        private const string ClassApplyingCondition = "pf-classapplying-condition";
        private const string ClassApplyingClassName = "pf-classapplying-class";

        [HtmlAttributeName(ClassApplyingCondition)]
        public bool Condition { get; set; }

        [HtmlAttributeName(ClassApplyingClassName)]
        public string ClassName { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (Condition)
            {
                var tagBuilder = new TagBuilder("*");
                tagBuilder.AddCssClass(ClassName);
                output.MergeAttributes(tagBuilder);
            }
        }
    }
}
