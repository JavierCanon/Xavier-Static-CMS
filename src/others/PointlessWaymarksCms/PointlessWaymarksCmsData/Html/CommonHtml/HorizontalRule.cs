﻿using HtmlTags;

namespace PointlessWaymarksCmsData.Html.CommonHtml
{
    public static class HorizontalRule
    {
        public static HtmlTag StandardRule()
        {
            return new HtmlTag("hr").AddClass("standard-rule");
        }

        public static HtmlTag StandardRuleIfNotEmptyTag(HtmlTag tag)
        {
            return string.IsNullOrWhiteSpace(tag.ToString())
                ? HtmlTag.Empty()
                : new HtmlTag("hr").AddClass("standard-rule");
        }
    }
}