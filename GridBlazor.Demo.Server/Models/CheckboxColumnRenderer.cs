﻿using GridMvc;
using GridShared.Columns;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.IO;
using System.Text.Encodings.Web;

namespace GridBlazor.Demo.Server.Models
{
    public class CheckboxColumnRenderer : GridHeaderRenderer
    {
        private readonly HtmlHelper _helper;

        public CheckboxColumnRenderer(HtmlHelper helper)
        {
            _helper = helper;
        }

        protected override string RenderAdditionalContent(IGridColumn column)
        {
            using (var sw = new StringWriter())
            {
                _helper.CheckBox("check-all", false, new { @class = "check-all" }).WriteTo(sw, HtmlEncoder.Default);
                return sw.ToString();
            }
        }
    }
}