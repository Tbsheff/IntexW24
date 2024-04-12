// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace Intex.Areas.Identity.Pages.Account.Manage
{
    public class DeletePersonalDataModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ILogger<DeletePersonalDataModel> _logger;

        // Constructor that initializes user manager, sign-in manager, and logger.
        public DeletePersonalDataModel(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            ILogger<DeletePersonalDataModel> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }


        // Bindable property to handle user input on the page.
        [BindProperty]
        public InputModel Input { get; set; }

        /// // Class for binding and validating the user input password.
        public class InputModel
        {

            [Required] // Validation to ensure the field is not empty.
            [DataType(DataType.Password)] // Specifies that the field is a password.
            public string Password { get; set; }
        }

        // Property to determine if the user has a password set.
        public bool RequirePassword { get; set; }

        // OnGet method to prepare the page when loaded.
        public async Task<IActionResult> OnGet()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                // If no user is found, return a NotFound result.
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            // Check if the user has a password set and update the RequirePassword property.
            RequirePassword = await _userManager.HasPasswordAsync(user);
            return Page();
        }

        // OnPostAsync method to handle the form submission.
        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                // If no user is found, return a NotFound result.
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            RequirePassword = await _userManager.HasPasswordAsync(user);
            if (RequirePassword)
            {
                // Check the user's password if one is required.
                if (!await _userManager.CheckPasswordAsync(user, Input.Password))
                {
                    // If the password is incorrect, add an error to the ModelState and reload the page.
                    ModelState.AddModelError(string.Empty, "Incorrect password.");
                    return Page();
                }
            }

            // Attempt to delete the user.
            var result = await _userManager.DeleteAsync(user);
            var userId = await _userManager.GetUserIdAsync(user);
            if (!result.Succeeded)
            {
                // If the deletion fails, throw an exception.
                throw new InvalidOperationException($"Unexpected error occurred deleting user.");
            }

            // Sign out the user.
            await _signInManager.SignOutAsync();

            // Log the deletion of the user.
            _logger.LogInformation("User with ID '{UserId}' deleted themselves.", userId);

            // Redirect to the home page after deletion.
            return Redirect("~/");
        }
    }
}
