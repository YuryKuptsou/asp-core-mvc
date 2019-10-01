using aspCoreMvc.Controllers;
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
using System.Text;

namespace unitTests
{
    [TestFixture]
    public class ProductControllerTest
    {
        private readonly IProductService _productService;
        private readonly IOptionsSnapshot<ProductOptions> _options;
        private readonly ILogger<ProductController> _logger;
        public ProductControllerTest()
        {
            _productService = Mock.Of<IProductService>();
            Mock.Get(_productService).Setup(s => s.GetAll(It.IsAny<int>())).Returns(GetProducts());

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
        }

        private IEnumerable<ProductViewModel> GetProducts()
        {
            return new List<ProductViewModel>
            {
                new ProductViewModel(),
                new ProductViewModel()
            };
        }
    }
}
