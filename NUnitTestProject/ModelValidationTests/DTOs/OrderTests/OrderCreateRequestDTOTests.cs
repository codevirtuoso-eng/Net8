using NUnit.Framework;
using NUnit.Framework.Legacy;
using SharedLibrary.DTO.Order;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NUnitTestProject.ModelValidationTests.DTOs.OrderTests
{
    public class OrderCreateRequestDTOTests
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
        public void OrderCreateRequestDTOUserIdShouldHaveRequiredAttribute()
        {
            var userIdPropertyInfo = typeof(OrderCreateRequestDTO).GetProperty("UserId");

            var userIdAttribute = userIdPropertyInfo.GetCustomAttributes(typeof(System.ComponentModel.DataAnnotations.RequiredAttribute), true)
                .Cast<System.ComponentModel.DataAnnotations.RequiredAttribute>()
                .FirstOrDefault();

            Assert.That(userIdAttribute, Is.Not.Null);
        }

        [Test]
        public void OrderCreateRequestDTOShouldReturnAllProperties()
        {
            var orderCreateRequestDTO = new OrderCreateRequestDTO()
            {
                UserId = "FakeId"
            };

            var errorcount = OrderCreateRequestDTOTests.MyModelValidation(orderCreateRequestDTO).Count();
            ClassicAssert.AreEqual(0, errorcount); // this is the form using older NUnit
            Assert.That(0, Is.EqualTo(errorcount)); // this is the new form using the latest NUnit
        }

        [Test]
        public void OrderCreateRequestDTOShouldNotReturnMissingCategory()
        {
            // perhaps a wasted tested since none of the properties are required
            var orderCreateRequestDTO = new OrderCreateRequestDTO()
            {
                UserId = null
            };

            var errorcount = OrderCreateRequestDTOTests.MyModelValidation(orderCreateRequestDTO).Count();
            ClassicAssert.AreEqual(1, errorcount); // this is the form using older NUnit
            Assert.That(1, Is.EqualTo(errorcount)); // this is the new form using the latest NUnit
        }
    }
}
