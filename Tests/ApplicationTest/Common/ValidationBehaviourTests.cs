using Application.Common.Behaviours;
using Application.Queries;
using Domain.Models;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Moq;
using NUnit.Framework;

namespace ApplicationTest.Common
{
    public class ValidationBehaviourTests
    {
        private readonly Mock<IValidator<GetItem>> _validatorMock;
        private readonly ValidationBehaviour<GetItem, Item> _validationBehaviour;

        public ValidationBehaviourTests()
        {
            _validatorMock = new Mock<IValidator<GetItem>>();
            var validators = new List<IValidator<GetItem>> { _validatorMock.Object };
            _validationBehaviour = new ValidationBehaviour<GetItem, Item>(validators);
        }

        [Test,Order(0)]
        public async Task Handle_ShouldCallValidateAsync()
        {
            // Arrange
            var request = new GetItem("NotEmpty");
            var next = new Mock<RequestHandlerDelegate<Item>>(); 
            _validatorMock.Setup(v => v.ValidateAsync(It.IsAny<GetItem>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());

            // Act
            await _validationBehaviour.Handle(request, next.Object, CancellationToken.None);

            // Assert
            _validatorMock.Verify(v => v.ValidateAsync(It.IsAny<GetItem>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task Handle_ShouldThrowValidationException_WhenValidationFails()
        {
            // Arrange
            var request = new GetItem("NotEmpty");
            var next = new Mock<RequestHandlerDelegate<Item>>();
            var failures = new List<ValidationFailure> { new ("Property", "Error") };
            _validatorMock.Setup(v => v.ValidateAsync(It.IsAny<GetItem>(), It.IsAny<CancellationToken>()))
                          .ReturnsAsync(new ValidationResult(failures));

            // Act
            Func<Task> act = async () => await _validationBehaviour.Handle(request, next.Object, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<ValidationException>();
        }

        [Test]
        public async Task Handle_ShouldCallNext_WhenValidationSucceeds()
        {
            // Arrange
            var request = new GetItem("NotEmpty");
            var next = new Mock<RequestHandlerDelegate<Item>>();
            _validatorMock.Setup(v => v.ValidateAsync(It.IsAny<GetItem>(), It.IsAny<CancellationToken>()))
                          .ReturnsAsync(new ValidationResult());

            // Act
            await _validationBehaviour.Handle(request, next.Object, CancellationToken.None);

            // Assert
            next.Verify(n => n(), Times.Once);
        }
    }
}