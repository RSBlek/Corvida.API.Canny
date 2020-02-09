using System;

namespace Corvida.API.Canny
{
    public class BadRequestException : Exception
    {
        public String Code { get; }
        public String Description { get; }

        internal BadRequestException(String code, String description) : base($"Code: {code}\nDescription: {description}")
        {
            this.Code = code;
            this.Description = description;
        }

    }
}
