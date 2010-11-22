﻿//  Copyright 2004-2010 Castle Project - http://www.castleproject.org/
//  
//  Licensed under the Apache License, Version 2.0 (the "License");
//  you may not use this file except in compliance with the License.
//  You may obtain a copy of the License at
//  
//      http://www.apache.org/licenses/LICENSE-2.0
//  
//  Unless required by applicable law or agreed to in writing, software
//  distributed under the License is distributed on an "AS IS" BASIS,
//  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//  See the License for the specific language governing permissions and
//  limitations under the License.
// 
namespace Castle.MonoRail.ViewEngines.Razor
{
	using System;
	using System.Web.WebPages;

	public abstract class WebViewPage<TData> : WebPageBase, IViewPage
	{
		public TData Data { get; set; }

		//On razor, the view is the parent of the layout.
		protected override void ConfigurePage(WebPageBase parentPage)
		{
			var parent = parentPage as WebViewPage<TData>;

			if (parent == null)
				throw new Exception("View base type is invalid");

			Context = parent.Context;
			Data = parent.Data;
		}

		public void SetData(object data)
		{
			Data = (TData) data;
		}

		public object GetData()
		{
			return Data;
		}
	}

	public abstract class WebViewPage : WebViewPage<dynamic>
	{
	}
}