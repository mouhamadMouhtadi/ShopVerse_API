﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace ShopVerse.Core.Specifications.ProductsSpecificaiton
{
    public class ProductSpecParams
    {
        private string? search;
        public string? Search
        {
            get => search;
            set => search = value?.ToLower();
        }
        public string? sort { get; set; }
        public int? brandId { get; set; }
        public int? typeId { get; set; }
        public int pageSize { get; set; } = 5;
        public int pageIndex { get; set; } = 1;

    }
}
