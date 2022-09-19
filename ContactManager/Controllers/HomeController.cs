using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text.Json;
using System.Web;
using ContactManager.Models.Data;
using ContactManager.Models.Entities;
using ContactManager.Models.Values;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace ContactManager.Controllers;

public class HomeController : Controller
{
    private readonly ContactDbContext _context;
    
    public HomeController(ContactDbContext context)
    {
        _context = context;
    }
    
    [HttpGet]
    public async Task<IActionResult> Index([FromQuery(Name = "page")] int page)
    {
        dynamic model  = new ExpandoObject();
        model.Contacts = await _context.Contacts.Skip(PageValues.CountOnPage*page)
            .Take(PageValues.CountOnPage).ToListAsync();
        model.Jobs = await _context.Jobs.ToListAsync();
        model.Page = page;
        model.isLast = await GetPageCount() <= page + 1;
        return View(model);
    }

    [HttpGet("{id}")]
    public async Task<Contact> GetContact(uint id)
    {
        return await _context.Contacts.Include(c => c.Position)
            .FirstAsync(c => c.ContactId == id);
    }
        

    [HttpPost]
    public async Task<double> AddContact([FromBody]JsonElement jsonContact)
    {
        Contact contact = jsonContact;
        if (await _context.Jobs.AnyAsync(j => j.JobTitle.Equals(contact.Position.JobTitle)))
        {
            contact.Position = await _context.Jobs.FirstAsync(j => j.JobTitle.Equals(contact.Position.JobTitle));
        }
        await _context.Contacts.AddAsync(contact);
        await _context.SaveChangesAsync();
        return await GetPageCount() - 1;
    }
    
    [HttpPut("{id}")]
    public async Task UpdateContact(uint id, [FromBody]JsonElement jsonContact)
    {
        Contact contact = jsonContact;
        contact.ContactId = id;
        if (await _context.Jobs.AnyAsync(j => j.JobTitle.Equals(contact.Position.JobTitle)))
        {
            contact.Position = await _context.Jobs.FirstAsync(j => j.JobTitle.Equals(contact.Position.JobTitle));
        }
        var oldContact = await _context.Contacts.Include(c => c.Position)
            .FirstAsync(c => c.ContactId == id);
        if(await _context.Contacts.CountAsync(c => c.Position.JobId == oldContact.Position.JobId) == 1)
        {
            _context.Jobs.Remove(oldContact.Position);
        }
        oldContact.Update(contact);
        await _context.SaveChangesAsync();
    }
    
    [HttpDelete("{id}")]
    public async Task<bool> DeleteContact(uint id, [FromQuery(Name = "page")] int page)
    {
        var contact = await _context.Contacts.Include(c => c.Position)
            .FirstAsync(c => c.ContactId == id);
        if(await _context.Contacts.CountAsync(c => c.Position.JobId == contact.Position.JobId) == 1)
        {
            _context.Jobs.Remove(contact.Position);
        }
        _context.Contacts.Remove(contact);
        await _context.SaveChangesAsync();
        return await GetPageCount() <= page;
    }

    private async Task<double> GetPageCount()
    {
        return Math.Ceiling((double) await _context.Contacts.CountAsync() / PageValues.CountOnPage);
    }
}