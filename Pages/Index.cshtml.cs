using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebFormCRUD.Data;

namespace WebFormCRUD.Pages
{
    public class IndexModel : PageModel
    {
        private readonly AppDbContext _context;

        public IndexModel(AppDbContext context)
        {
            _context = context;
        }

        // Properties for creating a user
        [BindProperty]
        public string Name { get; set; }
        [BindProperty]
        public string Email { get; set; }
        [BindProperty]
        public int Age { get; set; }

        // List of users to display
        public List<User> Users { get; set; } = new();

        // Search properties
        [BindProperty(SupportsGet = true)]
        public string? SearchName { get; set; }
        [BindProperty(SupportsGet = true)]
        public string? SearchEmail { get; set; }
        [BindProperty(SupportsGet = true)]
        public int? SearchAge { get; set; }

        public async Task OnGetAsync()
        {
            // Query the database for users
            IQueryable<User> query = _context.Users;

            // Apply search filters
            if (!string.IsNullOrEmpty(SearchName))
            {
                query = query.Where(u => u.Name.Contains(SearchName));
            }

            if (!string.IsNullOrEmpty(SearchEmail))
            {
                query = query.Where(u => u.Email.Contains(SearchEmail));
            }

            if (SearchAge.HasValue)
            {
                query = query.Where(u => u.Age == SearchAge.Value);
            }

            // Execute the query and populate the Users list
            Users = await query.ToListAsync();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Check if the email already exists in the database
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == Email);
            if (existingUser != null)
            {
                ModelState.AddModelError(string.Empty, "This email is already registered.");
                return Page();
            }

            // Create a new user
            var user = new User
            {
                Name = Name,
                Email = Email,
                Age = Age
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return RedirectToPage();
        }
    }
}
