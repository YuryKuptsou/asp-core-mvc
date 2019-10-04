using aspCoreMvc.Controllers;
using aspCoreMvc.Infrastructure;
using aspCoreMvc.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace unitTests
{
    [TestFixture]
    public class CategoryControllerTest
    {
        private readonly ICategoryService _categoryService;
        public CategoryControllerTest()
        {
            _categoryService = Mock.Of<ICategoryService>();
            Mock.Get(_categoryService).Setup(c => c.GetAll()).Returns(GetCatigories());
        }

        [Test]
        public void Index_ReturnViewResult_WithCategoryList()
        {
            //Arrange
            var controller = new CategoryController(_categoryService);

            //Act
            var result = controller.Index();

            //Assert
            Assert.That(result, Is.InstanceOf<ViewResult>());
            var viewResult = result as ViewResult;
            Assert.That(viewResult.ViewData.Model, Is.AssignableTo<IEnumerable<Category>>());
            var model = viewResult.ViewData.Model as IEnumerable<Category>;
            Assert.That(model, Has.Exactly(GetCatigories().Count()).Items);

        }

        private IEnumerable<Category> GetCatigories()
        {
            return new List<Category>
            {
                new Category { CategoryName = "cat1"},
                new Category { CategoryName = "cat2"}
            };
        }

    }
}
