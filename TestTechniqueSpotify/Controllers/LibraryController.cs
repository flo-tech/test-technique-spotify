using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MvcLibrary.Data;
using MvcLibrary.Models;
using SpotifyAPI.Web;

namespace TestTechniqueSpotify.Controllers
{
    public class LibraryController : Controller
    {
        private readonly MvcLibraryContext _context;

        public LibraryController(MvcLibraryContext context)
        {
            _context = context;
        }

        private static string _client_id = "179724cefcb94ac3b707263bdf3b884c";
        private static string _client_secret = "0b21630d4ed84f38bde2e08a38b67e9b";

        // GET: Library
        public async Task<IActionResult> Index()
        {
            List<Library> LibraryList = await _context.Library.ToListAsync();

            return _context.Library != null ?
                        View(LibraryList.OrderBy(x => (((float)x.Taste + (float)x.Originality) / 2)).Reverse()) :
                        Problem("Entity set 'MvcLibraryContext.Library'  is null.");
        }


        // GET: Library/Search
        public async Task<IActionResult> Search(string? title)
        {
            var myList = _context.Library.ToList();

            myList.Clear();

            if (title == null)
            {
                ViewBag.library = myList;
                return View();
            }

            var config = SpotifyClientConfig.CreateDefault();

            var request = new ClientCredentialsRequest(_client_id, _client_secret);
            var response = await new OAuthClient(config).RequestToken(request);

            var spotify = new SpotifyClient(config.WithToken(response.AccessToken));

            try
            {
                var req = new SearchRequest(SearchRequest.Types.Track, title);
                var track = await spotify.Search.Item(req);
                if (track.Tracks.Items != null)
                {
                    track.Tracks.Items.ForEach(item =>
                    {
                        myList.Add(new Library
                        {
                            Title = item.Name,
                            Artist = item.Artists.First().Name,
                            AlbumName = item.Album.Name,
                            AlbumImage = item.Album.Images.First().Url,
                            Originality = 0,
                            Taste = 0
                        });
                    });
                }
            }
            catch (APIException e)
            {
                // Prints: invalid id
                Console.WriteLine(e.Message);
                // Prints: BadRequest
                Console.WriteLine(e.Response?.StatusCode);
            }

            ViewBag.library = myList;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Search([Bind("Id,Title,Artist,AlbumName,AlbumImage,Originality,Taste")] Library library)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(library);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            return View(library);
        }

        public async Task<IActionResult> ChangeTaste(int? ID, int TasteLv)
        {
            var library = await _context.Library.FindAsync(ID);
            if (library == null)
            {
                return _context.Library != null ?
                        View("Index", await _context.Library.ToListAsync()) :
                        Problem("Entity set 'MvcLibraryContext.Library'  is null.");
            }
            library.Taste = TasteLv + 1;
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(library);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LibraryExists(library.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return _context.Library != null ?
                        View("Index", await _context.Library.ToListAsync()) :
                        Problem("Entity set 'MvcLibraryContext.Library'  is null.");
        }

        public async Task<IActionResult> ChangeOriginality(int? ID, int OriginalityLv)
        {
            var library = await _context.Library.FindAsync(ID);
            if (library == null)
            {
                return _context.Library != null ?
                        View("Index", await _context.Library.ToListAsync()) :
                        Problem("Entity set 'MvcLibraryContext.Library'  is null.");
            }
            library.Originality = OriginalityLv + 1;
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(library);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LibraryExists(library.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            return _context.Library != null ?
                        View("Index", await _context.Library.ToListAsync()) :
                        Problem("Entity set 'MvcLibraryContext.Library'  is null.");
        }


        private bool LibraryExists(int id)
        {
            return (_context.Library?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
