﻿namespace PingYourPackage.Domain.Entities.Extentions
{
    using System;
    using System.Linq;
    using PingYourPackage.Domain.Entities.Core;

    public static class IQueryableExtensions
    {
        public static PaginatedList<T> ToPaginatedList<T>(this IQueryable<T> query, int pageIndex, int pageSize)
        {
            var totalCount = query.Count();
            var collection = query.Skip((pageIndex - 1) * pageSize).Take(pageSize);
            return new PaginatedList<T>(pageIndex, pageSize, totalCount, collection);
        }
    }
}
