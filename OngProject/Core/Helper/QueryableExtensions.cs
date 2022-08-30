using System.Linq;
using OngProject.Core.Models.DTOs;

namespace OngProject.Core.Helper
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> Paginar<T>(this IQueryable<T> queryable, PaginacionDto paginacionDto)
        {
            return queryable
                .Skip((paginacionDto.Pagina - 1) * paginacionDto.CantidadRegistroPorPagina)
                .Take(paginacionDto.CantidadRegistroPorPagina);
        }
    }
}