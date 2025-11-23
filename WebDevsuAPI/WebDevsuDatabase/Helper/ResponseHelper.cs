using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebDevsuDatabase.Models.DTOs;

namespace WebDevsuDatabase.Helper
{
    public static class ResponseHelper
    {
        public static ResponseDTO<T> Ok<T>(T data, string message = "Operación exitosa")
        {
            return new ResponseDTO<T>
            {
                Status = 200,
                Success = true,
                Data = data,
                Message = message
            };
        }

        public static ResponseDTO<T> NotFound<T>(string message = "No encontrado")
        {
            return new ResponseDTO<T>
            {
                Status = 404,
                Success = false,
                Message = message
            };
        }

        public static ResponseDTO<T> Error<T>(string message = "Error interno del servidor")
        {
            return new ResponseDTO<T>
            {
                Status = 500,
                Success = false,
                Message = message
            };
        }
    }
}
