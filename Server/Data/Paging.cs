using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace LabApp.Server.Data
{
    public class Paging
    {
        private static readonly int DefaultPageSize = Int32.MaxValue;
        private int _page;
        private int _pageSize;
        
        public Paging()
        {
            Page = 1;
            PageSize = DefaultPageSize;
        }
        
        public Paging(int pageSize)
        {
            Page = 1;
            PageSize = pageSize;
        }
        
        public Paging(int page, int pageSize)
        {
            Page = page;
            PageSize = pageSize;
        }

        [FromQuery]
        [BindProperty(Name = "page", SupportsGet = true)]
        public int Page
        {
            get => _page;
            set => _page = value <= 0 ? 1 : value;
        }

        [FromQuery]
        [BindProperty(Name = "pageSize", SupportsGet = true)]
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value <= 0 ? DefaultPageSize : value;
        }

        [BindNever]
        public int Take => PageSize;

        [BindNever]
        public int Skip => (Page - 1) * PageSize;
    }
}