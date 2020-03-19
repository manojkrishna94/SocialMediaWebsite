﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SocialMedia.Data;
using SocialMedia.Models;

namespace SocialMedia.Web.Areas.Identity.Pages.Account.Manage
{
    public partial class IndexModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly SocialMediaDbContext _context;

        public IndexModel(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            SocialMediaDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            this._context = context;
        }

        public string Username { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }

            [Display(Name = "First name")]
            public string FirstName { get; set; }
            
            [Display(Name = "Last name")]
            public string LastName { get; set; }

            //TODO: Date picker
            [Display(Name = "Date of birth")]
            [DisplayFormat(DataFormatString = "{0:dd'/'MM'/'yyyy}", ApplyFormatInEditMode = true)]
            public DateTime? DOB { get; set; }

            [Display(Name = "Gender")]
            public Gender Gender { get; set; }

            [Display(Name = "Bio")]
            public string Bio { get; set; }
        }

        private async Task LoadAsync(User user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

            Username = userName;

            Input = new InputModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Gender = user.Gender,
                DOB = user.DOB,
                Bio = user.Bio
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            //Update details
            if (Input.FirstName != user.FirstName)
            {
                try
                {
                    var updateUser = await this._context.Users.FirstOrDefaultAsync(i => i.Id == user.Id);
                    updateUser.FirstName = this.Input.FirstName;
                    await this._context.SaveChangesAsync();
                }
                catch (Exception)
                {
                    StatusMessage = "Your first name has not been updated";
                    return RedirectToPage();
                }
               
            }
            if (Input.LastName != user.LastName)
            {
                try
                {
                    var updateUser = await this._context.Users.FirstOrDefaultAsync(i => i.Id == user.Id);
                    updateUser.LastName = this.Input.LastName;
                    await this._context.SaveChangesAsync();
                }
                catch (Exception)
                {
                    StatusMessage = "Your last name has not been updated";
                    return RedirectToPage();
                }

            }
            if (Input.DOB != user.DOB)
            {
                try
                {
                    var updateUser = await this._context.Users.FirstOrDefaultAsync(i => i.Id == user.Id);
                    updateUser.DOB = Input.DOB;
                    await this._context.SaveChangesAsync();
                }
                catch (Exception)
                {
                    StatusMessage = "Your date of birth has not been updated";
                    return RedirectToPage();
                }

            }
            if (Input.Gender != user.Gender)
            {
                try
                {
                    var updateUser = await this._context.Users.FirstOrDefaultAsync(i => i.Id == user.Id);
                    updateUser.Gender = Input.Gender;
                    await this._context.SaveChangesAsync();
                }
                catch (Exception)
                {
                    StatusMessage = "Your gender has not been updated";
                    return RedirectToPage();
                }

            }
            if (Input.Bio != user.Bio)
            {
                try
                {
                    var updateUser = await this._context.Users.FirstOrDefaultAsync(i => i.Id == user.Id);
                    updateUser.Bio = Input.Bio;
                    await this._context.SaveChangesAsync();
                }
                catch (Exception)
                {
                    StatusMessage = "Your bio has not been updated";
                    return RedirectToPage();
                }

            }
            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }
    }
}