using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using orderit_api.Dto;
using orderit_api.Interfaces;
using orderit_api.Models;
using orderit_api.Repository;

namespace orderit_api.Controller
{
    [Route("api/products")]
    [ApiController]
    [Authorize]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly IBrandRepository _brandRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        public ProductController(IProductRepository productRepository, IBrandRepository brandRepository,
            ICategoryRepository categoryRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _brandRepository = brandRepository;
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        [HttpGet("{productId}")]
        [ProducesResponseType(200, Type = typeof(Product))]
        public IActionResult GetProductById(int productId)
        {
            if (!_productRepository.ProductExist(productId))
                return NotFound();

            var product = _mapper.Map<ProductDto>(_productRepository.GetProductById(productId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(product);
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Product>))]
        public IActionResult GetAllProducts()
        {
            var products = _mapper.Map<IEnumerable<ProductDto>>(_productRepository.GetAllProducts());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(products);
        }

        [HttpGet("category")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Product>))]
        public IActionResult GetProductsByCategory([FromQuery] int categoryId)
        {
            if (!_categoryRepository.CategoryExists(categoryId))
                return NotFound();

            var products = _mapper.Map<IEnumerable<ProductDto>>(_productRepository.GetByCategory(categoryId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(products);
        }

        [HttpGet("brand")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Product>))]
        public IActionResult GetProductsByBrand([FromQuery] int brandId)
        {
            if (!_brandRepository.BrandExists(brandId))
                return NotFound();

            var products = _mapper.Map<IEnumerable<ProductDto>>(_productRepository.GetByBrand(brandId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(products);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateProduct([FromQuery] int brandId, [FromQuery] int categoryId, [FromBody] ProductDto productDto) 
        { 
            //Comprobamos si el registro es nulo
            if (productDto == null)
                return BadRequest(ModelState);

            //Comprobamos que el registro no exista
            var product = _productRepository.GetAllProducts()
                             .Where(s =>
                                 s.Name.Trim().ToUpper() == productDto.Name.TrimEnd().ToUpper()
                             )
                             .FirstOrDefault();

            if (product != null)
            {
                ModelState.AddModelError("", "Product already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_brandRepository.BrandExists(brandId))
                return NotFound("Brand not found");
            
            if (!_categoryRepository.CategoryExists(categoryId))
                return NotFound("Category not found");

            var productMap = _mapper.Map<Product>(productDto);
            var brand = _brandRepository.GetBrand(brandId);
            var category = _categoryRepository.GetCategory(categoryId);

            productMap = _productRepository.AssignFKs(brand, category, productMap);

            if (!_productRepository.CreateProduct(productMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }
            return Ok("Succesfully created");
        }

        [HttpPut("{productId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateProduct(int productId, int brandId,
            int categoryId, [FromBody] ProductDto productDto) 
        { 
            if (productDto == null) return BadRequest(ModelState);

            if(productId != productDto.ProductId) return BadRequest(ModelState);

            if (!_productRepository.ProductExist(productId))
                return NotFound();

            if(!ModelState.IsValid) return BadRequest(ModelState);

            if (!_brandRepository.BrandExists(brandId))
                return NotFound("Brand not found");

            if (!_categoryRepository.CategoryExists(categoryId))
                return NotFound("Category not found");

            var productMap = _mapper.Map<Product>(productDto);
            var brand = _brandRepository.GetBrand(brandId);
            var category = _categoryRepository.GetCategory(categoryId);

            productMap = _productRepository.AssignFKs(brand, category, productMap);


            if (!_productRepository.UpdateProduct(productMap)) 
            {
                ModelState.AddModelError("", "Something went wrong updating product");
                return StatusCode(500, ModelState);

            }

            return NoContent();
        }

        [HttpDelete("{productId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult DeleteProduct(int productId)
        {
            if (!_productRepository.ProductExist(productId))
            {
                return NotFound();
            }
            var productToDelete = _productRepository.GetProductById(productId);

            if (!ModelState.IsValid) 
                return BadRequest(ModelState);

            if (!_productRepository.DeleteProduct(productToDelete)) 
            {
                ModelState.AddModelError("", "Something went wrong deleting product");
            }

            return NoContent();
        }

    }
}
