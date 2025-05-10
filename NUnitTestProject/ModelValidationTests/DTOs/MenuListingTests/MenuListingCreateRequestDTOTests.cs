using NUnit.Framework;
using NUnit.Framework.Legacy;
using SharedLibrary.DTO.MenuListing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace NUnitTestProject.ModelValidationTests.DTOs.MenuListingTests
{
	public class MenuListingCreateRequestDTOTests
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
		public void MenuListingCreateRequestDTOShouldReturnAllProperties()
		{
			var menuListingCreateRequestDTO = new MenuListingCreateRequestDTO()
			{
				ItemId = 1,
				Category = "Side",
				Name = "French Fries",
				Cost = 3.95M
			};

			var errorcount = MenuListingCreateRequestDTOTests.MyModelValidation(menuListingCreateRequestDTO).Count();
			ClassicAssert.AreEqual(0, errorcount); // this is the form using older NUnit
            Assert.That(0, Is.EqualTo(errorcount)); // this is the new form using the latest NUnit
		}

		[Test]
		public void MenuListingCreateRequestDTOShouldReturnMissingCategory()
		{
			var menuListingCreateRequestDTO = new MenuListingCreateRequestDTO()
			{
				ItemId = 1,
				Category = "",
				Name = "French Fries",
				Cost = 3.95M
			};

			var errorcount = MenuListingCreateRequestDTOTests.MyModelValidation(menuListingCreateRequestDTO).Count();
            ClassicAssert.AreEqual(1, errorcount); // this is the form using older NUnit
            Assert.That(1, Is.EqualTo(errorcount)); // this is the new form using the latest NUnit
        }
	}
}
