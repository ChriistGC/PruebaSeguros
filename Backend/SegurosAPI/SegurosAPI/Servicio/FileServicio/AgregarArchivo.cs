using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using SegurosAPI.Data;
using SegurosAPI.Modelo;
using SegurosAPI.Servicio.ManejadorError;
using System.Net;

namespace SegurosAPI.Servicio.FileServicio
{
    public class AgregarArchivo
    {
        public class AddFile : IRequest<Unit>
        {
            public IFormFile File { set; get; }
        }

        public class Manejador : IRequestHandler<AddFile, Unit>
        {
            private readonly ApplicationDbContext _context;
            private readonly IMapper _mapper;
            public Manejador(ApplicationDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }
            public async Task<Unit> Handle(AddFile request, CancellationToken cancellationToken)
            {
                if (request.File != null && request.File.Length > 0)
                {
                    // Verificar la extensión del archivo
                    var fileExtension = Path.GetExtension(request.File.FileName).ToLower();

                    if (fileExtension == ".xlsx")
                    {
                        return await processExcelFile(request.File);
                    }
                    else
                    {
                        throw new ManejadorException(HttpStatusCode.BadGateway, new { Archivo = "Extensión de archivo no válida." });
                    }
                }
                else
                {
                    throw new ManejadorException(HttpStatusCode.BadRequest, new { Archivo = "No se proporcionó ningún archivo" });
                }
            }


            private async Task<Unit> processExcelFile(IFormFile file)
            {
                var clientes = new List<Cliente>();
                using (var stream = new MemoryStream())
                {
                    file.CopyToAsync(stream);

                    using (var package = new ExcelPackage(file.OpenReadStream()))
                    {
                        ExcelPackage.LicenseContext = LicenseContext.Commercial;
                        var worksheet = package.Workbook.Worksheets.FirstOrDefault();

                        if (worksheet != null)
                        {
                            int rowCount = worksheet.Dimension?.Rows ?? 0;

                            //Comprueba si existen datos en las filas
                            if (rowCount <= 1)
                            {
                                throw new ManejadorException(HttpStatusCode.NotFound, new { Archivo = "El archivo no contiene valores" });
                            }

                            //
                            var columnHeaders = obtenerColumnasExcel(worksheet);
                            int nombreColumna = columnHeaders.IndexOf("nombre");
                            int edadColumna = columnHeaders.IndexOf("edad");
                            int cedulaColumna = columnHeaders.IndexOf("cedula");
                            int telefonoColumna = columnHeaders.IndexOf("telefono");

                            Console.WriteLine(edadColumna);

                            for (int row = 2; row <= rowCount; row++)
                            {
                                Console.WriteLine(rowCount);
                                string edadT = worksheet.Cells[row, edadColumna + 1]?.Value?.ToString();
                                if (int.TryParse(edadT, out int _edad))
                                {
                                    var clienteT = new Cliente
                                    {
                                        nombre = worksheet.Cells[row, nombreColumna + 1]?.Value?.ToString(),
                                        cedula = worksheet.Cells[row, cedulaColumna + 1]?.Value?.ToString(),
                                        telefono = worksheet.Cells[row, telefonoColumna + 1]?.Value?.ToString(),
                                        edad = _edad
                                    };

                                    var SeguroEx = await _context.Cliente.AnyAsync(u => u.cedula.Equals(clienteT.cedula));
                                    if (!SeguroEx)
                                    {
                                        clientes.Add(clienteT);
                                    }

                                }

                            }
                        }

                    }
                }
                _context.Cliente.AddRange(clientes);

                //Comprueba si se realizaron cambios
                var valor = await _context.SaveChangesAsync();
                if (valor > 0)
                {
                    return Unit.Value;
                }
                throw new ManejadorException(HttpStatusCode.InternalServerError, new { Seguro = "No se han realizado cambios. Todas las cédulas se encuentran repetidas." });

            }

            //Metodo para obtener los encabezados de cada columna
            public List<string> obtenerColumnasExcel(ExcelWorksheet worksheet)
            {
                List<string> columnHeaders = new List<string>();
                var columnCount = worksheet.Dimension?.Columns ?? 0;

                for (int col = 1; col <= columnCount; col++)
                {
                    var columnHeader = worksheet.Cells[1, col]?.Value?.ToString()?.Trim().ToLower();
                    columnHeaders.Add(columnHeader);
                }

                return columnHeaders;
            }
        }
    }
}
