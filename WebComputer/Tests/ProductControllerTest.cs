using Moq;
using Xunit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebComputer.Controllers;
using WebComputer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace WebComputer.Tests
{
    public class ProductControllerTest
    {
        [Fact]
        public void GetListProductTest()
        {
            int categoryId = 1;

            
            var products = new List<Product>
        {
            new Product { ProductId = 2, CategoryId = categoryId, Name = "Microsoft Surface Surface Pro 9 (i5 1235U/8GB RAM/256GB SSD/13/Win11/Graphite)" },
            new Product { ProductId = 5, CategoryId = categoryId, Name = "Laptop Asus VivoBook E1404FA-NK177W (R5 7520U/16GB RAM/512GB SSD/14 FHD/Win11/Bạc)" },
            new Product { ProductId = 7, CategoryId = categoryId, Name = "Laptop Dell Inspiron 3520 (N3520-i5U085W11BLU) (i5 1235U 8GB RAM/512GB SSD/15.6 inch FHD/Win11/OfficeHS21/Đen)" },
            new Product { ProductId = 9, CategoryId = categoryId, Name = "Laptop Apple Macbook Air (MLXY3SA/A) (Apple M2/8C CPU/8C GPU/8GB RAM/256GB SSD/13.6 inch IPS/Mac OS/Bạc) (2022)" },
            new Product { ProductId = 11, CategoryId = categoryId, Name = "Laptop Acer Aspire 3 A314-36M-34AP (NX.KMRSV.001) (i3 N305/8GB RAM/512GB SSD/14.0 inch FHD IPS/Vỏ kim loại/Win 11/Xanh)" }
        }.AsQueryable();

            var mockProductSet = new Mock<DbSet<Product>>();
            mockProductSet.As<IQueryable<Product>>().Setup(m => m.Provider).Returns(products.Provider);
            mockProductSet.As<IQueryable<Product>>().Setup(m => m.Expression).Returns(products.Expression);
            mockProductSet.As<IQueryable<Product>>().Setup(m => m.ElementType).Returns(products.ElementType);
            mockProductSet.As<IQueryable<Product>>().Setup(m => m.GetEnumerator()).Returns(products.GetEnumerator());

            
            var categories = new List<Category>
        {
            new Category { CategoryId = categoryId, CategoryName = "Category1" }
        }.AsQueryable();

            var mockCategorySet = new Mock<DbSet<Category>>();
            mockCategorySet.As<IQueryable<Category>>().Setup(m => m.Provider).Returns(categories.Provider);
            mockCategorySet.As<IQueryable<Category>>().Setup(m => m.Expression).Returns(categories.Expression);
            mockCategorySet.As<IQueryable<Category>>().Setup(m => m.ElementType).Returns(categories.ElementType);
            mockCategorySet.As<IQueryable<Category>>().Setup(m => m.GetEnumerator()).Returns(categories.GetEnumerator());

            
            var suppliers = new List<Supplier>
        {
            new Supplier { SupplierId = 1, Products = new List<Product> { new Product { CategoryId = categoryId } } }
        }.AsQueryable();

            var mockSupplierSet = new Mock<DbSet<Supplier>>();
            mockSupplierSet.As<IQueryable<Supplier>>().Setup(m => m.Provider).Returns(suppliers.Provider);
            mockSupplierSet.As<IQueryable<Supplier>>().Setup(m => m.Expression).Returns(suppliers.Expression);
            mockSupplierSet.As<IQueryable<Supplier>>().Setup(m => m.ElementType).Returns(suppliers.ElementType);
            mockSupplierSet.As<IQueryable<Supplier>>().Setup(m => m.GetEnumerator()).Returns(suppliers.GetEnumerator());

            
            var mockContext = new Mock<ComputerStoreContext>();
            mockContext.Setup(c => c.Products).Returns(mockProductSet.Object);
            mockContext.Setup(c => c.Categories).Returns(mockCategorySet.Object);
            mockContext.Setup(c => c.Suppliers).Returns(mockSupplierSet.Object);

            var controller = new ProductController(mockContext.Object);

            
            var result = controller.ListProduct(categoryId);

            
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<Product>>(viewResult.Model);
            Assert.Equal(5, model.Count());

            
            Assert.Equal("Category1", viewResult.ViewData["categoryname"]);
            Assert.Equal(categoryId, viewResult.ViewData["categoryId"]);
            var supplierList = Assert.IsAssignableFrom<List<Supplier>>(viewResult.ViewData["supplier"]);
            Assert.Single(supplierList);
        }

        [Fact]
        public void ProductDetailsTest()
        {
            int productId = 1;
            int categoryId = 1;

            
            var products = new List<Product>
        {
            new Product { ProductId = productId, CategoryId = categoryId, Name = "Main Product" },
            new Product { ProductId = 2, CategoryId = categoryId, Name = "Related Product 1" },
            new Product { ProductId = 3, CategoryId = categoryId, Name = "Related Product 2" },
            new Product { ProductId = 4, CategoryId = categoryId, Name = "Related Product 3" },
            new Product { ProductId = 5, CategoryId = categoryId, Name = "Related Product 4" },
            new Product { ProductId = 6, CategoryId = categoryId, Name = "Related Product 5" },
            new Product { ProductId = 7, CategoryId = categoryId, Name = "Related Product 6" }
        }.AsQueryable();

            var mockProductSet = new Mock<DbSet<Product>>();
            mockProductSet.As<IQueryable<Product>>().Setup(m => m.Provider).Returns(products.Provider);
            mockProductSet.As<IQueryable<Product>>().Setup(m => m.Expression).Returns(products.Expression);
            mockProductSet.As<IQueryable<Product>>().Setup(m => m.ElementType).Returns(products.ElementType);
            mockProductSet.As<IQueryable<Product>>().Setup(m => m.GetEnumerator()).Returns(products.GetEnumerator());

            // Mock dữ liệu cho ProductSpecifications
            var specifications = new List<ProductSpecification>
        {
            new ProductSpecification { ProductId = productId, Specification = new Specification { SpecificationName = "Spec 1" } },
            new ProductSpecification { ProductId = productId, Specification = new Specification { SpecificationName = "Spec 2" } },
            new ProductSpecification { ProductId = productId, Specification = new Specification { SpecificationName = "Spec 3" } },
            new ProductSpecification { ProductId = productId, Specification = new Specification { SpecificationName = "Spec 4" } },
            new ProductSpecification { ProductId = productId, Specification = new Specification { SpecificationName = "Spec 5" } }
        }.AsQueryable();

            var mockSpecificationSet = new Mock<DbSet<ProductSpecification>>();
            mockSpecificationSet.As<IQueryable<ProductSpecification>>().Setup(m => m.Provider).Returns(specifications.Provider);
            mockSpecificationSet.As<IQueryable<ProductSpecification>>().Setup(m => m.Expression).Returns(specifications.Expression);
            mockSpecificationSet.As<IQueryable<ProductSpecification>>().Setup(m => m.ElementType).Returns(specifications.ElementType);
            mockSpecificationSet.As<IQueryable<ProductSpecification>>().Setup(m => m.GetEnumerator()).Returns(specifications.GetEnumerator());

            
            var mockContext = new Mock<ComputerStoreContext>();
            mockContext.Setup(c => c.Products).Returns(mockProductSet.Object);
            mockContext.Setup(c => c.ProductSpecifications).Returns(mockSpecificationSet.Object);

            var controller = new ProductController(mockContext.Object);

            
            var result = controller.ProductDetail(productId);

            
            var viewResult = Assert.IsType<ViewResult>(result);  

            
            Assert.Equal(products.First(p => p.ProductId == productId), viewResult.ViewData["product"]);  
            var relativeProducts = Assert.IsAssignableFrom<IEnumerable<Product>>(viewResult.ViewData["relativeproduct"]);
            Assert.Equal(5, relativeProducts.Count());  

            var specificationList = Assert.IsAssignableFrom<IEnumerable<ProductSpecification>>(viewResult.ViewData["specification"]);
            Assert.Equal(4, specificationList.Count());  
        }

        [Fact]
        public void ListProductBySupplierTest()
        {
            int supplierId = 1;
            int categoryId = 1;

            var products = new List<Product>
        {
            new Product { ProductId = 1, CategoryId = categoryId, Suppliers = new List<Supplier> { new Supplier { SupplierId = supplierId } }, Name = "Product 1" },
            new Product { ProductId = 2, CategoryId = categoryId, Suppliers = new List<Supplier> { new Supplier { SupplierId = supplierId } }, Name = "Product 2" },
            new Product { ProductId = 3, CategoryId = 2, Suppliers = new List<Supplier> { new Supplier { SupplierId = supplierId } }, Name = "Product 3" },  // Không cùng CategoryId
            new Product { ProductId = 4, CategoryId = categoryId, Suppliers = new List<Supplier> { new Supplier { SupplierId = 2 } }, Name = "Product 4" }   // Không cùng SupplierId
        }.AsQueryable();

            var mockProductSet = new Mock<DbSet<Product>>();
            mockProductSet.As<IQueryable<Product>>().Setup(m => m.Provider).Returns(products.Provider);
            mockProductSet.As<IQueryable<Product>>().Setup(m => m.Expression).Returns(products.Expression);
            mockProductSet.As<IQueryable<Product>>().Setup(m => m.ElementType).Returns(products.ElementType);
            mockProductSet.As<IQueryable<Product>>().Setup(m => m.GetEnumerator()).Returns(products.GetEnumerator());

            var mockContext = new Mock<ComputerStoreContext>();
            mockContext.Setup(c => c.Products).Returns(mockProductSet.Object);

            var controller = new ProductController(mockContext.Object);

            var result = controller.ListProductBySupplier(supplierId, categoryId);

            var partialViewResult = Assert.IsType<PartialViewResult>(result);  
            Assert.Equal("ListProductBySupplier", partialViewResult.ViewName);  

            var model = Assert.IsAssignableFrom<IEnumerable<Product>>(partialViewResult.Model);
            Assert.Equal(2, model.Count());  
            Assert.All(model, p => Assert.Equal(categoryId, p.CategoryId));  
            Assert.All(model, p => Assert.Contains(p.Suppliers, s => s.SupplierId == supplierId));  
        }
    }
    
    
}
