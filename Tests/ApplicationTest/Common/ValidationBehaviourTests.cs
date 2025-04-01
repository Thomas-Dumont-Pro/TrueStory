using Application.Common.Behaviours;
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
        private readonly Mock<IValidator<SampleRequest>> _validatorMock;
        private readonly ValidationBehaviour<SampleRequest, SampleResponse> _validationBehaviour;

        public ValidationBehaviourTests()
        {
            _validatorMock = new Mock<IValidator<SampleRequest>>();
            var validators = new List<IValidator<SampleRequest>> { _validatorMock.Object };
            _validationBehaviour = new ValidationBehaviour<SampleRequest, SampleResponse>(validators);
        }

        [Test,Order(0)]
        public async Task Handle_ShouldCallValidateAsync()
        {
            // Arrange
            var request = new SampleRequest();
            var next = new Mock<RequestHandlerDelegate<SampleResponse>>();

            // Act
            await _validationBehaviour.Handle(request, next.Object, CancellationToken.None);

            // Assert
            _validatorMock.Verify(v => v.ValidateAsync(It.IsAny<ValidationContext<SampleRequest>>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task Handle_ShouldThrowValidationException_WhenValidationFails()
        {
            // Arrange
            var request = new SampleRequest();
            var next = new Mock<RequestHandlerDelegate<SampleResponse>>();
            var failures = new List<ValidationFailure> { new ValidationFailure("Property", "Error") };
            _validatorMock.Setup(v => v.ValidateAsync(It.IsAny<ValidationContext<SampleRequest>>(), It.IsAny<CancellationToken>()))
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
            var request = new SampleRequest();
            var next = new Mock<RequestHandlerDelegate<SampleResponse>>();
            _validatorMock.Setup(v => v.ValidateAsync(It.IsAny<ValidationContext<SampleRequest>>(), It.IsAny<CancellationToken>()))
                          .ReturnsAsync(new ValidationResult());

            // Act
            await _validationBehaviour.Handle(request, next.Object, CancellationToken.None);

            // Assert
            next.Verify(n => n(), Times.Once);
        }
    }

    public class SampleRequest { }

    public class SampleResponse { }
}