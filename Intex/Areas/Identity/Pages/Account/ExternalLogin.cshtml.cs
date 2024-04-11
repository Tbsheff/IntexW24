// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using Intex.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;

namespace Intex.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ExternalLoginModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IUserStore<IdentityUser> _userStore;
        private readonly IUserEmailStore<IdentityUser> _emailStore;
        private readonly IEmailSender _emailSender;
        private readonly ILogger<ExternalLoginModel> _logger;
        private readonly ILegoRepository _repo;

        
        public List<SelectListItem> Countries { get; set; }
        public List<SelectListItem> Genders { get; set; }
        
        public ExternalLoginModel(
            SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager,
            IUserStore<IdentityUser> userStore,
            ILogger<ExternalLoginModel> logger,
            IEmailSender emailSender,
            ILegoRepository repo)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _logger = logger;
            _emailSender = emailSender;
            _repo = repo;
            
                        Genders = new List<SelectListItem>
            {
                new SelectListItem("Male", "M"),
                new SelectListItem("Female", "F"),
            };
            
            Countries = new List<SelectListItem>
{
    new SelectListItem("Afghanistan", "Afghanistan"),
    new SelectListItem("Åland Islands", "Åland Islands"),
    new SelectListItem("Albania", "Albania"),
    new SelectListItem("Algeria", "Algeria"),
    new SelectListItem("American Samoa", "American Samoa"),
    new SelectListItem("Andorra", "Andorra"),
    new SelectListItem("Angola", "Angola"),
    new SelectListItem("Anguilla", "Anguilla"),
    new SelectListItem("Antarctica", "Antarctica"),
    new SelectListItem("Antigua and Barbuda", "Antigua and Barbuda"),
    new SelectListItem("Argentina", "Argentina"),
    new SelectListItem("Armenia", "Armenia"),
    new SelectListItem("Aruba", "Aruba"),
    new SelectListItem("Australia", "Australia"),
    new SelectListItem("Austria", "Austria"),
    new SelectListItem("Azerbaijan", "Azerbaijan"),
    new SelectListItem("Bahamas", "Bahamas"),
    new SelectListItem("Bahrain", "Bahrain"),
    new SelectListItem("Bangladesh", "Bangladesh"),
    new SelectListItem("Barbados", "Barbados"),
    new SelectListItem("Belarus", "Belarus"),
    new SelectListItem("Belgium", "Belgium"),
    new SelectListItem("Belize", "Belize"),
    new SelectListItem("Benin", "Benin"),
    new SelectListItem("Bermuda", "Bermuda"),
    new SelectListItem("Bhutan", "Bhutan"),
    new SelectListItem("Bolivia", "Bolivia"),
    new SelectListItem("Bosnia and Herzegovina", "Bosnia and Herzegovina"),
    new SelectListItem("Botswana", "Botswana"),
    new SelectListItem("Bouvet Island", "Bouvet Island"),
    new SelectListItem("Brazil", "Brazil"),
    new SelectListItem("British Indian Ocean Territory", "British Indian Ocean Territory"),
    new SelectListItem("Brunei Darussalam", "Brunei Darussalam"),
    new SelectListItem("Bulgaria", "Bulgaria"),
    new SelectListItem("Burkina Faso", "Burkina Faso"),
    new SelectListItem("Burundi", "Burundi"),
    new SelectListItem("Cambodia", "Cambodia"),
    new SelectListItem("Cameroon", "Cameroon"),
    new SelectListItem("Canada", "Canada"),
    new SelectListItem("Cape Verde", "Cape Verde"),
    new SelectListItem("Cayman Islands", "Cayman Islands"),
    new SelectListItem("Central African Republic", "Central African Republic"),
    new SelectListItem("Chad", "Chad"),
    new SelectListItem("Chile", "Chile"),
    new SelectListItem("China", "China"),
    new SelectListItem("Christmas Island", "Christmas Island"),
    new SelectListItem("Cocos (Keeling) Islands", "Cocos (Keeling) Islands"),
    new SelectListItem("Colombia", "Colombia"),
    new SelectListItem("Comoros", "Comoros"),
    new SelectListItem("Congo", "Congo"),
    new SelectListItem("Congo, The Democratic Republic of The", "Congo, The Democratic Republic of The"),
    new SelectListItem("Cook Islands", "Cook Islands"),
    new SelectListItem("Costa Rica", "Costa Rica"),
    new SelectListItem("Cote D'ivoire", "Cote D'ivoire"),
    new SelectListItem("Croatia", "Croatia"),
    new SelectListItem("Cuba", "Cuba"),
    new SelectListItem("Cyprus", "Cyprus"),
    new SelectListItem("Czech Republic", "Czech Republic"),
    new SelectListItem("Denmark", "Denmark"),
    new SelectListItem("Djibouti", "Djibouti"),
    new SelectListItem("Dominica", "Dominica"),
    new SelectListItem("Dominican Republic", "Dominican Republic"),
    new SelectListItem("Ecuador", "Ecuador"),
    new SelectListItem("Egypt", "Egypt"),
    new SelectListItem("El Salvador", "El Salvador"),
    new SelectListItem("Equatorial Guinea", "Equatorial Guinea"),
    new SelectListItem("Eritrea", "Eritrea"),
    new SelectListItem("Estonia", "Estonia"),
    new SelectListItem("Ethiopia", "Ethiopia"),
    new SelectListItem("Falkland Islands (Malvinas)", "Falkland Islands (Malvinas)"),
    new SelectListItem("Faroe Islands", "Faroe Islands"),
    new SelectListItem("Fiji", "Fiji"),
    new SelectListItem("Finland", "Finland"),
    new SelectListItem("France", "France"),
    new SelectListItem("French Guiana", "French Guiana"),
    new SelectListItem("French Polynesia", "French Polynesia"),
    new SelectListItem("French Southern Territories", "French Southern Territories"),
    new SelectListItem("Gabon", "Gabon"),
    new SelectListItem("Gambia", "Gambia"),
    new SelectListItem("Georgia", "Georgia"),
    new SelectListItem("Germany", "Germany"),
    new SelectListItem("Ghana", "Ghana"),
    new SelectListItem("Gibraltar", "Gibraltar"),
    new SelectListItem("Greece", "Greece"),
    new SelectListItem("Greenland", "Greenland"),
    new SelectListItem("Grenada", "Grenada"),
    new SelectListItem("Guadeloupe", "Guadeloupe"),
    new SelectListItem("Guam", "Guam"),
    new SelectListItem("Guatemala", "Guatemala"),
    new SelectListItem("Guernsey", "Guernsey"),
    new SelectListItem("Guinea", "Guinea"),
    new SelectListItem("Guinea-bissau", "Guinea-bissau"),
    new SelectListItem("Guyana", "Guyana"),
    new SelectListItem("Haiti", "Haiti"),
    new SelectListItem("Heard Island and Mcdonald Islands", "Heard Island and Mcdonald Islands"),
    new SelectListItem("Holy See (Vatican City State)", "Holy See (Vatican City State)"),
    new SelectListItem("Honduras", "Honduras"),
    new SelectListItem("Hong Kong", "Hong Kong"),
    new SelectListItem("Hungary", "Hungary"),
    new SelectListItem("Iceland", "Iceland"),
    new SelectListItem("India", "India"),
    new SelectListItem("Indonesia", "Indonesia"),
    new SelectListItem("Iran, Islamic Republic of", "Iran, Islamic Republic of"),
    new SelectListItem("Iraq", "Iraq"),
    new SelectListItem("Ireland", "Ireland"),
    new SelectListItem("Isle of Man", "Isle of Man"),
    new SelectListItem("Israel", "Israel"),
    new SelectListItem("Italy", "Italy"),
    new SelectListItem("Jamaica", "Jamaica"),
    new SelectListItem("Japan", "Japan"),
    new SelectListItem("Jersey", "Jersey"),
    new SelectListItem("Jordan", "Jordan"),
    new SelectListItem("Kazakhstan", "Kazakhstan"),
    new SelectListItem("Kenya", "Kenya"),
    new SelectListItem("Kiribati", "Kiribati"),
    new SelectListItem("Korea, Democratic People's Republic of", "Korea, Democratic People's Republic of"),
    new SelectListItem("Korea, Republic of", "Korea, Republic of"),
    new SelectListItem("Kuwait", "Kuwait"),
    new SelectListItem("Kyrgyzstan", "Kyrgyzstan"),
    new SelectListItem("Lao People's Democratic Republic", "Lao People's Democratic Republic"),
    new SelectListItem("Latvia", "Latvia"),
    new SelectListItem("Lebanon", "Lebanon"),
    new SelectListItem("Lesotho", "Lesotho"),
    new SelectListItem("Liberia", "Liberia"),
    new SelectListItem("Libyan Arab Jamahiriya", "Libyan Arab Jamahiriya"),
    new SelectListItem("Liechtenstein", "Liechtenstein"),
    new SelectListItem("Lithuania", "Lithuania"),
    new SelectListItem("Luxembourg", "Luxembourg"),
    new SelectListItem("Macao", "Macao"),
    new SelectListItem("Macedonia, The Former Yugoslav Republic of", "Macedonia, The Former Yugoslav Republic of"),
    new SelectListItem("Madagascar", "Madagascar"),
    new SelectListItem("Malawi", "Malawi"),
    new SelectListItem("Malaysia", "Malaysia"),
    new SelectListItem("Maldives", "Maldives"),
    new SelectListItem("Mali", "Mali"),
    new SelectListItem("Malta", "Malta"),
    new SelectListItem("Marshall Islands", "Marshall Islands"),
    new SelectListItem("Martinique", "Martinique"),
    new SelectListItem("Mauritania", "Mauritania"),
    new SelectListItem("Mauritius", "Mauritius"),
    new SelectListItem("Mayotte", "Mayotte"),
    new SelectListItem("Mexico", "Mexico"),
    new SelectListItem("Micronesia, Federated States of", "Micronesia, Federated States of"),
    new SelectListItem("Moldova, Republic of", "Moldova, Republic of"),
    new SelectListItem("Monaco", "Monaco"),
    new SelectListItem("Mongolia", "Mongolia"),
    new SelectListItem("Montenegro", "Montenegro"),
    new SelectListItem("Montserrat", "Montserrat"),
    new SelectListItem("Morocco", "Morocco"),
    new SelectListItem("Mozambique", "Mozambique"),
    new SelectListItem("Myanmar", "Myanmar"),
    new SelectListItem("Namibia", "Namibia"),
    new SelectListItem("Nauru", "Nauru"),
    new SelectListItem("Nepal", "Nepal"),
    new SelectListItem("Netherlands", "Netherlands"),
    new SelectListItem("Netherlands Antilles", "Netherlands Antilles"),
    new SelectListItem("New Caledonia", "New Caledonia"),
        new SelectListItem("New Zealand", "New Zealand"),
    new SelectListItem("Nicaragua", "Nicaragua"),
    new SelectListItem("Niger", "Niger"),
    new SelectListItem("Nigeria", "Nigeria"),
    new SelectListItem("Niue", "Niue"),
    new SelectListItem("Norfolk Island", "Norfolk Island"),
    new SelectListItem("Northern Mariana Islands", "Northern Mariana Islands"),
    new SelectListItem("Norway", "Norway"),
    new SelectListItem("Oman", "Oman"),
    new SelectListItem("Pakistan", "Pakistan"),
    new SelectListItem("Palau", "Palau"),
    new SelectListItem("Palestinian Territory, Occupied", "Palestinian Territory, Occupied"),
    new SelectListItem("Panama", "Panama"),
    new SelectListItem("Papua New Guinea", "Papua New Guinea"),
    new SelectListItem("Paraguay", "Paraguay"),
    new SelectListItem("Peru", "Peru"),
    new SelectListItem("Philippines", "Philippines"),
    new SelectListItem("Pitcairn", "Pitcairn"),
    new SelectListItem("Poland", "Poland"),
    new SelectListItem("Portugal", "Portugal"),
    new SelectListItem("Puerto Rico", "Puerto Rico"),
    new SelectListItem("Qatar", "Qatar"),
    new SelectListItem("Reunion", "Reunion"),
    new SelectListItem("Romania", "Romania"),
    new SelectListItem("Russian Federation", "Russian Federation"),
    new SelectListItem("Rwanda", "Rwanda"),
    new SelectListItem("Saint Barthelemy", "Saint Barthelemy"),
    new SelectListItem("Saint Helena", "Saint Helena"),
    new SelectListItem("Saint Kitts and Nevis", "Saint Kitts and Nevis"),
    new SelectListItem("Saint Lucia", "Saint Lucia"),
    new SelectListItem("Saint Martin (French part)", "Saint Martin (French part)"),
    new SelectListItem("Saint Pierre and Miquelon", "Saint Pierre and Miquelon"),
    new SelectListItem("Saint Vincent and the Grenadines", "Saint Vincent and the Grenadines"),
    new SelectListItem("Samoa", "Samoa"),
    new SelectListItem("San Marino", "San Marino"),
    new SelectListItem("Sao Tome and Principe", "Sao Tome and Principe"),
    new SelectListItem("Saudi Arabia", "Saudi Arabia"),
    new SelectListItem("Senegal", "Senegal"),
    new SelectListItem("Serbia", "Serbia"),
    new SelectListItem("Seychelles", "Seychelles"),
    new SelectListItem("Sierra Leone", "Sierra Leone"),
    new SelectListItem("Singapore", "Singapore"),
    new SelectListItem("Sint Maarten (Dutch part)", "Sint Maarten (Dutch part)"),
    new SelectListItem("Slovakia", "Slovakia"),
    new SelectListItem("Slovenia", "Slovenia"),
    new SelectListItem("Solomon Islands", "Solomon Islands"),
    new SelectListItem("Somalia", "Somalia"),
    new SelectListItem("South Africa", "South Africa"),
    new SelectListItem("South Georgia and the South Sandwich Islands", "South Georgia and the South Sandwich Islands"),
    new SelectListItem("South Sudan", "South Sudan"),
    new SelectListItem("Spain", "Spain"),
    new SelectListItem("Sri Lanka", "Sri Lanka"),
    new SelectListItem("Sudan", "Sudan"),
    new SelectListItem("Suriname", "Suriname"),
    new SelectListItem("Svalbard and Jan Mayen", "Svalbard and Jan Mayen"),
    new SelectListItem("Swaziland", "Swaziland"),
    new SelectListItem("Sweden", "Sweden"),
    new SelectListItem("Switzerland", "Switzerland"),
    new SelectListItem("Syrian Arab Republic", "Syrian Arab Republic"),
    new SelectListItem("Taiwan, Province of China", "Taiwan, Province of China"),
    new SelectListItem("Tajikistan", "Tajikistan"),
    new SelectListItem("Tanzania, United Republic of", "Tanzania, United Republic of"),
    new SelectListItem("Thailand", "Thailand"),
    new SelectListItem("Timor-Leste", "Timor-Leste"),
    new SelectListItem("Togo", "Togo"),
    new SelectListItem("Tokelau", "Tokelau"),
    new SelectListItem("Tonga", "Tonga"),
    new SelectListItem("Trinidad and Tobago", "Trinidad and Tobago"),
    new SelectListItem("Tunisia", "Tunisia"),
    new SelectListItem("Turkey", "Turkey"),
    new SelectListItem("Turkmenistan", "Turkmenistan"),
    new SelectListItem("Turks and Caicos Islands", "Turks and Caicos Islands"),
    new SelectListItem("Tuvalu", "Tuvalu"),
    new SelectListItem("Uganda", "Uganda"),
    new SelectListItem("Ukraine", "Ukraine"),
    new SelectListItem("United Arab Emirates", "United Arab Emirates"),
    new SelectListItem("United Kingdom", "United Kingdom"),
    new SelectListItem("United States", "United States"),
    new SelectListItem("United States Minor Outlying Islands", "United States Minor Outlying Islands"),
    new SelectListItem("Uruguay", "Uruguay"),
    new SelectListItem("Uzbekistan", "Uzbekistan"),
    new SelectListItem("Vanuatu", "Vanuatu"),
    new SelectListItem("Venezuela, Bolivarian Republic of", "Venezuela, Bolivarian Republic of"),
    new SelectListItem("Viet Nam", "Viet Nam"),
    new SelectListItem("Virgin Islands, British", "Virgin Islands, British"),
    new SelectListItem("Virgin Islands, U.S.", "Virgin Islands, U.S."),
    new SelectListItem("Wallis and Futuna", "Wallis and Futuna"),
    new SelectListItem("Western Sahara", "Western Sahara"),
    new SelectListItem("Yemen", "Yemen"),
    new SelectListItem("Zambia", "Zambia"),
    new SelectListItem("Zimbabwe", "Zimbabwe")
};
    
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string ProviderDisplayName { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string ErrorMessage { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [EmailAddress]
            public string Email { get; set; }
            
            [Required]
            [StringLength(50)]
            [Display(Name = "First Name")]
            public string FirstName { get; set; }
            
            [Required]
            [StringLength(50)]
            [Display(Name = "Last Name")]
            public string LastName { get; set; }
            
            [Required]
            
            [Display(Name = "DOB")]
            public DateTime Dob { get; set; }
            
            [Required]
            [StringLength(50)]
            [Display(Name = "Country of Residence")]
            public string Country { get; set; }
            
            [Required]
            [StringLength(1)]
            [Display(Name = "Gender")]
            public string Gender { get; set; }
        }
        
        public IActionResult OnGet() => RedirectToPage("./Login");

        public IActionResult OnPost(string provider, string returnUrl = null)
        {
            // Request a redirect to the external login provider.
            var redirectUrl = Url.Page("./ExternalLogin", pageHandler: "Callback", values: new { returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return new ChallengeResult(provider, properties);
        }

        public async Task<IActionResult> OnGetCallbackAsync(string returnUrl = null, string remoteError = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            if (remoteError != null)
            {
                ErrorMessage = $"Error from external provider: {remoteError}";
                return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
            }
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                ErrorMessage = "Error loading external login information.";
                return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
            }

            // Sign in the user with this external login provider if the user already has a login.
            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);
            if (result.Succeeded)
            {
                _logger.LogInformation("{Name} logged in with {LoginProvider} provider.", info.Principal.Identity.Name, info.LoginProvider);
                return LocalRedirect(returnUrl);
            }
            if (result.IsLockedOut)
            {
                return RedirectToPage("./Lockout");
            }
            else
            {
                // If the user does not have an account, then ask the user to create an account.
                ReturnUrl = returnUrl;
                ProviderDisplayName = info.ProviderDisplayName;
                if (info.Principal.HasClaim(c => c.Type == ClaimTypes.Email))
                {
                    Input = new InputModel
                    {
                        Email = info.Principal.FindFirstValue(ClaimTypes.Email)
                    };
                }
                return Page();
            }
        }

        public async Task<IActionResult> OnPostConfirmationAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            // Get the information about the user from the external login provider
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                ErrorMessage = "Error loading external login information during confirmation.";
                return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
            }

            if (ModelState.IsValid)
            {
                var user = CreateUser();

                await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
                await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);

                var result = await _userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await _userManager.AddLoginAsync(user, info);
                    if (result.Succeeded)
                    {
                        _logger.LogInformation("User created an account using {Name} provider.", info.LoginProvider);

                        var newUser = new User
                        {
                            username = Input.Email
                        };
                        
                        _repo.AddUser(newUser);

                        _repo.Save();
                        
                        var customer = new Customer
                        {
                            customer_ID = newUser.user_id,
                            first_name = Input.FirstName,
                            last_name = Input.LastName,
                            birth_date = DateOnly.FromDateTime(Input.Dob),
                            country_of_residence = Input.Country,
                            gender = Input.Gender,
                            age = CalculateAge(Input.Dob) // Assuming you have a method to calculate age
                        };
                    
                  
                        _repo.AddCustomer(customer);
                        await _repo.SaveAsync();
                        

                        var userId = await _userManager.GetUserIdAsync(user);
                        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                        var callbackUrl = Url.Page(
                            "/Account/ConfirmEmail",
                            pageHandler: null,
                            values: new { area = "Identity", userId = userId, code = code },
                            protocol: Request.Scheme);

                        await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                            $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                        // If account confirmation is required, we need to show the link if we don't have a real email sender
                        if (_userManager.Options.SignIn.RequireConfirmedAccount)
                        {
                            return RedirectToPage("./RegisterConfirmation", new { Email = Input.Email });
                        }

                        await _signInManager.SignInAsync(user, isPersistent: false, info.LoginProvider);
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            ProviderDisplayName = info.ProviderDisplayName;
            ReturnUrl = returnUrl;
            return Page();
        }
        
        private byte CalculateAge(DateTime dob)
        {
            var age = DateTime.Today.Year - dob.Year;
            if (dob.Date > DateTime.Today.AddYears(-age)) age--;
            return (byte)age;
        }

        private IdentityUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<IdentityUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(IdentityUser)}'. " +
                    $"Ensure that '{nameof(IdentityUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the external login page in /Areas/Identity/Pages/Account/ExternalLogin.cshtml");
            }
        }

        private IUserEmailStore<IdentityUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<IdentityUser>)_userStore;
        }
    }
}
