﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProtectiveWearProductsApi.Models;
using ProtectiveWearProductsApi.Services;

namespace ProtectiveWearProductsApi.Controllers
{
    [Route("v1/api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        /// <summary>
        /// Propiedad que permite aplicar una injeccion en el constructor para invocar los metodos del servicio
        /// </summary>
        private readonly ProductService _productService;

        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="productService"></param>
        public ProductsController(ProductService productService)
        {
            this._productService = productService;
        }

        /// <summary>
        /// Proceso que consulta una lista de productos
        /// </summary>
        /// <returns>Retorna una lista de obketos de tipo producto</returns>
        [HttpGet]
        public async Task<ActionResult<List<Product>>> Products()
        {
            try
            {
                return await _productService.GetAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Proceso para consultar el detalle de un producto
        /// </summary>
        /// <param name="id">Identificación de un producto</param>
        /// <returns>Retorna la información de tallada de un producto</returns>
        [HttpGet("{id}", Name = "GetProductById")]
        public async Task<ActionResult<Product>> GetProduct(string id)
        {
            try
            {
                var result = await _productService.GetAsync(id);
                if (result == null)
                    return NotFound();

                return Ok(result);
            }
            catch (Exception)
            {
                throw;
            }
        }


        /// <summary>
        /// Proceso de creación de un producto
        /// </summary>
        /// <param name="model">Objeto de tipo producto</param>
        /// <returns>Retorna el nevo objeto creado con su id</returns>
        [HttpPost]
        public async Task<ActionResult<Product>> Create([FromBody]Product model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var newrecord = await _productService.CreateAsync(model);

                if (newrecord == null)
                    throw new Exception("El producto no pudo ser creado");

                return CreatedAtRoute("GetProductById", new { id = model.Id.ToString() }, model);
            }
            catch (Exception)
            {

                throw;
            }
        }


        /// <summary>
        /// Proceso de actualización de un producto
        /// </summary>
        /// <param name="id">Identificación de un producto</param>
        /// <param name="prod">Objeto de tipo producto</param>
        /// <returns>Retorna un valor vacio con resultado ok</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] Product prod)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                await _productService.UpdateAsync(id, prod);

                return NoContent();
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Proceso de eliminación de un producto
        /// </summary>
        /// <param name="id">Identificación de un producto</param>
        /// <returns>Retorna un valor vacio con resultado ok</returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {

            try
            {
                await _productService.RemoveAsync(id);
                return NoContent();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}