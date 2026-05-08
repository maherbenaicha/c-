using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniHub.DAL;
using UniHub.BL.Entities;
using UniHub.UI.ViewModels;

namespace UniHub.UI.Controllers
{
    [Authorize(Roles = "Student")]
    public class StudentController : Controller
    {
        private readonly UniHubDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public StudentController(UniHubDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // Dashboard étudiant
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null || !user.Department.HasValue)
                return RedirectToAction("AccessDenied", "Account", new { area = "Identity" });

            var myDepartmentActivities = _context.Activities != null
                ? await _context.Activities
                    .Where(a => a.Department == user.Department && a.IsPublished)
                    .Include(a => a.CreatedByUser)
                    .Include(a => a.Comments).ThenInclude(c => c.User)
                    .OrderByDescending(a => a.StartDate)
                    .ToListAsync()
                : new List<Activity>();

            var otherActivities = _context.Activities != null
                ? await _context.Activities
                    .Where(a => a.Department != user.Department && a.IsPublished)
                    .Include(a => a.CreatedByUser)
                    .Include(a => a.Comments).ThenInclude(c => c.User)
                    .OrderByDescending(a => a.StartDate)
                    .ToListAsync()
                : new List<Activity>();

            ViewBag.MyDepartmentActivities = myDepartmentActivities;
            ViewBag.OtherActivities = otherActivities;
            ViewBag.UserDepartment = user.Department;
            ViewBag.CurrentUserId = user.Id;

            return View();
        }

        // POST: Ajouter un commentaire sur un événement
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddComment(CommentViewModel model)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            if (ModelState.IsValid)
            {
                var comment = new ActivityComment
                {
                    ActivityId = model.ActivityId,
                    UserId = user.Id,
                    Content = model.Content,
                    CreatedDate = DateTime.Now
                };
                _context.ActivityComments?.Add(comment);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Commentaire ajouté.";
            }
            else
            {
                TempData["Error"] = "Commentaire invalide.";
            }

            return RedirectToAction(nameof(Index));
        }

        // POST: Supprimer son propre commentaire
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteComment(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            var comment = await _context.ActivityComments!.FindAsync(id);

            if (comment == null || comment.UserId != user?.Id)
                return NotFound();

            _context.ActivityComments!.Remove(comment);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Commentaire supprimé.";
            return RedirectToAction(nameof(Index));
        }

        // POST: Mettre à jour la photo de profil
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateProfilePhoto(IFormFile profilePhoto)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            if (profilePhoto != null && profilePhoto.Length > 0)
            {
                using var ms = new MemoryStream();
                await profilePhoto.CopyToAsync(ms);
                user.ProfilePicture = ms.ToArray();
                user.ProfilePictureContentType = profilePhoto.ContentType;
                await _userManager.UpdateAsync(user);
                TempData["Success"] = "Photo de profil mise à jour.";
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Photo de profil
        [AllowAnonymous]
        public async Task<IActionResult> ProfilePhoto(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user?.ProfilePicture == null) return NotFound();
            return File(user.ProfilePicture, user.ProfilePictureContentType ?? "image/jpeg");
        }

        // GET: Photo d'un événement (accessible aux étudiants)
        [AllowAnonymous]
        public async Task<IActionResult> EventPhoto(int id)
        {
            var activity = await _context.Activities!.FindAsync(id);
            if (activity?.EventPhoto == null) return NotFound();
            return File(activity.EventPhoto, activity.EventPhotoContentType ?? "image/jpeg");
        }
    }
}
