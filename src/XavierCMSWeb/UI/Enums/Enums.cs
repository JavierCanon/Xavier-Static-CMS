// GNU Version 3 License Copyright (c) 2020 Javier Cañon | https://javiercanon.com
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to
// deal in the Software without restriction, including without limitation the
// rights to use, copy, modify, merge, publish, distribute, sublicense, and/or
// sell copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS
// IN THE SOFTWARE.
namespace XavierCMSWeb.UI.Enums
{

	/// <summary>
	/// Meta Robots tag
	/// This tag helps you to specify the way your website will be crawled by the search engine. There are 4 types of Meta Robots Tag:
	/// Index, Follow - The search engine robots will start crawling your website from the main/index page and then will continue to the rest of the pages.
	/// Index, NoFollow - The search engine robots will start crawling your website from the main/index page and then will NOT continue to the rest of the pages.
	/// NoIndex, Follow - The search engine robots will skip the main/index page, but will crawl the rest of the pages.
	/// NoIndex, NoFollow - None of your pages will be crawled by the robot and your website will not be indexes by the search engines.
	/// </summary>
	public enum MetaRobots : int
	{
		Index_Follow = 1
		,
		Index_NoFollow = 2
		,
		NoIndex_Follow = 3
		,
		NoIndex_NoFollow = 4
	}

	public enum ContentType : int
	{
		Blog = 5
		,
		Faq = 8,
		Form = 3
		,
		Forum = 6
		,
		FrontPage = 1
		,
		LandingPage = 7
		,
		News = 4
		,
		Page = 2
	}


	public enum PageStatus : int
	{
		Draft = 1
		,
		NotPublished = 3
		,
		Published = 2
		,
		ToDelete = 4
	}

	/// <summary>
	/// locations for render controls in a page
	/// </summary>
	public enum ContentPlaces : int
	{
		ContentPlaceHolderBottom = 23,
		ContentPlaceHolderContent = 12
		,
		ContentPlaceHolderContentBottom = 13
		,
		ContentPlaceHolderContentTop = 11
		,
		ContentPlaceHolderFooter = 21
		,
		ContentPlaceHolderFooterBottom = 22
		,
		ContentPlaceHolderFooterTop = 	20
		,
		ContentPlaceHolderHead = 1
		,
		ContentPlaceHolderHeader = 6
		,
		ContentPlaceHolderHeaderBottom = 7
		,
		ContentPlaceHolderHeaderTop = 5
		,
		ContentPlaceHolderHeadStyles = 2
		,
		ContentPlaceHolderLogo = 4
		,
		ContentPlaceHolderMenu = 9
		,
		ContentPlaceHolderMenuBottom = 10
		,
		ContentPlaceHolderMenuTop = 8
		,
		ContentPlaceHolderSideBarLeft = 18
		,
		ContentPlaceHolderSideBarLeftBottom = 19
		,
		ContentPlaceHolderSideBarLeftTop = 17
		,
		ContentPlaceHolderSideBarRight = 15
		,
		ContentPlaceHolderSideBarRightBottom = 16
		,
		ContentPlaceHolderSideBarRightTop = 14
		,
		ContentPlaceHolderTop = 3
	}
}
