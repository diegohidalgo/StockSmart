using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StockSmart.Application.Products.Command.CreateProduct;
using StockSmart.Application.Products.Command.UpdateProduct;
using StockSmart.Application.Products.Queries.GetProductById;
using StockSmart.Presentation.Controllers.Base;

namespace StockSmart.Presentation.Controllers.V1;

[ApiController]
[Route("api/product")]
[ApiVersion("1.0")]
public class ProductController(IMediator mediator) : ApiControllerBase(mediator)
{

    /// <summary>
    /// Gets a product by id
    /// </summary>
    /// <remarks>Allows the consumer to retrieve a single product</remarks>
    /// <param name="id"></param>
    /// <returns>ProductResponse</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ProductResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ProductResponse>> GetById(int id)
    {
        var result = await _mediator.Send(new GetProductByIdQuery(id));
        return this.Ok(result);
    }

    /// <summary>
    /// Creates a product
    /// </summary>
    /// <remarks>Allows the consumer to create multiple products at the same time</remarks>
    /// <param name="product"></param>
    /// <returns>List of ProductResponse</returns>
    [HttpPost]
    [ProducesResponseType(typeof(IEnumerable<ProductResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<ProductResponse>>> Post(
        [FromBody] CreateProductCommand product,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(product, cancellationToken);
        return Created(string.Empty, result);
    }

    /// <summary>
    /// Updates a product
    /// </summary>
    /// <remarks>Allows the consumer to update multiple products at the same time</remarks>
    /// <param name="product"></param>
    /// <returns>List of ProductResponse</returns>
    [HttpPut]
    [ProducesResponseType(typeof(IEnumerable<ProductResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<ProductResponse>>> Update(
        [FromBody] UpdateProductCommand product,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(product, cancellationToken);
        return Created(string.Empty, result);
    }
}
