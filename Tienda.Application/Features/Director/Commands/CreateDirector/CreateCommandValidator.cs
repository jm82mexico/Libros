using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;

namespace Tienda.Application.Features.Director.Commands.CreateDirector
{
    public class CreateCommandValidator : AbstractValidator<CreateDirectorCommand>
    {
        public CreateCommandValidator()
        {
            RuleFor(p => p.Nombre)
                .NotEmpty().WithMessage("{PropertyName} es requerido.")
                .NotNull()
                .MaximumLength(50).WithMessage("{PropertyName} debe tener un máximo de {MaxLength} caracteres.");

            RuleFor(p => p.Apellido)
                .NotEmpty().WithMessage("{PropertyName} es requerido.")
                .NotNull()
                .MaximumLength(50).WithMessage("{PropertyName} debe tener un máximo de {MaxLength} caracteres.");

            
        }
    }
    
}