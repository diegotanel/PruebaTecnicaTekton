using NUnit.Framework;
using ApplicationServices.Products.Commands;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using FluentAssertions;

namespace Test
{
    [TestFixture]
    public class UpdateProductCommandTests
    {
        private UpdateProductCommand _command;

        [SetUp]
        public void SetUp()
        {
            _command = new UpdateProductCommand
            {
                ProductId = 1,
                Name = "Test Product",
                Status = 1,
                Stock = 10,
                Description = "Test Description",
                Price = 100.0m
            };
        }

        [Test]
        public void UpdateProductCommand_ValidData_ShouldBeValid()
        {
            // Act
            var validationResults = ValidateModel(_command);

            // Assert
            validationResults.Should().BeEmpty();
        }

        [Test]
        public void UpdateProductCommand_MissingName_ShouldBeInvalid()
        {
            // Arrange
            _command.Name = null;

            // Act
            var validationResults = ValidateModel(_command);

            // Assert
            validationResults.Should().ContainSingle();
            validationResults[0].ErrorMessage.Should().Be("El nombre es obligatorio");
        }

        [Test]
        public void UpdateProductCommand_InvalidStatus_ShouldBeInvalid()
        {
            // Arrange
            _command.Status = 2;

            // Act
            var validationResults = ValidateModel(_command);

            // Assert
            validationResults.Should().ContainSingle();
            validationResults[0].ErrorMessage.Should().Be("El valor debe ser 0 o 1");
        }

        // Helper method to validate the model
        private IList<ValidationResult> ValidateModel(object model)
        {
            var validationResults = new List<ValidationResult>();
            var context = new ValidationContext(model, null, null);
            Validator.TryValidateObject(model, context, validationResults, true);
            return validationResults;
        }
    }
}

