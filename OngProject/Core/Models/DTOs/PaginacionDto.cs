namespace OngProject.Core.Models.DTOs
{
    public class PaginacionDto
    {
        public int Pagina { get; set; } = 1;

        private int cantidadRegistroPorPagina { get; set; } = 10;
        private readonly int cantidadMaximaRegistroPorPagina = 50;
        
        public int CantidadRegistroPorPagina
        {
            get => cantidadRegistroPorPagina;

            set
            {
                cantidadRegistroPorPagina = (value > cantidadMaximaRegistroPorPagina) ? cantidadMaximaRegistroPorPagina : value;
            }
        }
    }
}