using aspCoreMvc.Controllers;
using aspCoreMvc.Infrastructure;
using aspCoreMvc.Infrastructure.Interfaces;
using aspCoreMvc.Infrastructure.Options;
using aspCoreMvc.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace unitTests
{
    [TestFixture]
    public class ProductControllerTest
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly IOptionsSnapshot<ProductOptions> _options;
        private readonly ILogger<ProductController> _logger;
        private readonly ISupplierService _supplierService;

        public ProductControllerTest()
        {
            _productService = Mock.Of<IProductService>();
            Mock.Get(_productService).Setup(s => s.GetAll(It.IsAny<int>())).Returns(GetProducts());
            Mock.Get(_productService).Setup(s => s.Get(It.IsAny<int>())).Returns(new Product { Id = 1 });

            _supplierService = Mock.Of<ISupplierService>();
            Mock.Get(_supplierService).Setup(s => s.GetAll()).Returns(GetSuppliers());

            _categoryService = Mock.Of<ICategoryService>();
            Mock.Get(_categoryService).Setup(s => s.GetAll()).Returns(GetCategories());

            _options = Mock.Of<IOptionsSnapshot<ProductOptions>>(m => m.Value == new ProductOptions { ProductCount = 3 });
            _logger = Mock.Of<ILogger<ProductController>>();
        }

     

        [Test]
        public void Index_ReturnViewResult_WithProductList()
        {
            //Arrange
            var controller = new ProductController(_productService, null, null, _options, _logger);

            //Act
            var result = controller.Index();

            //Assert
            Assert.That(result, Is.InstanceOf<ViewResult>());
            var viewResult = result as ViewResult;
            Assert.That(viewResult.ViewData.Model, Is.AssignableTo<IEnumerable<ProductViewModel>>());
            var model = viewResult.ViewData.Model as IEnumerable<ProductViewModel>;
            Assert.That(model, Has.Exactly(GetProducts().Count()).Items);
        }

        [Test]
        public void Create_ReturnViewResult_WithZeroModelId()
        {
            //Arrange
            var controller = new ProductController(null, _supplierService, _categoryService, _options, _logger);

            //Act
            var result = controller.Create();

            //Assert
            Assert.That(result, Is.InstanceOf<ViewResult>());
            var viewResult = result as ViewResult;
            Assert.That(viewResult.ViewName, Is.EqualTo("Update"));
            Assert.That(viewResult.ViewData.Model, Is.InstanceOf<ProductViewModel>());
            var model = viewResult.ViewData.Model as ProductViewModel;
            Assert.That(model.Id, Is.Zero);
        }

        [Test]
        public void Update_ReturnViewresult_WithNonZeroModelId()
        {
            //Arrange
            var controller = new ProductController(_productService, _supplierService, _categoryService, _options, _logger);

            //Act
            var result = controller.Update(id: 1);

            //Assert
            Assert.That(result, Is.InstanceOf<ViewResult>());
            var viewResult = result as ViewResult;
            Assert.That(viewResult.ViewData.Model, Is.InstanceOf<ProductViewModel>());
            var model = viewResult.ViewData.Model as ProductViewModel;
            Assert.That(model.Id, Is.Not.Zero);
        }

        [Test]
        public void UpdatePost_ReturnViewResult_WhenModelStateIsNotValid()
        {
            //Arrange
            var controller = new ProductController(null, _supplierService, _categoryService, _options, _logger);
            controller.ModelState.AddModelError("ProductName", "Required");
            var model = new ProductViewModel();

            //Act
            var result = controller.Update(model);

            //Assert
            Assert.That(result, Is.InstanceOf<ViewResult>());
            var viewResult = result as ViewResult;
            Assert.That(viewResult.ViewData.Model, Is.InstanceOf<ProductViewModel>());
            Assert.That(viewResult.ViewData.ModelState.ErrorCount, Is.Not.Zero);
        }

        [Test]
        public void UpdatePost_ReturnRedirectToActionAndCreate_WhenModelIsValidAndZeroModelId()
        {
            //Arrange
            Mock.Get(_productService).Setup(s => s.Create(It.IsAny<Product>())).Returns(1).Verifiable();
            var controller = new ProductController(_productService, _supplierService, _categoryService, _options, _logger);
            var model = new ProductViewModel { Id = 0 };

            //Act
            var result = controller.Update(model);

            //Assert
            Assert.That(result, Is.InstanceOf<RedirectToActionResult>());
            var redirectResult = result as RedirectToActionResult;
            Assert.That(redirectResult.ControllerName, Is.Null);
            Assert.That(redirectResult.ActionName, Is.EqualTo("Index"));
            Mock.Get(_productService).Verify();
        }

        [Test]
        public void UpdatePost_ReturnRedirectToActionAndUpdate_WhenModelIsValidAndNonZeroModelId()
        {
            //Arrange
            Mock.Get(_productService).Setup(s => s.Update(It.IsAny<Product>())).Verifiable();
            var controller = new ProductController(_productService, _supplierService, _categoryService, _options, _logger);
            var model = new ProductViewModel { Id = 1 };

            //Act
            var result = controller.Update(model);

            //Assert
            Assert.That(result, Is.InstanceOf<RedirectToActionResult>());
            var redirectResult = result as RedirectToActionResult;
            Assert.That(redirectResult.ControllerName, Is.Null);
            Assert.That(redirectResult.ActionName, Is.EqualTo("Index"));
            Mock.Get(_productService).Verify();
        }


        private IEnumerable<ProductViewModel> GetProducts()
        {
            return new List<ProductViewModel>
            {
                new ProductViewModel(),
                new ProductViewModel()
            };
        }

        private IEnumerable<Supplier> GetSuppliers()
        {
            return new List<Supplier>
            {
                new Supplier(),
                new Supplier()
            };
        }

        private IEnumerable<Category> GetCategories()
        {
            return new List<Category>
            {
                new Category(),
                new Category()
            };
        }
    }
}
