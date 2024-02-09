/*--****************************************************************************
  --* Project Name    : Nfs.Data
  --* Reference       : System
  --*                   System.Collections.Generic
  --*                   System.Threading.Tasks
  --*                   Microsoft.AspNetCore.Http
  --*                   Microsoft.AspNetCore.Mvc
  --*                   Microsoft.EntityFrameworkCore
  --*                   Nfs.Catalog.Service.Models.Customers
  --*                   Nfs.Core.Domain.Customers
  --*                   Nfs.Services.Customers
  --* Description     : Customer api controller
  --* Configuration Record
  --* Review            Ver  Author           Date      Cr       Comments
  --* 001               001  A HATKAR         20/06/24  CR-XXXXX Original
  --****************************************************************************/
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nfs.Catalog.Service.Models.Customers;
using Nfs.Core.Domain.Customers;
using Nfs.Services.Customers;

namespace Nfs.Catalog.Service.Controllers;

/// <summary>
/// Represents customer controller
/// </summary>
[Route("api/[controller]")]
public partial class CustomerController : BaseApiController
{
    #region Fields

    protected readonly CustomerSettings _customerSettings;
    protected readonly ICustomerService _customerService;

    #endregion

    #region Ctor

    public CustomerController(CustomerSettings customerSettings, 
        ICustomerService customerService)
    {
        _customerSettings = customerSettings;
        _customerService = customerService;
    }

    #endregion

    #region Customers

    /// <summary>
    /// Gets customers
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the list of customers
    /// </returns>
    // GET: api/Customer/List
    [HttpGet]
    [Route("List")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public virtual async Task<ActionResult<IEnumerable<Customer>>> ListAsync()
    {
        //try to get customers
        var customers = await _customerService.GetAllCustomersAsync();

        //prepare model

        return Ok(customers);
    }

    /// <summary>
    /// Gets a customer
    /// </summary>
    /// <param name="id">Customer identifier</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the customer
    /// </returns>
    // GET: api/Customer/GetById/1
    [HttpGet]
    [Route("GetById/{id:int}")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public virtual async Task<IActionResult> GetCustomerByIdAsync([FromRoute] int id)
    {
        if (id <= 0)
            return BadRequest("Invalid customer Id.");

        //try to get customer with the specified id
        var customer = await _customerService.GetCustomerByIdAsync(id);
        if (customer == null || customer.Deleted)
            return NotFound("No customer found with this specified id.");

        //prepare model

        return Ok(EntityToModel(customer));
    }

    /// <summary>
    /// Create a new customer
    /// </summary>
    /// <param name="model">Customer dto model</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    // POST: api/Customer/Create
    [HttpPost]
    [Route("Create")]
    [ProducesResponseType(typeof(Customer), StatusCodes.Status200OK)]
    public virtual async Task<ActionResult<Customer>> CreateAsync([FromBody] CustomerModel model)
    {
        if (ModelState.IsValid)
        {
            //fill entity from model
            var customer = ModelToEntity(model);

            customer.CreatedOnUtc = DateTime.UtcNow;
            customer.LastActivityDateUtc = DateTime.UtcNow;

            //insert customer
            await _customerService.InsertCustomerAsync(customer);

            //return created response status code (201)
            return CreatedAtAction(nameof(GetCustomerByIdAsync), new { id = customer.Id }, customer);
        }

        //return empty content response (204)
        return NoContent();
    }

    /// <summary>
    /// Update customer
    /// </summary>
    /// <param name="id">Customer identifier</param>
    /// <param name="model">Customer dto model</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    // PUT: api/Customer/Update/1
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut]
    [Route("Update/{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public virtual async Task<ActionResult<Customer>> UpdateAsync([FromRoute] int id,
        [FromBody] CustomerModel model)
    {
        if (id != model.Id)
        {
            return BadRequest();
        }

        //try to get the customer with the specified id
        var customer = await _customerService.GetCustomerByIdAsync(id);
        if (customer == null || customer.Deleted)
            return NotFound("No customer found with the specified id.");

        customer.Username = model.Username;
        customer.FirstName = model.FirstName;
        customer.LastName = model.LastName;
        customer.Email = model.Email;
        customer.Gender = model.Gender;
        customer.DateOfBirth = model.DateOfBirth;
        customer.Company = model.Company;
        customer.StreetAddress = model.StreetAddress;
        customer.StreetAddress2 = model.StreetAddress2;
        customer.ZipPostalCode = model.ZipPostalCode;
        customer.City = model.City;
        customer.County = model.County;
        customer.CountryId = model.CountryId;
        customer.StateProvinceId = model.StateProvinceId;
        customer.Phone = model.Phone;
        customer.Fax = model.Fax;
        customer.Active = model.Active;
        customer.Deleted = model.Deleted;
        customer.CreatedOnUtc = model.CreatedOnUtc;
        customer.LastLoginDateUtc = model.LastLoginDateUtc;
        customer.LastActivityDateUtc = model.LastActivityDateUtc;

        if (ModelState.IsValid)
        {
            try
            {
                //update existing customer
                await _customerService.UpdateCustomerAsync(customer.Id, customer);
            }
            catch (DbUpdateConcurrencyException) when (!CustomerExists(id))
            {
                return NotFound();
            }

            //return created response status code (201)
            return CreatedAtAction(nameof(GetCustomerByIdAsync), new { id = customer.Id }, customer);
        }

        //return empty content response (204)
        return NoContent();
    }

    /// <summary>
    /// Delete a customer
    /// </summary>
    /// <param name="id">Customer identifier</param>
    /// <returns>
    /// A task represents the asynchronous operation
    /// The task result contains the number of deleted customers
    /// </returns>
    // DELETE: api/Customer/Delete/1
    [HttpDelete]
    [Route("Delete/{id:int}")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public virtual async Task<ActionResult<int>> DeleteAsync([FromRoute] int id)
    {
        //try to get a customer with the specified id
        var customer = await _customerService.GetCustomerByIdAsync(id);
        if (customer == null)
            return NotFound();

        var totalRecordsDeleted = await _customerService.DeleteCustomerAsync(customer.Id, customer);

        return totalRecordsDeleted;
    }

    #endregion

    #region Utilities

    /// <summary>
    /// Customer exists check
    /// </summary>
    /// <param name="id">Customer identifier</param>
    /// <returns>True: If customer exists else false</returns>
    protected bool CustomerExists(int id)
    {
        var customer = _customerService.GetCustomerByIdAsync(id);
        return (customer == null) ? false : true;
    }

    //Object maper
    protected static CustomerModel EntityToModel(Customer entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return new CustomerModel
        {
            Id = entity.Id,
            Username = entity.Username,
            FirstName = entity.FirstName,
            LastName = entity.LastName,
            Email = entity.Email,
            Gender = entity.Gender,
            DateOfBirth = entity.DateOfBirth,
            Company = entity.Company,
            StreetAddress = entity.StreetAddress,
            StreetAddress2 = entity.StreetAddress2,
            ZipPostalCode = entity.ZipPostalCode,
            City = entity.City,
            County = entity.County,
            CountryId = entity.CountryId,
            StateProvinceId = entity.StateProvinceId,
            Phone = entity.Phone,
            Fax = entity.Fax,
            Active = entity.Active,
            Deleted = entity.Deleted,
            CreatedOnUtc = entity.CreatedOnUtc,
            LastLoginDateUtc = entity.LastLoginDateUtc,
            LastActivityDateUtc = entity.LastActivityDateUtc,
        };
    }

    //Object maper
    protected static Customer ModelToEntity(CustomerModel model)
    {
        ArgumentNullException.ThrowIfNull(model);

        return new Customer
        {
            Id = model.Id,
            Username = model.Username,
            FirstName = model.FirstName,
            LastName = model.LastName,
            Email = model.Email,
            Gender = model.Gender,
            DateOfBirth = model.DateOfBirth,
            Company = model.Company,
            StreetAddress = model.StreetAddress,
            StreetAddress2 = model.StreetAddress2,
            ZipPostalCode = model.ZipPostalCode,
            City = model.City,
            County = model.County,
            CountryId = model.CountryId,
            StateProvinceId = model.StateProvinceId,
            Phone = model.Phone,
            Fax = model.Fax,
            Active = model.Active,
            Deleted = model.Deleted,
            CreatedOnUtc = model.CreatedOnUtc,
            LastLoginDateUtc = model.LastLoginDateUtc,
            LastActivityDateUtc = model.LastActivityDateUtc,
        };
    }

    #endregion
}