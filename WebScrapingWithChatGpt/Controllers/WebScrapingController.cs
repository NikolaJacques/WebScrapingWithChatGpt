using Microsoft.AspNetCore.Mvc;
using WebScrapingWithChatGpt.Interfaces;
using WebScrapingWithChatGpt.Models;

namespace WebScrapingWithChatGpt.Controllers
{
    public class WebScrapingController : Controller
    {
        private readonly IWebScrapingService _scrapingService;
        public WebScrapingController(IWebScrapingService scrapingService)
        {
            _scrapingService = scrapingService;
        }

        [Route("api/scrape")]
        [ProducesResponseType<Contact>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost]
        public async Task<IActionResult> ScrapeContentAsync([FromBody] string[] html)
        {
            try
            {
                if (html == null)
                {
                    return BadRequest();
                } 

                Contact? result = await _scrapingService.ScrapeData<Contact>(html);
                
                if (result != null)
                {
                    return Ok(result);
                }
                else
                {
                    return Ok(new Contact());
                }
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
