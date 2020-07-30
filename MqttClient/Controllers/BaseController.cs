using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using MqttClient.Models;

namespace MqttClient.Controllers
{
    public class BaseController : Controller
    {
        protected IEnumerable<T> Pagination<T>(IEnumerable<T> source, PaginationParameter pagination = null)
        {
            if (pagination == null)
            {
                pagination = new PaginationParameter();
            }

            int totalCount = source.Count();
            int currentPage = pagination.pageNumber;
            int pageSize = pagination.pageSize;
            int totalPages = (int) Math.Ceiling(totalCount / (double) pageSize);

            var items = source.Skip((currentPage - 1) * pageSize).Take(pageSize).ToArray();

            Response.Headers["X-Pagination-Current-Page"] = currentPage.ToString();
            Response.Headers["X-Pagination-Page-Count"] = totalPages.ToString();
            Response.Headers["X-Pagination-Per-Page"] = pageSize.ToString();
            Response.Headers["X-Pagination-Total-Count"] = totalCount.ToString();
            Response.Headers["Access-Control-Expose-Headers"] = string.Join(
                ", ",
                new List<string>
                {
                    "X-Pagination-Current-Page",
                    "X-Pagination-Per-Page",
                    "X-Pagination-Page-Count",
                    "X-Pagination-Total-Count"
                });

            return items;
        }
    }
}
