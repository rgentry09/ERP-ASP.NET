﻿using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Threading.Tasks;

namespace GridMvc.TagHelpers
{
    public class GridTagHelper : TagHelper
    {
        private readonly IHtmlHelper _html;

        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }
        public ISGrid Model { get; set; }

        public GridTagHelper(IHtmlHelper html)
        {
            _html = html;
        }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = null;
            (_html as IViewContextAware).Contextualize(ViewContext);
            var content = await _html.PartialAsync("_Grid", Model);
            output.Content.SetHtmlContent(content);
        }
    }
}
