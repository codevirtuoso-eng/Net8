using NUnit.Framework;
using NUnit.Framework.Legacy;
using SharedLibrary.DTO;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace UnitTestProject.ModelValidationTests.DTOs
{
    public class LoginRequestDTOTests
    {
        // defines a common method to be used to perform model validation against classes decorated with data annotations
        private static IList<ValidationResult> MyModelValidation(object model)
        {
            var result = new List<ValidationResult>();
            var validationContext = new ValidationContext(model);

            var isValid = Validator.TryValidateObject(model, validationContext, result, validateAllProperties: true);

            if (model is IValidatableObject) (model as IValidatableObject).Validate(validationContext);

            return result;
        }

        [Test]
        public void LoginRequestDTOShouldReturnAllProperties()
        {
            var loginRequestDTO = new LoginRequestDTO()
            {
                UserName = "userName",
                UserPassword = "userPassword"
            };

            var errorcount = LoginRequestDTOTests.MyModelValidation(loginRequestDTO).Count();
            ClassicAssert.AreEqual(0, errorcount); // this is the form using older NUnit
            Assert.That(0, Is.EqualTo(errorcount)); // this is the new form using the latest NUnit
        }

        [Test]
        public void LoginRequestDTOShouldReturnMissingPassword()
        {
            var loginRequestDTO = new LoginRequestDTO()
            {
                UserName = "userName",
                UserPassword = string.Empty
            };

            var errorcount = LoginRequestDTOTests.MyModelValidation(loginRequestDTO).Count();
            ClassicAssert.AreEqual(1, errorcount); // this is the form using older NUnit
            Assert.That(1, Is.EqualTo(errorcount)); // this is the new form using the latest NUnit
        }
    }
}
