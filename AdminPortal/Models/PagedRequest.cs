// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace AdminPortal.Models
{
    public class PagedRequest
    {
        private const int DefaultPageSize = 15;
        private const int MaxPageSize = 500;
        private int _pageNumber = 1;
        private int _pageSize = DefaultPageSize;

        public int PageNumber
        {
            get => _pageNumber;
            set => _pageNumber = value < 1 ? 1 : value;
        }
        public int PageSize
        {
            get => _pageSize;
            set
            {
                if (value < 1)
                {
                    _pageSize = DefaultPageSize;
                }
                else if (value > MaxPageSize)
                {
                    _pageSize = MaxPageSize;
                }
                else
                {
                    _pageSize = value;
                }
            }
        }
    }
}
